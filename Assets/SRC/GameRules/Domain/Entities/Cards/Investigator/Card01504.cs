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
        public Reaction<UpdateStatGameAction> DamageBySanity { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AbilityUsed = new State(false);
            DamageBySanity = new Reaction<UpdateStatGameAction>(DamageBySanityCondition, DamageBySanityLogic);
        }

        /*******************************************************************/
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);
            if (gameAction is IPhase) await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, false));
        }

        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await DamageBySanity.Check(gameAction);
        }

        /*******************************************************************/
        public override async Task StarEffect() => await Task.CompletedTask;

        public override int StarValue() => Owner.FearRecived;

        /*******************************************************************/
        private async Task DamageBySanityLogic(UpdateStatGameAction updateStatGameAction)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Harm Creature");
            interactableGameAction.CreateMainButton().SetLogic(Continue);

            foreach (CardCreature creature in Owner.CreaturesInSamePlace)
            {
                interactableGameAction.Create()
                .SetCard(creature)
                .SetInvestigator(Owner)
                .SetLogic(HarmCreature);

                /*******************************************************************/
                async Task HarmCreature()
                {
                    await _gameActionsProvider.Create(new HarmToCardGameAction(creature, Owner.InvestigatorCard, amountDamage: 1));
                    await _gameActionsProvider.Create(new UpdateStatesGameAction(AbilityUsed, true));
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);

            /*******************************************************************/
            async Task Continue() => await Task.CompletedTask;
        }

        private bool DamageBySanityCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (AbilityUsed.IsActive) return false;
            if (!updateStatGameAction.HasStat(Owner.Sanity)) return false;
            if (Owner.Sanity.Value >= Owner.Sanity.ValueBeforeUpdate) return false;
            if (!IsInPlay) return false;
            if (!Owner.CreaturesInSamePlace.Any()) return false;
            return true;
        }
    }
}
