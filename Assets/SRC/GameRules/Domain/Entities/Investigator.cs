using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Investigator : Owner
    {
        [Inject] private readonly ZonesProvider _zonesProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        [JsonProperty("DeckBuildingConditions")] public Dictionary<Faction, int> DeckBuildingConditions { get; init; }
        public CardInvestigator InvestigatorCard => Cards.OfType<CardInvestigator>().First();
        public CardAvatar AvatarCard => Cards.OfType<CardAvatar>().First();
        public List<Card> RequerimentCard => Cards.FindAll(card => card.ExtraInfo?.IsRequired ?? false);
        public List<Card> FullDeck => Cards.FindAll(card => card is not CardAvatar && card is not CardInvestigator);
        public Zone HandZone => Zones.First(zone => zone.ZoneType == ZoneType.Hand);
        public Zone DeckZone => Zones.First(zone => zone.ZoneType == ZoneType.InvestigatorDeck);
        public Zone DiscardZone => Zones.First(zone => zone.ZoneType == ZoneType.InvestigatorDiscard);
        public Zone AidZone => Zones.First(zone => zone.ZoneType == ZoneType.Aid);
        public Zone DangerZone => Zones.First(zone => zone.ZoneType == ZoneType.Danger);
        public Zone InvestigatorZone => Zones.First(zone => zone.ZoneType == ZoneType.Investigator);
        public SlotsCollection SlotsCollection { get; } = new();
        public IEnumerable<SlotType> AllSlotsInPlay => CardsInPlay.OfType<CardSupply>().SelectMany(card => card.Info.Slots);
        public IEnumerable<SlotType> GetAllSlotsExeded()
        {
            List<SlotType> slots = new();
            List<SlotType> freeSlots = SlotsCollection.AllSlotsType.ToList();
            foreach (SlotType slot in AllSlotsInPlay)
            {
                if (slot == SlotType.None) continue;
                if (freeSlots.Contains(slot)) freeSlots.Remove(slot);
                else slots.Add(slot);
            }
            return slots;
        }

        public bool HasSlotsExeded => GetAllSlotsExeded().Any();

        /*******************************************************************/
        public bool CanBeHealed => Health.Value < InitialHealth;
        public bool CanBeRestoreSanity => Sanity.Value < InitialSanity;
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
        public int AmountCardsInPlay => CardsInPlay.Count();
        public string Code => InvestigatorCard.Info.Code;
        public Card CardAidToDraw => DeckZone.Cards.LastOrDefault();
        public Card CardDangerToDraw => _chaptersProvider.CurrentScene.CardDangerToDraw;
        public CardPlace CurrentPlace => _cardsProvider.GetCardWithThisZone(AvatarCard.CurrentZone) as CardPlace;
        public IEnumerable<Card> CardsInPlay => Cards.Where(card => ZoneType.PlayZone.HasFlag(card.CurrentZone.ZoneType))
            .Union(AidZone.Cards); //But Cards not Owner how Lita
        public IEnumerable<Card> DiscardableCardsInHand => HandZone.Cards.Where(card => card.CanBeDiscarded);
        public IEnumerable<CardCreature> CreaturesInSamePlace => _cardsProvider.GetCards<CardCreature>()
          .Where(creature => creature.CurrentPlace != null && creature.CurrentPlace == CurrentPlace);

        private IEnumerable<CardColosus> ColosusConfronted => _cardsProvider.GetCards<CardCreature>()
            .Where(creature => creature.CurrentPlace == CurrentPlace && !creature.Exausted.IsActive).OfType<CardColosus>();
        public IEnumerable<CardCreature> CreaturesEnganged => DangerZone.Cards.OfType<CardCreature>().Concat(ColosusConfronted.Cast<CardCreature>());
        public IEnumerable<CardCreature> NearestCreatures => _cardsProvider.GetCards<CardCreature>().Where(creature => creature.IsInPlay)
            .OrderBy(creature => creature.CurrentPlace.DistanceTo(CurrentPlace).distance);

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
        public State Resign => InvestigatorCard.Resign;
        public State Defeated => InvestigatorCard.Defeated;
        public State IsPlayingTurns => InvestigatorCard.IsPlaying;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            Zones.Add(new(ZoneType.Hand));
            Zones.Add(new(ZoneType.InvestigatorDeck));
            Zones.Add(new(ZoneType.InvestigatorDiscard));
            Zones.Add(new(ZoneType.Aid));
            Zones.Add(new(ZoneType.Danger));
            Zones.Add(new(ZoneType.Investigator));
            //SlotsCollection = new(this);
        }

        /*******************************************************************/
        public bool HasThisOwnerZone(Zone zone) =>
          zone == HandZone ||
          zone == DeckZone ||
          zone == DiscardZone ||
          zone == AidZone ||
          zone == InvestigatorZone;

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
