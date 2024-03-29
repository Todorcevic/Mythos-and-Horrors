using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class ChallengeComponent : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private Image _skill;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _token;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _investigator;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _challenge;

        //[SerializeField, Required, AssetsOnly] private Sprite _ancient;
        //[SerializeField, Required, AssetsOnly] private Sprite _creature;
        //[SerializeField, Required, AssetsOnly] private Sprite _cultist;
        //[SerializeField, Required, AssetsOnly] private Sprite _danger;
        //[SerializeField, Required, AssetsOnly] private Sprite _fail;
        //[SerializeField, Required, AssetsOnly] private Sprite _star;
        //[SerializeField, Required, AssetsOnly] private Sprite _void;

        [SerializeField, Required, AssetsOnly] private Sprite _strengh;
        [SerializeField, Required, AssetsOnly] private Sprite _agility;
        [SerializeField, Required, AssetsOnly] private Sprite _intelligence;
        [SerializeField, Required, AssetsOnly] private Sprite _power;

        /*******************************************************************/

        public void SetSkill(Sprite sprite)
        {
            _skill.sprite = sprite;
        }

        public void SetToken(Sprite sprite)
        {
            _token.sprite = sprite;
        }

        public void SetInvestigator(Sprite sprite)
        {
            _investigator.sprite = sprite;
        }

        public void SetDanger(Sprite sprite)
        {
            _challenge.sprite = sprite;
        }

    }
}

