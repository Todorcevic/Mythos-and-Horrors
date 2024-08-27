using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01177 : CardCreature, ITarget
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderBy(investigator => investigator.HandSize).First();

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.Yithian };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<CreatureAttackGameAction>(DiscardCondition, DiscardLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task DiscardLogic(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (creatureAttackGameAction.Investigator.DiscardableCardsInHand.Any())
            {
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(creatureAttackGameAction.Investigator.DiscardableCardsInHand.Rand()).Execute();
            }
            else
            {
                Dictionary<Stat, int> stats = new()
                    {
                        { Damage, 1 },
                        { Fear, 1 }
                    };
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(stats).Execute();

                CreateOneTimeReaction<CreatureAttackGameAction>(RestoreCondition, RestoreLogic, GameActionTime.After);

                async Task RestoreLogic(CreatureAttackGameAction creatureAttackAfterGameAction)
                {
                    Dictionary<Stat, int> stats = new()
                    {
                        { Damage, 1 },
                        { Fear, 1 }
                    };
                    await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(stats).Execute();
                }

                bool RestoreCondition(CreatureAttackGameAction creatureAttackAfterGameAction)
                {
                    if (creatureAttackAfterGameAction != creatureAttackGameAction) return false;
                    return true;
                }
            }
        }

        private bool DiscardCondition(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (!IsInPlay.IsTrue) return false;
            if (creatureAttackGameAction.Creature != this) return false;
            return true;
        }

        /*******************************************************************/
    }

}
