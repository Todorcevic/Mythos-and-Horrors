using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01509 : CardSupply, IDrawRevelation, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Charge Charge { get; private set; }
        public Stat ChargeFear { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item, Tag.Weakness };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ChargeFear = CreateStat(3);
            Charge = new Charge(ChargeFear, ChargeType.Special);
            CreateActivation(1, TakeFearLogic, TakeFearCondition, PlayActionType.Activate, new Localization("Activation_Card01509"));
            CreateForceReaction<RevealChallengeTokenGameAction>(ChangeTokenCondition, ChangeTokenLogic, GameActionTime.After);
        }

        /*******************************************************************/
        public Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public async Task PlayRevelationFor(Investigator investigator) =>
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(ChargeFear, 3).Execute();

        /*******************************************************************/
        private bool TakeFearCondition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (ChargeFear.Value < 1) return false;
            return true;
        }

        private async Task TakeFearLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(ControlOwner, fromCard: this, amountFear: 1).Execute();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(ChargeFear, 1).Execute();

            if (DiscardCondition()) await DiscardLogic();

            /*******************************************************************/
            async Task DiscardLogic()
            {
                await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
            }

            bool DiscardCondition()
            {
                if (IsInPlay.IsFalse) return false;
                if (ChargeFear.Value > 0) return false;
                return true;
            }
        }

        /*******************************************************************/
        private async Task ChangeTokenLogic(RevealChallengeTokenGameAction reavelChangeTokenGameAction)
        {
            await _gameActionsProvider.Create<RestoreChallengeTokenGameAction>().SetWith(reavelChangeTokenGameAction.ChallengeTokenRevealed).Execute();
            await _gameActionsProvider.Create<RevealChallengeTokenGameAction>().SetWith(_chaptersProvider.CurrentScene.FailToken, reavelChangeTokenGameAction.Investigator).Execute();
        }

        private bool ChangeTokenCondition(RevealChallengeTokenGameAction reavelChangeTokenGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (reavelChangeTokenGameAction.Investigator != ControlOwner) return false;
            if (reavelChangeTokenGameAction.ChallengeTokenRevealed != _chaptersProvider.CurrentScene.StarToken) return false;
            return true;
        }
    }
}
