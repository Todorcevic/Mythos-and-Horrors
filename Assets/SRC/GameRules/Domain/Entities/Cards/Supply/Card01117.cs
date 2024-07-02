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
        public CardCreature Urmodoth => _cardsProvider.TryGetCard<Card01157>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley);
            CreateActivation(1, ThrowLitaActivate, ThrowLitaConditionToActivate, PlayActionType.Activate, cardAffected: () => Urmodoth);
            CreateForceReaction<AttackCreatureGameAction>(AttackCondition, AttackLogic, GameActionTime.Before);
            CreateBuff(CardsToBuff, GainStrength, RemoveGainStrenghtBuff);
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

        /*******************************************************************/
        private bool ThrowLitaConditionToActivate(Investigator investigator)
        {
            if (_chaptersProvider.CurrentScene is not SceneCORE3) return false;
            if (!IsInPlay) return false;
            if (!Urmodoth?.IsInPlay ?? true) return false;
            if (CurrentPlace != Urmodoth.CurrentPlace) return false;
            if (investigator.CurrentPlace != Urmodoth.CurrentPlace) return false;
            return true;
        }

        private async Task ThrowLitaActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(_chaptersProvider.CurrentScene.FullResolutions[3]).Execute();
        }

        /*******************************************************************/
        private async Task ParleyActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<ParleyGameAction>().SetWith(TakeLita).Execute();

            /*******************************************************************/
            async Task TakeLita() => await _gameActionsProvider.Create<ChallengePhaseGameAction>()
                .SetWith(activeInvestigator.Intelligence, 4, "Parley with Lita", cardToChallenge: this, ParleySucceed, null)
                .Execute();

            async Task ParleySucceed() => await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(this, activeInvestigator.AidZone).Execute();
        }

        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            if (_chaptersProvider.CurrentScene is not SceneCORE1) return false;
            if (activeInvestigator.AvatarCard.CurrentZone != CurrentZone) return false;
            if (Owner != null) return false;
            return true;
        }
    }
}
