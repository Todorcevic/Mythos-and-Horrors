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
            CreateActivation(CreateStat(1), ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley);
            CreateActivation(CreateStat(1), ThrowLitaActivate, ThrowLitaConditionToActivate, PlayActionType.Activate);
            CreateReaction<AttackCreatureGameAction>(AttackCondition, AttackLogic, GameActionTime.Before);
            CreateBuff(CardsToBuff, GainStrength, RemoveGainStrenghtBuff);
        }

        /*******************************************************************/
        private async Task AttackLogic(AttackCreatureGameAction attackGameAction)
        {
            attackGameAction.SuccesEffects.Add(AttackSucceed);
            await Task.CompletedTask;

            async Task AttackSucceed()
            {
                await _gameActionsProvider.Create(new HarmToCardGameAction(attackGameAction.CardCreature, this, amountDamage: 1));
            }
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
            await _gameActionsProvider.Create(new IncrementStatGameAction(map));
        }

        private async Task RemoveGainStrenghtBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> map = cards.OfType<CardInvestigator>().ToDictionary(card => card.Strength, card => 1);
            await _gameActionsProvider.Create(new DecrementStatGameAction(map));
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
            await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[3]));
        }

        /*******************************************************************/
        private async Task ParleyActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new ParleyGameAction(TakeLita));

            /*******************************************************************/
            async Task TakeLita() => await _gameActionsProvider.Create(new ChallengePhaseGameAction(
                    activeInvestigator.Intelligence, 4, "Parley with Lita", cardToChallenge: this, ParleySucceed, null));

            async Task ParleySucceed() => await _gameActionsProvider.Create(new MoveCardsGameAction(this, activeInvestigator.AidZone));
        }

        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            if (_chaptersProvider.CurrentScene is not SceneCORE1) return false;
            if (!IsInPlay) return false;
            if (activeInvestigator.AvatarCard.CurrentZone != CurrentZone) return false;
            if (Owner != null) return false;
            return true;
        }
    }
}
