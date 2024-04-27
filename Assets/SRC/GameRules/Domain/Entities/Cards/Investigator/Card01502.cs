using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01502 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        private bool _isReactionUsed;

        public Buff ExtraTurnBuff { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraTurnBuff = _buffsProvider.Create()
                        .SetCard(this)
                        .SetDescription(nameof(AddExtraTurnBuff))
                        .SetCardsToBuff(CardsToBuff)
                        .SetAddBuff(AddExtraTurnBuff)
                        .SetRemoveBuff(RemoveExtraTurnBuff);
        }

        /*******************************************************************/
        private async Task AddExtraTurnBuff(IEnumerable<Card> allTomes)
        {
            Dictionary<Stat, int> allStat = allTomes.Cast<IActivable>().ToDictionary(tome => tome.ActivateTurnsCost, tome => 1);
            await _gameActionsProvider.Create(new DecrementStatGameAction(allStat));
        }

        private async Task RemoveExtraTurnBuff(IEnumerable<Card> allTomes)
        {
            Dictionary<Stat, int> allStat = allTomes.Cast<IActivable>().ToDictionary(tome => tome.ActivateTurnsCost, tome => 1);
            await _gameActionsProvider.Create(new IncrementStatGameAction(allStat));
        }

        private IEnumerable<Card> CardsToBuff()
        {
            if (BuffActivation()) return _cardsProvider.AllCards.Where(card => card.Tags.Contains(Tag.Tome)
                && card.IsInPlay
                && card is IActivable activable
                && activable.ActivateTurnsCost.Value > 0);

            return Enumerable.Empty<Card>();

            bool BuffActivation()
            {
                if (_isReactionUsed) return false;
                if (!IsInPlay) return false;
                return true;
            }
        }


        /*******************************************************************/
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);
            if (gameAction is RoundGameAction) _isReactionUsed = false;
        }

        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            UseExtraTurnCondition(gameAction);

            void UseExtraTurnCondition(GameAction gameAction)
            {
                if (gameAction is not ActivateCardGameAction activateCardGameAction) return;
                if (_isReactionUsed) return;
                if (!IsInPlay) return;
                if (activateCardGameAction.Investigator != Owner) return;
                if (!((Card)activateCardGameAction.ActivableCard).Tags.Contains(Tag.Tome)) return;
                _isReactionUsed = true;
            }
        }

        /*******************************************************************/
        public override async Task StarEffect()
        {
            _gameActionsProvider.CurrentChallenge.SuccesEffects.Add(DrawCards);
            await Task.CompletedTask;
        }

        public override int StarValue() => 0;

        private async Task DrawCards() => await new SafeForeach<Card>(DrawAid, GetTomes).Execute();

        private async Task DrawAid(Card tome) => await _gameActionsProvider.Create(new DrawAidGameAction(Owner));

        private IEnumerable<Card> GetTomes() => Owner.CardsInPlay.Where(card => card.Tags.Contains(Tag.Tome));
    }
}
