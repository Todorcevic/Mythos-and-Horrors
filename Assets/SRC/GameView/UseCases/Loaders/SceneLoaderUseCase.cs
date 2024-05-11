using MythosAndHorrors.GameRules;
using System;
using System.Reflection;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SceneLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly DiContainer _diContainer;
        [Inject] private readonly DataSaveUseCase _saveDataLoaderUseCase;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly ChallengeTokensProvider _challengeTokensProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        /*******************************************************************/
        public void Execute()
        {
            SceneInfo sceneInfo = _jsonService.CreateDataFromFile<SceneInfo>(_filesPath.JSON_SCENE_PATH(_saveDataLoaderUseCase.DataSave.SceneSelected));
            Type type = (Assembly.GetAssembly(typeof(Scene)).GetType(typeof(Scene) + sceneInfo.Code)
               ?? throw new InvalidOperationException("Scene not found" + sceneInfo.Code));
            Scene currentScene = (Scene)_diContainer.Instantiate(type, new object[] { sceneInfo });
            _chaptersProvider.SetCurrentScene(currentScene);
            CreateTokens(sceneInfo);
        }

        private void CreateTokens(SceneInfo sceneInfo)
        {
            var challengeTokens = _chaptersProvider.CurrentDificulty switch
            {
                Dificulty.Easy => sceneInfo.ChallengeTokensEasy,
                Dificulty.Normal => sceneInfo.ChallengeTokensNormal,
                Dificulty.Hard => sceneInfo.ChallengeTokensHard,
                Dificulty.Expert => sceneInfo.ChallengeTokensExpert,
                _ => throw new InvalidOperationException("Dificulty not found"),
            };

            _challengeTokensProvider.CreateTokens(challengeTokens);
        }

    }
}
