using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01509 : CardSupply, IDrawActivable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item, Tag.Weakness };

        public Stat ChargeFear { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraStat = ChargeFear = CreateStat(3);
            CreateActivation(CreateStat(1), TakeFearLogic, TakeFearCondition, PlayActionType.Activate);
            CreateForceReaction<RevealChallengeTokenGameAction>(ChangeTokenCondition, ChangeTokenLogic, GameActionTime.After);
        }

        /*******************************************************************/
        public Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public async Task PlayRevelationFor(Investigator investigator) =>
            await _gameActionsProvider.Create(new UpdateStatGameAction(ChargeFear, 3));

        /*******************************************************************/
        private bool TakeFearCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (ChargeFear.Value < 1) return false;
            return true;
        }

        private async Task TakeFearLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(ControlOwner, fromCard: this, amountFear: 1));
            await _gameActionsProvider.Create(new DecrementStatGameAction(ChargeFear, 1));

            if (DiscardCondition()) await DiscardLogic();

            /*******************************************************************/
            async Task DiscardLogic()
            {
                await _gameActionsProvider.Create(new DiscardGameAction(this));
            }

            bool DiscardCondition()
            {
                if (!IsInPlay) return false;
                if (ChargeFear.Value > 0) return false;
                return true;
            }
        }

        /*******************************************************************/
        private async Task ChangeTokenLogic(RevealChallengeTokenGameAction reavelChangeTokenGameAction)
        {
            await _gameActionsProvider.Create(new RestoreChallengeTokenGameAction(reavelChangeTokenGameAction.ChallengeTokenRevealed));
            await _gameActionsProvider.Create(new RevealChallengeTokenGameAction(_chaptersProvider.CurrentScene.FailToken, reavelChangeTokenGameAction.Investigator));
        }

        private bool ChangeTokenCondition(RevealChallengeTokenGameAction reavelChangeTokenGameAction)
        {
            if (!IsInPlay) return false;
            if (reavelChangeTokenGameAction.Investigator != ControlOwner) return false;
            if (reavelChangeTokenGameAction.ChallengeTokenRevealed != _chaptersProvider.CurrentScene.StarToken) return false;
            return true;
        }
    }
}
