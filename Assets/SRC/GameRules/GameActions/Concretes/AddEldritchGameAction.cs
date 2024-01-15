using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class AddEldritchGameAction : GameAction
    {
        private int _amount;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        public async Task Run(int amount)
        {
            _amount = amount;
            await Start();
        }

        /*******************************************************************/

        protected override async Task ExecuteThisLogic()
        {
            _chaptersProvider.CurrentScene.CurrentPlot.Eldritch
                .UpdateValue(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch.Value + _amount);
            await Task.CompletedTask;
        }
    }
}
