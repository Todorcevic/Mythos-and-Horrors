using MythsAndHorrors.GameRules;
using System;
using System.Reflection;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class CardBuilder
    {
        [Inject] private readonly DiContainer _sceneContainer;
        [Inject] private readonly CardInfoBuilder _cardInfoBuilder;

        /*******************************************************************/
        public Card BuildWith(CardInfo cardInfo) =>
            _sceneContainer.Instantiate(GetType(cardInfo.CardType), new object[] { cardInfo }) as Card;

        public T BuildOfType<T>() where T : Card => _sceneContainer.Instantiate<T>(new object[]
            {
                _cardInfoBuilder.CreateRandom().WithCardType(GetCardType<T>()).GiveMe()
            });

        public Card BuildRand() => BuildWith(_cardInfoBuilder.CreateRandom().GiveMe());

        private Type GetType(CardType cardType) =>
            Assembly.GetAssembly(typeof(Card)).GetType(typeof(Card) + cardType.ToString())
                ?? throw new InvalidOperationException("Card not found Type: " + cardType.ToString());

        private CardType GetCardType<T>() where T : Card => typeof(T) switch
        {
            Type type when type == typeof(CardInvestigator) => CardType.Investigator,
            Type type when type == typeof(CardSupply) => CardType.Supply,
            Type type when type == typeof(CardTalent) => CardType.Talent,
            Type type when type == typeof(CardCreature) => CardType.Creature,
            Type type when type == typeof(CardAdversity) => CardType.Adversity,
            Type type when type == typeof(CardCondition) => CardType.Condition,
            Type type when type == typeof(CardGoal) => CardType.Goal,
            Type type when type == typeof(CardPlot) => CardType.Plot,
            Type type when type == typeof(CardPlace) => CardType.Place,
            _ => throw new Exception(typeof(T) + " wrong CardType"),
        };
    }
}
