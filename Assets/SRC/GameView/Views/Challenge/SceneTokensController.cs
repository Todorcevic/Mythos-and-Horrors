using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SceneTokensController : MonoBehaviour
    {
        private bool isInitialized;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _ancientSceneTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _creatureSceneTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _dangerSceneTokensView;
        [SerializeField, Required, ChildGameObjectsOnly] SceneTokenView _cultistSceneTokensView;

        /*******************************************************************/
        private void SetTokens()
        {
            if (isInitialized) return;
            isInitialized = true;
            _ancientSceneTokensView.SetToken(_chaptersProvider.CurrentScene.AncientToken);
            _creatureSceneTokensView.SetToken(_chaptersProvider.CurrentScene.CreatureToken);
            _dangerSceneTokensView.SetToken(_chaptersProvider.CurrentScene.DangerToken);
            _cultistSceneTokensView.SetToken(_chaptersProvider.CurrentScene.CultistToken);
        }

        public void UpdateValues(Investigator investigator)
        {
            SetTokens();
            _ancientSceneTokensView.UpdateValue(investigator);
            _creatureSceneTokensView.UpdateValue(investigator);
            _dangerSceneTokensView.UpdateValue(investigator);
            _cultistSceneTokensView.UpdateValue(investigator);
        }

        public void Disable()
        {
            isInitialized = false;
        }
    }
}

