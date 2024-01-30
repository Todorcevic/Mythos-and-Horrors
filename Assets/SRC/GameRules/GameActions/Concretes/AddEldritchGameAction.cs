using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class AddEldritchGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public int Amount { get; }

        /*******************************************************************/
        public AddEldritchGameAction(int amount)
        {
            Amount = amount;
        }

        /*******************************************************************/

        protected override async Task ExecuteThisLogic()
        {
            _chaptersProvider.CurrentScene.CurrentPlot.Eldritch
                .UpdateValue(_chaptersProvider.CurrentScene.CurrentPlot.Eldritch.Value + Amount);
            await Task.CompletedTask;
        }
    }
}
