using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01571 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Relic };
        public Charge Charge { get; private set; }
        public State Played { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(4, ChargeType.MagicCharge);
            Played = CreateState(false);
            CreateOptativeReaction<RevealRandomChallengeTokenGameAction>(PlayCondition, PlayLogic, GameActionTime.Before);
            CreateForceReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task PlayLogic(RevealRandomChallengeTokenGameAction reavealChallengeTokenGameAction)
        {
            reavealChallengeTokenGameAction.Cancel();

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Played, true).Execute();
            await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(ControlOwner).Execute();
            await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(ControlOwner).Execute();

            IEnumerable<ChallengeToken> allTokens = _challengeTokensProvider.ChallengeTokensRevealed;
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Choose token");
            foreach (ChallengeToken token in allTokens)
            {
                interactableGameAction.CreateEffect(this, new Stat(0, false), SelectToken, PlayActionType.Choose, ControlOwner);

                /*******************************************************************/
                async Task SelectToken() => await RestoreAllTokesn(allTokens.Except(new[] { token }));
            }

            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Execute();
            await interactableGameAction.Execute();
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Played, false).Execute();
        }

        private async Task RestoreAllTokesn(IEnumerable<ChallengeToken> allTokens)
        {

            await _gameActionsProvider.Create<SafeForeach<ChallengeToken>>().SetWith(AllTokens, LogicForToken).Execute();

            /*******************************************************************/
            async Task LogicForToken(ChallengeToken token) => await _gameActionsProvider.Create<RestoreChallengeTokenGameAction>().SetWith(token).Execute();
            IEnumerable<ChallengeToken> AllTokens() => allTokens;
        }

        private bool PlayCondition(RevealRandomChallengeTokenGameAction reavealChallengeTokenGameAction)
        {
            if (!IsInPlay) return false;
            if (Charge.Amount.Value < 1) return false;
            if (reavealChallengeTokenGameAction.Investigator != ControlOwner) return false;
            if (Played.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction updateStatGameAction)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        private bool DiscardCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (!updateStatGameAction.HasThisStat(Charge.Amount)) return false;
            if (Charge.Amount.Value > 0) return false;
            return true;
        }

        /*******************************************************************/
    }
}
