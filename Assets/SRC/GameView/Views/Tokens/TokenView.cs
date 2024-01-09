using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    public class TokenView : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _amount;

        /*******************************************************************/
        public void SetAmount(int amount)
        {
            gameObject.SetActive(amount > 0);
            _amount.text = amount > 1 ? amount.ToString() : string.Empty;
        }

        public TokenView Clone()
        {
            TokenView newToken = Instantiate(this, transform.parent);
            newToken.SetAmount(0);
            newToken.name = name;
            newToken.transform.position += Vector3.up * 0.2f;
            return newToken;
        }
    }
}
