using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01551 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandTurnsCost = CreateStat(0); //Podria hacerse que cambiara con un Reaction cuando estubiera en la mano del investigador
        }

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction chooseCreature = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01551"));

            foreach (CardCreature creature in investigator.CreaturesInSamePlace)
            {
                chooseCreature.CreateCardEffect(creature, investigator.InvestigatorAttackTurnsCost, AttackCreature, PlayActionType.Attack, investigator, new Localization("CardEffect_Card01551"));

                async Task AttackCreature()
                {
                    AttackCreatureGameAction playAttackGameAction = _gameActionsProvider.Create<AttackCreatureGameAction>()
                        .SetWith(investigator, creature, amountDamage: 3);
                    playAttackGameAction.ChangeStat(investigator.Agility);
                    await playAttackGameAction.Execute();
                }
            }

            await chooseCreature.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!investigator.CanAttack.IsTrue) return false;
            if (!investigator.CreaturesInSamePlace.Any()) return false;
            return true;
        }
    }
}
