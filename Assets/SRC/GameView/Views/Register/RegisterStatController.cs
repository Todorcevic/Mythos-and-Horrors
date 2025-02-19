using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class RegisterStatController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _image;

        /*******************************************************************/
        public void ShowXP(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ZenjectHelper.Instantiate(_image, transform).gameObject.SetActive(true);
            }
        }
    }
}
