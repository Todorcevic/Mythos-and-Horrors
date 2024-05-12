using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.EditMode.Tests
{
    [TestFixture]
    public class BuffsTests : TestCORE1Base
    {
        private Card cardWithBuff;
        private IEnumerable<Buff> originalBuffs;

        [Test]
        public void SimpleBuff()
        {
            cardWithBuff = _cardsProvider.GetCard<Card01530>();
            originalBuffs = cardWithBuff.Buffs.ToList();
            _buffsProvider.Remove(originalBuffs);
            CreateBuff(cardWithBuff);
            _preparationSceneCORE1.StartingScene().Wait();

            _gameActionsProvider.Create(new MoveCardsGameAction(cardWithBuff, _investigatorsProvider.First.AidZone)).Wait();

            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Intelligence + 1));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Intelligence + 1));
        }

        [TearDown]
        public override void RunAfterAnyTest()
        {
            _buffsProvider.Remove(cardWithBuff.Buffs);
            _buffsProvider.Add(originalBuffs);
            base.RunAfterAnyTest();
        }

        private void CreateBuff(Card card)
        {
            _buffsProvider.Create()
              .SetCard(card)
              .SetDescription(nameof(AddIntelligenceBuff))
              .SetCardsToBuff(CardsToBuff)
              .SetAddBuff(AddIntelligenceBuff)
              .SetRemoveBuff(RemoveIntelligenceBuff);

            IEnumerable<Card> CardsToBuff()
            {
                if (BuffActivation()) return _investigatorsProvider.GetInvestigatorsInThisPlace(card.Owner.CurrentPlace)
                          .Select(investigator => investigator.InvestigatorCard);

                return Enumerable.Empty<Card>();

                bool BuffActivation() => _investigatorsProvider.GetInvestigatorWithThisZone(card.CurrentZone)?.AidZone == card.CurrentZone;
            }

            async Task AddIntelligenceBuff(IEnumerable<Card> cards)
            {
                Dictionary<Stat, int> map = cards.ToDictionary(card => ((CardInvestigator)card).Intelligence, card => 1);
                await _gameActionsProvider.Create(new IncrementStatGameAction(map));
            }

            async Task RemoveIntelligenceBuff(IEnumerable<Card> cards)
            {
                Dictionary<Stat, int> map = cards.ToDictionary(card => ((CardInvestigator)card).Intelligence, card => 1);
                await _gameActionsProvider.Create(new DecrementStatGameAction(map));
            }
        }

        private void CreateCreatureBuff(CardCreature creature)
        {
            _buffsProvider.Create()
              .SetCard(creature)
              .SetDescription(nameof(AddBlankBuff))
              .SetCardsToBuff(CardsToBuff)
              .SetAddBuff(AddBlankBuff)
              .SetRemoveBuff(RemoveBlankBuff);

            IEnumerable<Card> CardsToBuff()
            {
                return _investigatorsProvider.GetInvestigatorsInThisPlace(creature.CurrentPlace).SelectMany(investigator => investigator.CardsInPlay);
            }

            async Task AddBlankBuff(IEnumerable<Card> cards)
            {
                await _gameActionsProvider.Create(new UpdateStatesGameAction(cards.Select(card => card.Blancked), true));
            }

            async Task RemoveBlankBuff(IEnumerable<Card> cards)
            {
                await _gameActionsProvider.Create(new UpdateStatesGameAction(cards.Select(card => card.Blancked), false));
            }
        }
    }
}
