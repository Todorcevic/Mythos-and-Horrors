using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class GlowComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _spriteRenderer;
        [SerializeField, Required, AssetsOnly] private Material _redMaterial;
        [SerializeField, Required, AssetsOnly] private Material _greenMaterial;

        /*******************************************************************/
        public Tween Off()
        {
            return _spriteRenderer.DOFade(0f, ViewValues.FAST_TIME_ANIMATION).OnComplete(() => gameObject.SetActive(false));
            //_spriteRenderer.enabled = false;
        }

        public Tween SetRedGlow()
        {
            return _spriteRenderer.DOFade(1f, ViewValues.FAST_TIME_ANIMATION).OnStart(Stariting);

            void Stariting()
            {
                _spriteRenderer.material = _redMaterial;
                gameObject.SetActive(true);
            }
        }
        public Tween SetGreenGlow()
        {
            return _spriteRenderer.DOFade(1f, ViewValues.FAST_TIME_ANIMATION).OnStart(Stariting);

            void Stariting()
            {
                _spriteRenderer.material = _greenMaterial;
                gameObject.SetActive(true);
            }
        }

        public void SetGreenGlowWithShader() //Podria no ser eficiente, ver documentacion de AllInOneShader
        {
            _spriteRenderer.material.SetColor("_Color", Color.green);
        }
    }
}
