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
            CreateFastActivation(InvestigateLogic, InvestigateCondition, PlayActionType.Activate, new Localization("Activation_Card01689"));
            CreateForceReaction<ResolveChallengeGameAction>(ResolveCondition, ResolveLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task ResolveLogic(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(resolveChallengeGameAction.ChallengePhaseGameAction.ActiveInvestigator.CurrentTurns, 0).Execute();
            CreateOneTimeReaction<OneInvestigatorTurnGameAction>(PassTurnCondition, PassTurnLogic, GameActionTime.Before);

            async Task PassTurnLogic(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                oneInvestigatorTurnGameAction.Cancel();
                await Task.CompletedTask;
            }

            bool PassTurnCondition(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction) => true;
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
            if (!IsInPlay.IsTrue) return false;
            if (Charge.IsEmpty) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.CanInvestigate.IsTrue) return false;
            if (!investigator.CurrentPlace.CanBeInvestigated.IsTrue) return false;
            return true;
        }

        private async Task InvestigateLogic(Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
               .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01689"));
            interactable.CreateCardEffect(investigator.CurrentPlace, investigator.InvestigationTurnsCost, Investigate,
                PlayActionType.Investigate, investigator, new Localization("CardEffect_Card01689"), cardAffected: this);
            await interactable.Execute();

            /*******************************************************************/
            async Task Investigate()
            {
                await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
                _investigatePlaceGameAction = _gameActionsProvider.Create<InvestigatePlaceGameAction>()
                    .SetWith(investigator, investigator.CurrentPlace);
                _investigatePlaceGameAction.ChangeStat(investigator.Power);
                _investigatePlaceGameAction.UpdateAmountHints(_investigatePlaceGameAction.AmountHints + 1);
                await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(_investigatePlaceGameAction.StatModifier, 2).Execute();
                await _investigatePlaceGameAction.Execute();
            }
        }
    }
}
