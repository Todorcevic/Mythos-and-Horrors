using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01121b : CardCreature, IStalker, ITarget, ISpawnable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public IReaction AvoidGainHintReaction { get; private set; }
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
           .OrderByDescending(investigator => investigator.Hints.Value).First();
        public CardPlace SpawnPlace => TargetInvestigator.CurrentPlace;
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist, Tag.Elite };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            RemoveStat(Health);
            Health = CreateStat((Info.Health ?? 0) + _investigatorsProvider.AllInvestigators.Count() * 2);
            CreateBuff(CardsToBuff, CantGainAndPayHintsBuff, RemoveCantGainAndPayHintsBuff, new Localization("Buff_Card01121b"));
            AvoidGainHintReaction = CreateForceReaction<GainHintGameAction>(CantGainHintsCondition, CantGainHintsLogic, GameActionTime.Initial);
            AvoidGainHintReaction.Disable();
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() =>
           ConfrontedInvestigator != null ? new List<Card> { ConfrontedInvestigator.InvestigatorCard } : Enumerable.Empty<Card>();

        private async Task CantGainAndPayHintsBuff(IEnumerable<Card> cards)
        {
            AvoidGainHintReaction.Enable();
            CardInvestigator investigatorCard = cards.Cast<CardInvestigator>().First();
            await _gameActionsProvider.Create<UpdateConditionalGameAction>().SetWith(investigatorCard.CanPayHints, false).Execute();
        }

        private async Task RemoveCantGainAndPayHintsBuff(IEnumerable<Card> cards)
        {
            AvoidGainHintReaction.Disable();
            CardInvestigator investigatorCard = cards.Cast<CardInvestigator>().First();
            await _gameActionsProvider.Create<ResetConditionalGameAction>().SetWith(investigatorCard.CanPayHints).Execute();
        }

        /*******************************************************************/
        bool CantGainHintsCondition(GainHintGameAction gainHintGameAction)
        {
            if (gainHintGameAction.Investigator != ConfrontedInvestigator) return false;
            return true;
        }

        async Task CantGainHintsLogic(GainHintGameAction gainHintGameAction)
        {
            gainHintGameAction.Cancel();
            await Task.CompletedTask;
        }
    }
}
