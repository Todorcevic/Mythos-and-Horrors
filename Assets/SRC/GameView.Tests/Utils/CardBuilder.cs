using MythsAndHorrors.GameRules;
using System;
using Zenject;

namespace MythsAndHorrors.GameView.Tests
{
    public class CardBuilder
    {
        [Inject] private readonly CardGeneratorComponent _cardGenerator;
        [Inject] private readonly DiContainer SceneContainer;

        public CardView BuildOne(Faction faction = Faction.Brave)
        {
            Card card = SceneContainer.Instantiate<CardAdventurer>(new object[]
               {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Talent,
                        Code = "00001",
                        Name = "Adventurer1",
                        Faction = faction,
                        Health= 10,
                        Sanity=6,
                        Strength=2,
                        Agility=3,
                        Intelligence=4,
                        Power=0,
                        EnemyDamage=4,
                        EnemyFear=2,
                        Cost=5
                    }
               });

            return _cardGenerator.BuildCard(card);
        }

        public CardView[] BuildManySame(int count)
        {
            CardView[] cards = new CardView[count];
            for (int i = 0; i < count; i++)
            {
                cards[i] = BuildOne();
            }
            return cards;
        }

        public CardView[] BuildManyRandom(int count)
        {
            CardView[] cards = new CardView[count];
            for (int i = 0; i < count; i++)
            {
                cards[i] = BuildRand();
            }
            return cards;
        }

        public CardView BuildRand()
        {
            (CardType cardType, Type type) = GetRandomCardType();
            Random rand = new();

            Card card = (Card)SceneContainer.Instantiate(type, new object[]
               {
                    new CardInfo()
                    {
                        Description = GetRandomName(),
                        CardType = cardType,
                        Code = rand.Next(10000, 99999).ToString(),
                        Name = GetRandomName(),
                        Faction = (Faction)rand.Next(1,7),
                        Health = rand.Next(1,4),
                        Sanity = rand.Next(1,4),
                        Strength = rand.Next(1,2),
                        Agility = rand.Next(1,2),
                        Intelligence = rand.Next(1,2),
                        Power = rand.Next(1,2)
                    }
               });

            return _cardGenerator.BuildCard(card);
        }

        private (CardType cardType, Type type) GetRandomCardType()
        {
            Array values = Enum.GetValues(typeof(CardType));
            CardType randomCardType = (CardType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            Type cardType = null;
            switch (randomCardType)
            {
                case CardType.Adventurer:
                    cardType = typeof(CardAdventurer);
                    break;
                case CardType.Supply:
                    cardType = typeof(CardSupply);
                    break;
                case CardType.Talent:
                    cardType = typeof(CardTalent);
                    break;
                case CardType.Creature:
                    cardType = typeof(CardCreature);
                    break;
                case CardType.Adversity:
                    cardType = typeof(CardAdversity);
                    break;
                case CardType.Condition:
                    cardType = typeof(CardCondition);
                    break;
                case CardType.Goal:
                    cardType = typeof(CardGoal);
                    break;
                case CardType.Plot:
                    cardType = typeof(CardPlot);
                    break;
                case CardType.Place:
                    cardType = typeof(CardPlace);
                    break;
                default:
                    break;
            }
            return (randomCardType, cardType);
        }

        private string GetRandomName()
        {
            const string words = "abcdefghijklmnopqrstuvwxyz";
            Random random = new();
            char[] nameArray = new char[6];

            for (int i = 0; i < 6; i++)
            {
                int randomIndex = random.Next(words.Length);
                nameArray[i] = words[randomIndex];
            }

            return new string(nameArray);
        }
    }
}
