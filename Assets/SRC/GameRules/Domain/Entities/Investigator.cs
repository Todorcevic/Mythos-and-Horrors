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
        public bool CanBeHealed => Health.Value < RealHealth;
        public bool CanInvestigate => CurrentPlace.InvestigationTurnsCost.Value <= CurrentTurns.Value;
        public bool HasTurnsAvailable => CurrentTurns.Value > 0;
        public bool IsInPlay => InvestigatorZone.HasThisCard(InvestigatorCard);
        public int Position => _investigatorsProvider.GetInvestigatorPosition(this);
        public int RealHealth => InvestigatorCard.RealHealth;
        public int RealSanity => InvestigatorCard.RealSanity;
        public int HandSize => HandZone.Cards.Count;
        public string Code => InvestigatorCard.Info.Code;
        public Card CardAidToDraw => DeckZone.Cards.LastOrDefault();
        public CardPlace CurrentPlace => _cardsProvider.GetCardWithThisZone(AvatarCard.CurrentZone) as CardPlace;
        public IEnumerable<Card> FullDeck => Cards.Concat(RequerimentCard);
        public IEnumerable<Card> AllCards => FullDeck.Concat(new[] { InvestigatorCard }).Concat(new[] { AvatarCard });
        public IEnumerable<CardCreature> CreaturesInSamePlace => _cardsProvider.AllCards.OfType<CardCreature>()
          .Where(creature => creature.CurrentPlace == CurrentPlace);
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
        public Stat ResourceTurnsCost => InvestigatorCard.ResourceTurnsCost;
        public Investigator NextInvestigator => _investigatorsProvider.Investigators.NextElementFor(this);
        public Investigator NextInvestigatorInPlay => _investigatorsProvider.AllInvestigatorsInPlay.NextElementFor(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            HandZone = _zonesProvider.Create();
            DeckZone = _zonesProvider.Create();
            DiscardZone = _zonesProvider.Create();
            AidZone = _zonesProvider.Create();
            DangerZone = _zonesProvider.Create();
            InvestigatorZone = _zonesProvider.Create();
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
    }
}
