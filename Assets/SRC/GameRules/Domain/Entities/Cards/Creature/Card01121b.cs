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

        public IReaction AvoidGainKeyReaction { get; private set; }
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
           .OrderByDescending(investigator => investigator.Keys.Value).First();
        public CardPlace SpawnPlace => TargetInvestigator.CurrentPlace;
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist, Tag.Elite };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            RemoveStat(Health);
            Health = CreateStat((Info.Health ?? 0) + _investigatorsProvider.AllInvestigators.Count() * 2);
            CreateBuff(CardsToBuff, CantGainAndPayKeyssBuff, RemoveCantGainAndPayKeysBuff, new Localization("Buff_Card01121b"));
            AvoidGainKeyReaction = CreateForceReaction<GainKeyGameAction>(CantGainKeysCondition, CantGainKeysLogic, GameActionTime.Initial);
            AvoidGainKeyReaction.Disable();
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() =>
           ConfrontedInvestigator != null ? new List<Card> { ConfrontedInvestigator.InvestigatorCard } : Enumerable.Empty<Card>();

        private async Task CantGainAndPayKeyssBuff(IEnumerable<Card> cards)
        {
            AvoidGainKeyReaction.Enable();
            CardInvestigator investigatorCard = cards.Cast<CardInvestigator>().First();
            await _gameActionsProvider.Create<UpdateConditionalGameAction>().SetWith(investigatorCard.CanPayKeys, false).Execute();
        }

        private async Task RemoveCantGainAndPayKeysBuff(IEnumerable<Card> cards)
        {
            AvoidGainKeyReaction.Disable();
            CardInvestigator investigatorCard = cards.Cast<CardInvestigator>().First();
            await _gameActionsProvider.Create<ResetConditionalGameAction>().SetWith(investigatorCard.CanPayKeys).Execute();
        }

        /*******************************************************************/
        bool CantGainKeysCondition(GainKeyGameAction gainKeyGameAction)
        {
            if (gainKeyGameAction.Investigator != ConfrontedInvestigator) return false;
            return true;
        }

        async Task CantGainKeysLogic(GainKeyGameAction gainKeyGameAction)
        {
            gainKeyGameAction.Cancel();
            await Task.CompletedTask;
        }
    }
}
