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
            CreateForceReaction<DefeatCardGameAction>(RevealCondition, RevealLogic, GameActionTime.After);
        }

        /*******************************************************************/
        protected bool RevealCondition(DefeatCardGameAction updateStatGameAction)
        {
            if (updateStatGameAction.Card != GhoulPriest) return false;
            if (Revealed.IsActive) return false;
            if (!IsInPlay.IsTrue) return false;
            return true;
        }

        protected async Task RevealLogic(DefeatCardGameAction updateStatGameAction) =>
            await _gameActionsProvider.Create<RevealGameAction>().SetWith(this).Execute();

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01110");
            interactableGameAction.CreateCardEffect(this, new Stat(0, false), BurnIt, PlayActionType.Choose, playedBy: _investigatorsProvider.Leader, "CardEffect_Card01110");
            interactableGameAction.CreateCardEffect(this, new Stat(0, false), NoBurn, PlayActionType.Choose, playedBy: _investigatorsProvider.Leader, "CardEffect_Card01110-1");
            await interactableGameAction.Execute();

            /*******************************************************************/
            async Task BurnIt()
            {
                await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(_chaptersProvider.CurrentScene.FullResolutions[1]).Execute();
            }

            async Task NoBurn()
            {
                await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(_chaptersProvider.CurrentScene.FullResolutions[2]).Execute();
            }
        }
    }
}
