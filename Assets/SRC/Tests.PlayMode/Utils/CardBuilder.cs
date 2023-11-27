using MythsAndHorrors.EditMode;
using System;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class CardBuilder
    {
        [Inject] private readonly DiContainer SceneContainer;

        /*******************************************************************/
        public Card SingleCard => SceneContainer.Instantiate<CardAdventurer>(new object[]
              {
                    new CardInfo()
                    {
                        Description = "DescriptionTest1",
                        CardType = CardType.Adventurer,
                        Code = "00001",
                        Name = "Adventurer1",
                        Faction = Faction.Brave,
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

        public Card SingleCard2 => SceneContainer.Instantiate<CardAdventurer>(new object[]
              {
                    new CardInfo()
                    {
                        Description = "DescriptionTest2",
                        CardType = CardType.Condition,
                        Code = "00002",
                        Name = "Condition2",
                        Faction = Faction.Cunning,
                        Health= 5,
                        Sanity=3,
                        Strength=6,
                        Agility=5,
                        Intelligence=3,
                        Power=2,
                        EnemyDamage=7,
                        EnemyFear=5,
                        Cost=1
                    }
              });

        public Card BuildRand()
        {
            (CardType cardType, Type type) = GetRandomCardType();
            Random rand = new();

            return (Card)SceneContainer.Instantiate(type, new object[]
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
        }

        private (CardType cardType, Type type) GetRandomCardType()
        {
            Array values = Enum.GetValues(typeof(CardType));
            CardType randomCardType = (CardType)values.GetValue(UnityEngine.Random.Range(1, values.Length));
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
