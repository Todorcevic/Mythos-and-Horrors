using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeMeterComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos2;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _minos1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _zero;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus1;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _plus2;


        /*******************************************************************/


        private void Calculate()
        {

        }

    }
}

