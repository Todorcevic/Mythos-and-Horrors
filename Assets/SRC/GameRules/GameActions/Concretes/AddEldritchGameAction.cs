using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class AddEldritchGameAction : GameAction
    {
        private int _amount;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly IResourceMover _resourceMover;

        /*******************************************************************/
        public async Task Run(int amount)
        {
            _amount = amount;
            await Start();
        }

        /*******************************************************************/

        protected override async Task ExecuteThisLogic()
        {
            _chaptersProvider.CurrentScene.CurrentPlot.EldritchTotal += _amount;
            await Task.CompletedTask;
            //await _resourceMover.GainResource(null, _amount); //TODO updateStatPresenter
        }
    }
}
