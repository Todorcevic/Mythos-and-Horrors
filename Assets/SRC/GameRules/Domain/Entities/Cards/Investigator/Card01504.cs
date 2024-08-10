using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01504 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State AbilityUsed { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Sorcerer };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AbilityUsed = CreateState(false);
            CreateOptativeReaction<HarmToCardGameAction>(DamageBySanityCondition, DamageBySanityLogic, GameActionTime.After);
            CreateForceReaction<PhaseGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        protected override async Task StarEffect() => await Task.CompletedTask;

        protected override int StarValue() => Owner.FearRecived.Value;

        /*******************************************************************/
        private async Task DamageBySanityLogic(HarmToCardGameAction harmToCardGameAction)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, code: "Interactable_Card01504");
            foreach (CardCreature creature in Owner.CreaturesInSamePlace)
            {
                interactableGameAction.CreateEffect(creature, new Stat(0, false), HarmCreature, PlayActionType.Choose, Owner);

                /*******************************************************************/
                async Task HarmCreature()
                {
                    await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(creature, Owner.InvestigatorCard, amountDamage: 1).Execute();
                    await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, true).Execute();
                };
            }

            await interactableGameAction.Execute();
        }

        private bool DamageBySanityCondition(HarmToCardGameAction harmToCardGameAction)
        {
            if (harmToCardGameAction.Card != this) return false;
            if (harmToCardGameAction.TotalFearApply <= 0) return false;
            if (AbilityUsed.IsActive) return false;
            if (!IsInPlay) return false;
            if (!Owner.CreaturesInSamePlace.Any()) return false;
            return true;
        }

        /*******************************************************************/
        private async Task RestartAbilityLogic(PhaseGameAction phaseGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, false).Execute();
        }

        private bool RestartAbilityCondition(PhaseGameAction phaseGameAction)
        {
            if (!AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
