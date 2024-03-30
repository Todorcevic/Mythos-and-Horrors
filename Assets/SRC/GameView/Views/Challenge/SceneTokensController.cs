using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SceneTokensController : MonoBehaviour
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _ancientSceneTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _creatureSceneTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _dangerSceneTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _cultistSceneTokensView;

        /*******************************************************************/
        public void SetTokens()
        {
            _ancientSceneTokensView.SetChallengeToken(_chaptersProvider.CurrentScene.AncientToken);
            _creatureSceneTokensView.SetChallengeToken(_chaptersProvider.CurrentScene.CreatureToken);
            _dangerSceneTokensView.SetChallengeToken(_chaptersProvider.CurrentScene.DangerToken);
            _cultistSceneTokensView.SetChallengeToken(_chaptersProvider.CurrentScene.CultistToken);
        }

        public void UpdateValues()
        {
            _ancientSceneTokensView.UpdateValue();
            _creatureSceneTokensView.UpdateValue();
            _dangerSceneTokensView.UpdateValue();
            _cultistSceneTokensView.UpdateValue();
        }
    }
}

