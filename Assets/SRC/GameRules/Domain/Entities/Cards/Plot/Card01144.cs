﻿using System.Collections.Generic;
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
            CreateBuff(CardsToBuff, AddStrenghAndAgilityBuff, RemoveAddStrenghAndAgilityBlankBuff, new Localization("Buff_Card01144"));
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigatorsToChallenge, ChallengePower).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigatorsToChallenge() => _investigatorsProvider.AllInvestigatorsInPlay;

            async Task ChallengePower(Investigator investigator)
            {
                await _gameActionsProvider.Create<ChallengePhaseGameAction>()
                    .SetWith(investigator.Power, 6, new Localization("Challenge_Card01144", CurrentName), failEffect: AddMadness, cardToChallenge: this)
                    .Execute();

                /*******************************************************************/
                async Task AddMadness() => await _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, SceneCORE3.Haunteds.First(haunted => !haunted.IsInPlay.IsTrue)).Execute();
            }
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff() => IsInPlay.IsTrue ? _cardsProvider.GetCardsInPlay().OfType<CardCreature>() : Enumerable.Empty<Card>();

        private async Task AddStrenghAndAgilityBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (var card in cards.OfType<CardCreature>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
            }

            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(allStats).Execute();
        }
        private async Task RemoveAddStrenghAndAgilityBlankBuff(IEnumerable<Card> cards)
        {
            Dictionary<Stat, int> allStats = new();
            foreach (var card in cards.OfType<CardCreature>())
            {
                allStats[card.Strength] = 1;
                allStats[card.Agility] = 1;
            }

            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(allStats).Execute();
        }
    }
}
