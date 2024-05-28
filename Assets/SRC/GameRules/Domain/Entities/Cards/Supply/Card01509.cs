using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01509 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item, Tag.Weakness };

        public Stat Fear { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraStat = Fear = CreateStat(3);
            CreateReaction<MoveCardsGameAction>(PrepareCondition, PrepareLogic, true);
            CreateReaction<RevealChallengeTokenGameAction>(ChangeTokenCondition, ChangeTokenLogic, false);
            CreateActivation(CreateStat(1), TakeFearLogic, TakeFearCondition);
            CreateReaction<UpdateStatGameAction>(DiscardCondition, DiscardLogic, false);
        }

        /*******************************************************************/
        private async Task DiscardLogic(UpdateStatGameAction action)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool DiscardCondition(UpdateStatGameAction updateStatGameAction)
        {
            if (!IsInPlay) return false;
            if (Fear.Value > 0) return false;
            return true;
        }

        /*******************************************************************/
        private bool TakeFearCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Fear.Value < 1) return false;
            return true;
        }

        private async Task TakeFearLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(ControlOwner, fromCard: this, amountFear: 1));
            await _gameActionsProvider.Create(new DecrementStatGameAction(Fear, 1));
        }

        /*******************************************************************/
        private async Task ChangeTokenLogic(RevealChallengeTokenGameAction reavelChangeTokenGameAction)
        {
            await _gameActionsProvider.Create(new RestoreChallengeToken(reavelChangeTokenGameAction.ChallengeTokenRevealed));
            await _gameActionsProvider.Create(new RevealChallengeTokenGameAction(_chaptersProvider.CurrentScene.FailToken, reavelChangeTokenGameAction.Investigator));
        }

        private bool ChangeTokenCondition(RevealChallengeTokenGameAction reavelChangeTokenGameAction)
        {
            if (!IsInPlay) return false;
            if (reavelChangeTokenGameAction.Investigator != ControlOwner) return false;
            if (reavelChangeTokenGameAction.ChallengeTokenRevealed != _chaptersProvider.CurrentScene.StarToken) return false;
            return true;
        }

        /*******************************************************************/
        private async Task PrepareLogic(MoveCardsGameAction moveCardGameAction)
        {
            moveCardGameAction.AllMoves[this] = new(ControlOwner.DangerZone, false);
            await _gameActionsProvider.Create(new UpdateStatGameAction(Fear, 3));
        }

        private bool PrepareCondition(MoveCardsGameAction moveCardGameAction)
        {
            if (!moveCardGameAction.Cards.Contains(this)) return false;
            if (moveCardGameAction.AllMoves[this].zone.ZoneType != ZoneType.Hand) return false;
            return true;
        }
    }
}
