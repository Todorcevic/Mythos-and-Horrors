using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class CounterController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private Sprite _fillCount;
        [SerializeField, Required, AssetsOnly] private Sprite _voidCount;
        [SerializeField, Required, ChildGameObjectsOnly] private List<SpriteRenderer> _allCounts;

        protected int AmountEnable => _allCounts.Count(spriteRenderer => spriteRenderer.gameObject.activeSelf);

        /*******************************************************************/
        public void EnableThisAmount(int amount)
        {
            _allCounts.ForEach(spriteRenderer => spriteRenderer.gameObject.SetActive(false));
            _allCounts.Take(amount).ForEach(spriteRenderer => spriteRenderer.gameObject.SetActive(true));
        }

        public void ShowThisAmount(int amount)
        {
            _allCounts.ForEach(spriteRenderer => spriteRenderer.sprite = _voidCount);
            _allCounts.Take(amount).ForEach(spriteRenderer => spriteRenderer.sprite = _fillCount);
        }
    }
}
