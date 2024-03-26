using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeTokenView : MonoBehaviour
    {
        private ChallengeToken _type;
        [SerializeField, Required, ChildGameObjectsOnly] protected TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private Rigidbody _rigidBody;

        /*******************************************************************/
        public void Set(ChallengeToken type, int? amount)
        {
            _type = type;
            _value.text = amount?.ToString() ?? string.Empty;
        }

        public void PushUp()
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce(transform.up * Random.Range(1f, 10f), ForceMode.Impulse);
            _rigidBody.AddTorque(new Vector3(1, 2, 5), ForceMode.Impulse);
        }
    }
}
