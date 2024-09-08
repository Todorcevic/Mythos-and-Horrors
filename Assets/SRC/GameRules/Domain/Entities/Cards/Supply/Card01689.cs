using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01689 : CardSupply, IChargeable
    {
        private InvestigatePlaceGameAction _investigatePlaceGameAction;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell };
        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(3, ChargeType.MagicCharge);
            CreateActivation(1, InvestigateLogic, InvestigateCondition, PlayActionType.Activate | PlayActionType.Investigate, new Localization("Activation_Card01689"));
            CreateForceReaction<ResolveChallengeGameAction>(ResolveCondition, ResolveLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task ResolveLogic(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(resolveChallengeGameAction.ChallengePhaseGameAction.ActiveInvestigator.CurrentActions, 0).Execute();
            CreateOneTimeReaction<InvestigatorTurnGameAction>(PassTurnCondition, PassTurnLogic, GameActionTime.Before);

            async Task PassTurnLogic(InvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                oneInvestigatorTurnGameAction.Cancel();
                await Task.CompletedTask;
            }

            bool PassTurnCondition(InvestigatorTurnGameAction oneInvestigatorTurnGameAction) => true;
        }

        private bool ResolveCondition(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            if (resolveChallengeGameAction.ChallengePhaseGameAction != _investigatePlaceGameAction) return false;
            if (resolveChallengeGameAction.ChallengePhaseGameAction.ResultChallenge.TokensRevealed
                .All(token => ((int)token.TokenType) < 10 || ((int)token.TokenType) > 50)) return false;
            return true;
        }

        /*******************************************************************/
        private bool InvestigateCondition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (Charge.IsEmpty) return false;
            if (investigator != ControlOwner) return false;
            if (investigator.CurrentPlace.CanBeInvestigated.IsFalse) return false;
            return true;
        }

        private async Task InvestigateLogic(Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
               .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01689"));
            interactable.CreateCardEffect(investigator.CurrentPlace, new Stat(0, false), Investigate,
                PlayActionType.Choose, investigator, new Localization("CardEffect_Card01689"), cardAffected: this);
            await interactable.Execute();

            /*******************************************************************/
            async Task Investigate()
            {
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
                _investigatePlaceGameAction = _gameActionsProvider.Create<InvestigatePlaceGameAction>()
                    .SetWith(investigator, investigator.CurrentPlace);
                _investigatePlaceGameAction.ChangeStat(investigator.Power);
                _investigatePlaceGameAction.UpdateAmountKeys(_investigatePlaceGameAction.AmountKeys + 1);
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(_investigatePlaceGameAction.StatModifier, 2).Execute();
                await _investigatePlaceGameAction.Execute();
            }
        }
    }
}
