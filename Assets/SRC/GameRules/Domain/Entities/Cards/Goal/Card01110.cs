using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01110 : CardGoal
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public CardCreature GhoulPriest => _cardsProvider.GetCard<Card01116>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            RevealReaction = CreateFinishReaction<DefeatCardGameAction>(RevealCondition, RevealLogic);
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
        public override async Task CompleteEffect()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Take decision");

            interactableGameAction.Create()
                        .SetCard(this)
                        .SetInvestigator(_investigatorsProvider.Leader)
                        .SetCardAffected(this)
                        .SetLogic(BurnIt);


            interactableGameAction.Create()
                       .SetCard(this)
                       .SetInvestigator(_investigatorsProvider.Leader)
                       .SetCardAffected(this)
                       .SetLogic(NoBurn);

            /*******************************************************************/
            async Task BurnIt() => await _chaptersProvider.CurrentScene.Resolution1();

            async Task NoBurn() => await _chaptersProvider.CurrentScene.Resolution2();

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
