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
        public Stat Victory { get; private set; }
        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            Victory = CreateStat(2);
            RevealReaction = CreateReaction<DefeatCardGameAction>(RevealCondition, RevealLogic, false);
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

            await _gameActionsProvider.Create(interactableGameAction);

            /*******************************************************************/
            async Task BurnIt()
            {
                await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.VictoryZone));
                await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.Resolutions[1]));
            }

            async Task NoBurn()
            {
                await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.VictoryZone));
                await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.Resolutions[2]));
            }
        }
    }
}
