using MythosAndHorrors.GameRules;
using System;
using System.Collections.Generic;
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
        [Inject] private readonly OwnersProvider _ownersProvider;

        /*******************************************************************/
        public void Execute()
        {
            string sceneSelected = _saveDataLoaderUseCase.DataSave.SceneSelected;
            Type type = (Assembly.GetAssembly(typeof(Scene)).GetType(typeof(Scene) + sceneSelected)
                ?? throw new InvalidOperationException("Scene not found " + sceneSelected));
            Scene scene = (Scene)_jsonService.CreateDataFromFile(type, _filesPath.JSON_SCENE_PATH(sceneSelected));
            _diContainer.Inject(scene);
            _ownersProvider.AddOwner(scene); //Order is important bcz CreateTokens need CurrentScene
            scene.Init();
            CreateTokens(scene);
        }

        private void CreateTokens(Scene scene)
        {
            List<ChallengeTokenType> challengeTokens = _chaptersProvider.CurrentDificulty switch
            {
                Dificulty.Easy => scene.ChallengeTokensEasy,
                Dificulty.Normal => scene.ChallengeTokensNormal,
                Dificulty.Hard => scene.ChallengeTokensHard,
                Dificulty.Expert => scene.ChallengeTokensExpert,
                _ => throw new InvalidOperationException("Dificulty not found"),
            };

            _challengeTokensProvider.CreateTokens(challengeTokens);
        }
    }
}
