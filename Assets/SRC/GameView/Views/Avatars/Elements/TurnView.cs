using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TurnView : MonoBehaviour
    {
        public bool IsOn => gameObject.activeSelf;

        /*******************************************************************/
        public void TurnOn()
        {
            gameObject.SetActive(true);
        }

        public void TurnOff()
        {
            gameObject.SetActive(false);
        }
    }
}
