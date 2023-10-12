using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class GlowView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _spriteRenderer;
        [SerializeField, Required, AssetsOnly] private Material _redMaterial;
        [SerializeField, Required, AssetsOnly] private Material _greenMaterial;

        /*******************************************************************/
        public void Off()
        {
            _spriteRenderer.enabled = false;
        }

        public void SetRedGlow()
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.material = _redMaterial;
        }
        public void SetGreenGlow()
        {
            _spriteRenderer.enabled = true;
            _spriteRenderer.material = _greenMaterial;
        }

        public void SetGreenGlowWithShader() //Podria no ser eficiente, ver documentacion de AllInOneShader
        {
            _spriteRenderer.material.SetColor("_Color", Color.green);
        }
    }
}
