using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01110 : CardGoal, IVictoriable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();
        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;

        int IVictoriable.Victory => 2;
        bool IVictoriable.IsVictoryComplete => Revealed.IsActive;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Reveal.Disable();
            CreateReaction<DefeatCardGameAction>(RevealCondition, RevealLogic, false);
        }

        /*******************************************************************/
        protected bool RevealCondition(DefeatCardGameAction updateStatGameAction)
        {
            if (updateStatGameAction.Card != GhoulPriest) return false;
            if (Revealed.IsActive) return false;
            if (!IsInPlay) return false;
            return true;
        }

        protected async Task RevealLogic(DefeatCardGameAction updateStatGameAction) =>
            await _gameActionsProvider.Create(new RevealGameAction(this));

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Take decision", _investigatorsProvider.Leader);
            interactableGameAction.CreateEffect(this, BurnIt, PlayActionType.Choose, playedBy: _investigatorsProvider.Leader);
            interactableGameAction.CreateEffect(this, NoBurn, PlayActionType.Choose, playedBy: _investigatorsProvider.Leader);

            await _gameActionsProvider.Create(interactableGameAction);

            /*******************************************************************/
            async Task BurnIt()
            {
                await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[1]));
            }

            async Task NoBurn()
            {
                await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[2]));
            }
        }
    }
}
