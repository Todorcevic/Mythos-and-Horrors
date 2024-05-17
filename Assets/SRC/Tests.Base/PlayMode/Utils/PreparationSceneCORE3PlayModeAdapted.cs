using MythosAndHorrors.EditMode.Tests;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class PreparationSceneCORE3PlayModeAdapted : PreparationScenePlayModeAdapted
    {
        [Inject] private readonly PreparationSceneCORE3 _preparationSceneCORE3;

        /*******************************************************************/
        public SceneCORE3 SceneCORE3 => _preparationSceneCORE3.SceneCORE3;

        protected override MythosAndHorrors.EditMode.Tests.Preparation Preparation => _preparationSceneCORE3;
    }
}
