using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01144 : CardPlot
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        private SceneCORE3 SceneCORE3 => (SceneCORE3)_chaptersProvider.CurrentScene;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateBuff(CardsToBuff, AddStrenghAndAgilityBuff, RemoveAddStrenghAndAgilityBlankBuff);
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(() => _investigatorsProvider.AllInvestigatorsInPlay, ChallengePower));

            /*******************************************************************/
            async Task ChallengePower(Investigator investigator)
            {
                await _gameActionsProvider.Create(new ChallengePhaseGameAction(investigator.Power, 6, "Power Challenge", failEffect: AddMadness, cardToChallenge: this));

                /*******************************************************************/
                async Task AddMadness()
                {
                    await _gameActionsProvider.Create(new DrawGameAction(investigator, SceneCORE3.Haunteds.First(haunted => !haunted.IsInPlay)));
                }
            }
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() => IsInPlay ? _cardsProvider.GetCardsInPlay().OfType<CardCreature>() : Enumerable.Empty<Card>();

        private async Task AddStrenghAndAgilityBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (var card in cards.OfType<CardCreature>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
            }

            await _gameActionsProvider.Create(new IncrementStatGameAction(allStats));
        }
        private async Task RemoveAddStrenghAndAgilityBlankBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (var card in cards.OfType<CardCreature>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
            }

            await _gameActionsProvider.Create(new DecrementStatGameAction(allStats));
        }
    }
}
