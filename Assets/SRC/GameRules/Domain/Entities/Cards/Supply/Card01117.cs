using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01117 : CardSupply
    {
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Ally };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<AttackCreatureGameAction>(AttackCondition, AttackLogic, GameActionTime.Before);
            CreateBuff(CardsToBuff, GainStrength, RemoveGainStrenghtBuff, "Buff_Card01117");
        }

        /*******************************************************************/
        private async Task AttackLogic(AttackCreatureGameAction attackGameAction)
        {
            attackGameAction.SuccesEffects.Add(AttackSucceed);
            await Task.CompletedTask;

            /*******************************************************************/
            async Task AttackSucceed() => await _gameActionsProvider.Create<HarmToCardGameAction>()
                .SetWith(attackGameAction.CardCreature, this, amountDamage: 1).Execute();
        }

        private bool AttackCondition(AttackCreatureGameAction attackGameAction)
        {
            if (!IsInPlay) return false;
            if (attackGameAction.ActiveInvestigator.CurrentPlace != CurrentPlace) return false;
            if (!attackGameAction.CardCreature.HasThisTag(Tag.Monster)) return false;
            return true;
        }

        /*******************************************************************/
        private async Task GainStrength(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.OfType<CardInvestigator>().ToDictionary(card => card.Strength, card => 1);
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(map).Execute();
        }

        private async Task RemoveGainStrenghtBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.OfType<CardInvestigator>().ToDictionary(card => card.Strength, card => 1);
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(map).Execute();
        }

        private IEnumerable<Card> CardsToBuff()
        {
            return IsInPlay ? _investigatorProvider.AllInvestigatorsInPlay
                .Where(investigator => investigator.CurrentPlace == CurrentPlace).Select(investigator => investigator.InvestigatorCard) :
                Enumerable.Empty<Card>();
        }
    }
}
