using MythsAndHorrors.GameRules;
using System;

namespace MythsAndHorrors.PlayMode.Tests
{
    public class CardInfoBuilder
    {
        private CardInfo currentCardInfo = new();

        /*******************************************************************/
        public CardInfo GiveMe() => currentCardInfo;

        public CardInfoBuilder WithName(string name)
        {
            currentCardInfo = currentCardInfo with { Name = name };
            return this;
        }

        public CardInfoBuilder WithDescription(string description)
        {
            currentCardInfo = currentCardInfo with { Description = description };
            return this;
        }

        public CardInfoBuilder WithCardType(CardType cardType)
        {
            currentCardInfo = currentCardInfo with { CardType = cardType };
            return this;
        }

        public CardInfoBuilder WithCode(string code)
        {
            currentCardInfo = currentCardInfo with { Code = code };
            return this;
        }

        public CardInfoBuilder WithFaction(Faction faction)
        {
            currentCardInfo = currentCardInfo with { Faction = faction };
            return this;
        }

        public CardInfoBuilder WithHealth(int? health)
        {
            currentCardInfo = currentCardInfo with { Health = health };
            return this;
        }

        public CardInfoBuilder WithSanity(int? sanity)
        {
            currentCardInfo = currentCardInfo with { Sanity = sanity };
            return this;
        }

        public CardInfoBuilder WithStrength(int? strength)
        {
            currentCardInfo = currentCardInfo with { Strength = strength };
            return this;
        }

        public CardInfoBuilder WithAgility(int? agility)
        {
            currentCardInfo = currentCardInfo with { Agility = agility };
            return this;
        }

        public CardInfoBuilder WithIntelligence(int? intelligence)
        {
            currentCardInfo = currentCardInfo with { Intelligence = intelligence };
            return this;
        }

        public CardInfoBuilder WithPower(int? power)
        {
            currentCardInfo = currentCardInfo with { Power = power };
            return this;
        }

        public CardInfoBuilder WithCost(int? cost)
        {
            currentCardInfo = currentCardInfo with { Cost = cost };
            return this;
        }

        public CardInfoBuilder WithEnigma(int? enigma)
        {
            currentCardInfo = currentCardInfo with { Enigma = enigma };
            return this;
        }

        public CardInfoBuilder WithHints(int? hints)
        {
            currentCardInfo = currentCardInfo with { Hints = hints };
            return this;
        }

        public CardInfoBuilder WithEnemyDamage(int? damage)
        {
            currentCardInfo = currentCardInfo with { EnemyDamage = damage };
            return this;
        }

        public CardInfoBuilder WithEnemyFear(int? fear)
        {
            currentCardInfo = currentCardInfo with { EnemyFear = fear };
            return this;
        }

        public CardInfoBuilder WithWild(int? wild)
        {
            currentCardInfo = currentCardInfo with { Wild = wild };
            return this;
        }

        public CardInfoBuilder WithEldritch(int? eldritch)
        {
            currentCardInfo = currentCardInfo with { Eldritch = eldritch };
            return this;
        }

        public CardInfoBuilder WithSlot(SlotType slot)
        {
            currentCardInfo = currentCardInfo with { Slots = new[] { slot } };
            return this;
        }

        public CardInfoBuilder CreateRandom()
        {
            Random rand = new();

            currentCardInfo = new CardInfo()
            {
                Description = GetRandomName(),
                CardType = GetRandomCardType(),
                Code = rand.Next(10000, 99999).ToString(),
                Name = GetRandomName(),
                Faction = (Faction)rand.Next(1, 6),
                Health = rand.Next(1, 4),
                Sanity = rand.Next(1, 4),
                Strength = rand.Next(1, 4),
                Agility = rand.Next(1, 4),
                Intelligence = rand.Next(1, 4),
                Power = rand.Next(1, 4),
                Cost = rand.Next(1, 4),
                Enigma = rand.Next(1, 4),
                Hints = rand.Next(1, 4),
                EnemyDamage = rand.Next(1, 4),
                EnemyFear = rand.Next(1, 4),
                Wild = rand.Next(1, 4),
                Eldritch = rand.Next(1, 4),
                Slots = new[] { SlotType.None }
            };
            return this;
        }

        private CardType GetRandomCardType()
        {
            Array values = Enum.GetValues(typeof(CardType));
            return (CardType)values.GetValue(UnityEngine.Random.Range(1, values.Length - 1));
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
