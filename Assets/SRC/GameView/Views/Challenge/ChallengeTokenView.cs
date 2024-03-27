using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Threading.Tasks;
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

        public async Task PushUp()
        {
            _rigidBody.isKinematic = false;
            _rigidBody.AddForce(transform.up * Random.Range(100f, 200f), ForceMode.Impulse);
            _rigidBody.AddTorque(new Vector3(10, 20, 50), ForceMode.Impulse);

            while (!_rigidBody.IsSleeping())
            {
                await Task.Yield();
            }
        }
    }
}
