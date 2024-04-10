using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Investigator
    {
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        [JsonProperty("InvestigatorCard")] public CardInvestigator InvestigatorCard { get; init; }
        [JsonProperty("AvatarCard")] public CardAvatar AvatarCard { get; init; }
        [JsonProperty("Cards")] public List<Card> Cards { get; init; }
        [JsonProperty("RequerimentCard")] public List<Card> RequerimentCard { get; init; }
        [JsonProperty("DeckBuildingConditions")] public Dictionary<Faction, int> DeckBuildingConditions { get; init; }
        public Zone HandZone { get; private set; }
        public Zone DeckZone { get; private set; }
        public Zone DiscardZone { get; private set; }
        public Zone AidZone { get; private set; }
        public Zone DangerZone { get; private set; }
        public Zone InvestigatorZone { get; private set; }
        public SlotsCollection SlotsCollection { get; } = new();

        /*******************************************************************/
        public bool CanBeHealed => Health.Value < InitialHealth;
        public bool CanInvestigate => CurrentPlace.InvestigationTurnsCost.Value <= CurrentTurns.Value;
        public bool HasTurnsAvailable => CurrentTurns.Value > 0;
        public bool IsInPlay => InvestigatorZone.HasThisCard(InvestigatorCard);
        public int Position => _investigatorsProvider.GetInvestigatorPosition(this);
        public int DefaultHealth => InvestigatorCard.Info.Health ?? 0;
        public int DefaultSanity => InvestigatorCard.Info.Sanity ?? 0;
        public int InitialHealth => DefaultHealth - Injury.Value;
        public int InitialSanity => DefaultSanity - Shock.Value;
        public int DamageRecived => InitialHealth - Health.Value;
        public int FearRecived => InitialSanity - Sanity.Value;
        public int HandSize => HandZone.Cards.Count;
        public string Code => InvestigatorCard.Info.Code;
        public Card CardAidToDraw => DeckZone.Cards.LastOrDefault();
        public CardPlace CurrentPlace => _cardsProvider.GetCardWithThisZone(AvatarCard.CurrentZone) as CardPlace;
        public IEnumerable<Card> FullDeck => Cards.Concat(RequerimentCard);
        public IEnumerable<Card> AllCards => FullDeck.Concat(new[] { InvestigatorCard }).Concat(new[] { AvatarCard });
        public IEnumerable<Card> CardsInPlay => AllCards.Where(card => ZoneType.PlayZone.HasFlag(card.CurrentZone.ZoneType));
        public IEnumerable<CardCreature> CreaturesInSamePlace => _cardsProvider.AllCards.OfType<CardCreature>()
          .Where(creature => creature.CurrentPlace != null && creature.CurrentPlace == CurrentPlace);
        public Stat Health => InvestigatorCard.Health;
        public Stat Sanity => InvestigatorCard.Sanity;
        public Stat Strength => InvestigatorCard.Strength;
        public Stat Agility => InvestigatorCard.Agility;
        public Stat Intelligence => InvestigatorCard.Intelligence;
        public Stat Power => InvestigatorCard.Power;
        public Stat Xp => InvestigatorCard.Xp;
        public Stat Injury => InvestigatorCard.Injury;
        public Stat Shock => InvestigatorCard.Shock;
        public Stat Resources => InvestigatorCard.Resources;
        public Stat Hints => InvestigatorCard.Hints;
        public Stat CurrentTurns => InvestigatorCard.CurrentTurns;
        public Stat MaxTurns => InvestigatorCard.MaxTurns;
        public Stat MaxHandSize => InvestigatorCard.MaxHandSize;
        public Stat DrawTurnsCost => InvestigatorCard.DrawTurnsCost;
        public Stat TurnsCost => InvestigatorCard.TurnsCost;
        public Investigator NextInvestigator => _investigatorsProvider.Investigators.NextElementFor(this);
        public Investigator NextInvestigatorInPlay => _investigatorsProvider.AllInvestigatorsInPlay.NextElementFor(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            HandZone = _zonesProvider.Create(ZoneType.Hand);
            DeckZone = _zonesProvider.Create(ZoneType.InvestigatorDeck);
            DiscardZone = _zonesProvider.Create(ZoneType.InvestigatorDiscard);
            AidZone = _zonesProvider.Create(ZoneType.Aid);
            DangerZone = _zonesProvider.Create(ZoneType.Danger);
            InvestigatorZone = _zonesProvider.Create(ZoneType.Investigator);
        }

        /*******************************************************************/
        public bool HasThisZone(Zone zone) =>
            zone == HandZone || HandZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DeckZone || DeckZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DiscardZone || DiscardZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == AidZone || AidZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == DangerZone || DangerZone.Cards.Select(card => card.OwnZone).Contains(zone) ||
            zone == InvestigatorZone || InvestigatorZone.Cards.Select(card => card.OwnZone).Contains(zone);

        public bool HasThisCard(Card card) => AllCards.Contains(card);

        public ChallengeType GetChallengeType(Stat stat)
        {
            if (stat == Strength) return ChallengeType.Strength;
            if (stat == Agility) return ChallengeType.Agility;
            if (stat == Intelligence) return ChallengeType.Intelligence;
            if (stat == Power) return ChallengeType.Power;
            return ChallengeType.None;
        }
    }
}
