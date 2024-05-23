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
            string sceneSelected = _saveDataLoaderUseCase.DataSave.SceneSelected;
            Type type = (Assembly.GetAssembly(typeof(Scene)).GetType(typeof(Scene) + sceneSelected)
                ?? throw new InvalidOperationException("Scene not found " + sceneSelected));
            object scene = _jsonService.CreateDataFromFile(type, _filesPath.JSON_SCENE_PATH(sceneSelected));
            _diContainer.Inject(scene);
            _chaptersProvider.SetCurrentScene((Scene)scene);
            CreateTokens((Scene)scene);
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
