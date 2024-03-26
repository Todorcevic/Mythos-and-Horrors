using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeTokenView : MonoBehaviour
    {
        [SerializeField, Required] private ChallengeTokenType _type;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _value;
        [SerializeField, Required, ChildGameObjectsOnly] private Rigidbody _rigidBody;

        public ChallengeTokenType Type => _type;

        /*******************************************************************/
        public void SetValue(int? amount)
        {
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
