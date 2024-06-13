using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01565 : CardConditionTrigged
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell, Tag.Spirit };
        protected override bool IsFast => true;
        protected override bool FastReactionAtStart => true;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandReaction.Disable();
            CreateReaction<PlayRevelationAdversityGameAction>(PlayFromHandCondition.IsTrueWith, Choose, isAtStart: FastReactionAtStart);
        }

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not PlayRevelationAdversityGameAction playRevelationAdversity) return false;
            if (playRevelationAdversity.Investigator != ControlOwner) return false;
            if (playRevelationAdversity.CardAdversity.HasThisTag(Tag.Weakness)) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not PlayRevelationAdversityGameAction playRevelationAdversity) return;
            playRevelationAdversity.Cancel();
            await _gameActionsProvider.Create(new DiscardGameAction(playRevelationAdversity.CardAdversity));
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountFear: 1));
        }

        /*******************************************************************/
        private async Task Choose(PlayRevelationAdversityGameAction playRevelationGameAction)
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(playRevelationGameAction.CardAdversity.FaceDown, false));

            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true,
                    mustShowInCenter: true,
                    "Optative Reaction",
                    playRevelationGameAction.Investigator);

            interactableGameAction.CreateEffect(this,
                new Stat(0, false),
                Cancel,
                PlayActionType.PlayFromHand,
                playedBy: playRevelationGameAction.Investigator);


            interactableGameAction.CreateEffect(playRevelationGameAction.CardAdversity,
                new Stat(0, false),
                Play,
                PlayActionType.Choose,
                playedBy: playRevelationGameAction.Investigator);


            await _gameActionsProvider.Create(interactableGameAction);

            /*******************************************************************/
            async Task Cancel() => await PlayFromHand(playRevelationGameAction);
            async Task Play() => await Task.CompletedTask;
        }
    }
}