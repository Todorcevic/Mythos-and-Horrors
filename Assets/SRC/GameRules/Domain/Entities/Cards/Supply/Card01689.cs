using System;
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
            CreateActivation(1, InvestigateLogic, InvestigateCondition, PlayActionType.Investigate);
            CreateForceReaction<ResolveChallengeGameAction>(ResolveCondition, ResolveLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task ResolveLogic(ResolveChallengeGameAction resolveChallengeGameAction)
        {
            await _gameActionsProvider.Create(new UpdateStatGameAction(resolveChallengeGameAction.ChallengePhaseGameAction.ActiveInvestigator.CurrentTurns, 0));
            CreateOneTimeReaction<OneInvestigatorTurnGameAction>(PassTurnCondition, PassTurnLogic, GameActionTime.Before);

            async Task PassTurnLogic(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                oneInvestigatorTurnGameAction.Cancel();
                await Task.CompletedTask;
            }

            bool PassTurnCondition(OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                return true;
            }
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
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (Charge.IsEmpty) return false;
            return true;
        }

        private async Task InvestigateLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(Charge.Amount, 1));
            _investigatePlaceGameAction = new(investigator, investigator.CurrentPlace);
            _investigatePlaceGameAction.ChangeStat(investigator.Power);
            _investigatePlaceGameAction.UpdateAmountHints(_investigatePlaceGameAction.AmountHints + 1);
            await _gameActionsProvider.Create(new IncrementStatGameAction(_investigatePlaceGameAction.StatModifier, 2));
            await _gameActionsProvider.Create(_investigatePlaceGameAction);
        }

        /*******************************************************************/
    }
}
