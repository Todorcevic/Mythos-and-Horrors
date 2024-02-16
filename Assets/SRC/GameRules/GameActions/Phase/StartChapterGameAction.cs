using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class StartChapterGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public ChapterInfo Chapter { get; }

        /*******************************************************************/
        public StartChapterGameAction(ChapterInfo chapter)
        {
            Chapter = chapter;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _viewLayerProvider.PlayAnimationWith(this);
        }
    }
}
