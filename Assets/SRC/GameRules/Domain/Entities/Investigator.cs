using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Investigator : Owner
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        [JsonProperty("DeckBuildingConditions")] public Dictionary<Faction, int> DeckBuildingConditions { get; init; }
        [JsonProperty("Xp")] public Stat Xp { get; init; }
        [JsonProperty("Injury")] public Stat Injury { get; init; }
        [JsonProperty("Shock")] public Stat Shock { get; init; }
        [JsonProperty("IsDie")] public State IsDie { get; init; }
        public CardInvestigator InvestigatorCard => Cards.OfType<CardInvestigator>().First();
        public CardAvatar AvatarCard => Cards.OfType<CardAvatar>().First();
        public List<Card> RequerimentCards => Cards.FindAll(card => card.ExtraInfo?.IsRequired ?? false);
        public List<Card> PermanentCards => Cards.FindAll(card => card is IPermanentable);
        public List<Card> FullDeck => Cards.FindAll(card => card is not CardAvatar && card is not CardInvestigator && card is not IPermanentable);
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
        public int Position => _investigatorsProvider.GetInvestigatorPosition(this);
        public int HealthLeft => InvestigatorCard.Health.Value - DamageRecived.Value;
        public int SanityLeft => InvestigatorCard.Sanity.Value - FearRecived.Value;
        public int HandSize => HandZone.Cards.Count;
        public string Code => InvestigatorCard.Info.Code;
        public Card CardAidToDraw => DeckZone.Cards.LastOrDefault();
        public Card CardDangerToDraw => _chaptersProvider.CurrentScene.CardDangerToDraw;
        public CardPlace CurrentPlace => _cardsProvider.GetCardWithThisZone(AvatarCard.CurrentZone) as CardPlace;
        public IEnumerable<Card> CardsInPlay => Cards.Where(card => ZoneType.PlayZone.HasFlag(card.CurrentZone.ZoneType))
            .Union(AidZone.Cards); //But Cards not Owner how Lita
        public IEnumerable<Card> DiscardableCardsInHand => HandZone.Cards.Where(card => card.CanBeDiscarted.IsTrue);
        public IEnumerable<CardCreature> CreaturesInSamePlace => _cardsProvider.GetCards<CardCreature>()
          .Where(creature => creature.CurrentPlace != null && creature.CurrentPlace == CurrentPlace);
        private IEnumerable<CardColosus> ColosusConfronted => _cardsProvider.GetCards<CardCreature>()
            .Where(creature => creature.CurrentPlace == CurrentPlace && !creature.Exausted.IsActive).OfType<CardColosus>();
        public IEnumerable<CardCreature> AllTypeCreaturesConfronted => BasicCreaturesConfronted.Concat(ColosusConfronted.Cast<CardCreature>());
        public IEnumerable<CardCreature> BasicCreaturesConfronted => DangerZone.Cards.OfType<CardCreature>();
        public IEnumerable<CardCreature> NearestCreatures => _cardsProvider.GetCards<CardCreature>().Where(creature => creature.IsInPlay.IsTrue)
            .OrderBy(creature => creature.CurrentPlace.DistanceTo(CurrentPlace).distance);

        public Stat Health => InvestigatorCard.Health;
        public Stat Sanity => InvestigatorCard.Sanity;
        public Stat DamageRecived => InvestigatorCard.DamageRecived;
        public Stat FearRecived => InvestigatorCard.FearRecived;
        public Stat Strength => InvestigatorCard.Strength;
        public Stat Agility => InvestigatorCard.Agility;
        public Stat Intelligence => InvestigatorCard.Intelligence;
        public Stat Power => InvestigatorCard.Power;
        public Stat Resources => InvestigatorCard.Resources;
        public Stat Hints => InvestigatorCard.Hints;
        public Stat CurrentActions => InvestigatorCard.CurrentActions;
        public Stat MaxActions => InvestigatorCard.MaxActions;
        public Stat MaxHandSize => InvestigatorCard.MaxHandSize;
        public State Resign => InvestigatorCard.Resign;
        public State Defeated => InvestigatorCard.Defeated;
        public State IsPlayingTurns => InvestigatorCard.IsPlaying;
        public State Isolated => InvestigatorCard.Isolated;
        public Conditional CanPayHints => InvestigatorCard.CanPayHints;
        public Conditional CanBeHealed => InvestigatorCard.CanBeHealed;
        public Conditional CanBeRestoreSanity => InvestigatorCard.CanBeRestoreSanity;
        public Conditional HasTurnsAvailable => InvestigatorCard.HasActionsAvailable;
        public Conditional IsInPlay => InvestigatorCard.IsInPlay;

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
            InvestigatorCard.SetLazyStats(Injury.Value, Shock.Value);
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
