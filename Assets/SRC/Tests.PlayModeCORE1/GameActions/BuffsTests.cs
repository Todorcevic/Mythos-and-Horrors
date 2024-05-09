using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.TestTools;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class BuffsTests : TestBase
    {
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator BuffTest()
        {
            Card cardWithBuff = _cardsProvider.GetCard<Card01530>();
            IEnumerable<Buff> originalBuffs = cardWithBuff.Buffs.ToList();
            _buffsProvider.Remove(originalBuffs);
            CreateBuff(cardWithBuff);

            yield return _preparationScene.StartingScene();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardWithBuff, _investigatorsProvider.First.AidZone)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Intelligence + 1));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Intelligence + 1));

            _buffsProvider.Remove(cardWithBuff.Buffs);
            _buffsProvider.Add(originalBuffs);
        }

        [UnityTest]
        public IEnumerator DisablingBuffTest()
        {
            Card cardWithBuff = _cardsProvider.GetCard<Card01530>();
            IEnumerable<Buff> originalBuffs = cardWithBuff.Buffs.ToList();
            _buffsProvider.Remove(originalBuffs);
            CreateBuff(cardWithBuff);

            CardCreature creatureWithBuff = _cardsProvider.GetCard<Card01603>();
            IEnumerable<Buff> originalCreatureBuffs = creatureWithBuff.Buffs.ToList();
            _buffsProvider.Remove(originalCreatureBuffs);
            CreateCreatureBuff(creatureWithBuff);

            yield return _preparationScene.StartingScene();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(cardWithBuff, _investigatorsProvider.First.AidZone)).AsCoroutine();
            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Intelligence + 1));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Intelligence + 1));

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(creatureWithBuff, _investigatorsProvider.First.DangerZone)).AsCoroutine();

            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Intelligence));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Intelligence));

            yield return _gameActionsProvider.Create(new DiscardGameAction(creatureWithBuff)).AsCoroutine();
            Assert.That(_investigatorsProvider.First.Intelligence.Value, Is.EqualTo(_investigatorsProvider.First.InvestigatorCard.Info.Intelligence + 1));
            Assert.That(_investigatorsProvider.Second.Intelligence.Value, Is.EqualTo(_investigatorsProvider.Second.InvestigatorCard.Info.Intelligence + 1));

            _buffsProvider.Remove(cardWithBuff.Buffs);
            _buffsProvider.Add(originalBuffs);

            _buffsProvider.Remove(creatureWithBuff.Buffs);
            _buffsProvider.Add(originalCreatureBuffs);
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
