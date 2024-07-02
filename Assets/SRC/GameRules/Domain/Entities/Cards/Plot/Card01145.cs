using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01145 : CardPlot, IRevealable
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        private SceneCORE3 SceneCORE3 => (SceneCORE3)_chaptersProvider.CurrentScene;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            CreateForceReaction<DefeatCardGameAction>(DefeatUrmodotCondition, DefeatUrmodotLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task DefeatUrmodotLogic(DefeatCardGameAction defeatGameAction)
        {
            await CompleteEffect();
        }

        private bool DefeatUrmodotCondition(DefeatCardGameAction defeatGameAction)
        {
            if (defeatGameAction.Card != SceneCORE3.Urmodoth) return false;
            if (!Revealed.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task RevealEffect(RevealGameAction revealGameAction)
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(RevealHistory, this).Start();

            if (SceneCORE3.CurrentGoal == SceneCORE3.GoalCards.ElementAt(0))
            {
                await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.Ritual, SceneCORE3.GetPlaceZone(1, 4)).Start();
            }
            else
            {
                await _gameActionsProvider.Create<SafeForeach<CardCreature>>().SetWith(CreaturesInRitual, Discard).Start();

                /*******************************************************************/
                async Task Discard(CardCreature creature)
                {
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(creature).Start();
                }

                IEnumerable<CardCreature> CreaturesInRitual()
                {
                    return _cardsProvider.GetCardsInPlay().OfType<CardCreature>().Where(card => card.CurrentPlace == SceneCORE3.Ritual);
                }
            }

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(SceneCORE3.CurrentGoal, SceneCORE3.OutZone).Start();
            await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(SceneCORE3.Urmodoth, SceneCORE3.Ritual).Start();
        }

        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[2]));

        }
    }
}
