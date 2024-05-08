using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01148 : CardGoal, IVictoriable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public IEnumerable<Investigator> InvestigatorsVictoryAffected => _investigatorsProvider.AllInvestigators;
        int IVictoriable.Victory => 5;
        bool IVictoriable.IsVictoryComplete => Revealed.IsActive;

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.VictoryZone));
            await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.Resolutions[1]));
        }
    }
}
