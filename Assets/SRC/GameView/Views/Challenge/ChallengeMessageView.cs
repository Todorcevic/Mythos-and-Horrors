using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{

    public class ChallengeMessageView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _image;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _value;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _effect;

        /*******************************************************************/
        public void SetMessage(string value, string effect, Sprite sprite)
        {
            _value.text = value;
            _effect.text = effect;
            _image.sprite = sprite;
            gameObject.SetActive(true);
        }
    }
}

