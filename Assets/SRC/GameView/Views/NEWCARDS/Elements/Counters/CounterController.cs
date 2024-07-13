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
        protected SpriteRenderer LastShowed => _allCounts.LastOrDefault(spriteRenderer => spriteRenderer.sprite == _fillCount);
        protected SpriteRenderer FirstVoid => _allCounts.FirstOrDefault(spriteRenderer => spriteRenderer.sprite == _voidCount);

        /*******************************************************************/
        public void EnableThisAmount(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                EnableCount(_allCounts[i]);
            }

            for (int i = amount; i < _allCounts.Count; i++)
            {
                DisableCount(_allCounts[i]);
            }
        }

        public void ShowThisAmount(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _allCounts[i].sprite = _fillCount;
            }

            for (int i = amount; i < AmountEnable; i++)
            {
                _allCounts[i].sprite = _voidCount;
            }
        }

        private void EnableCount(SpriteRenderer count)
        {
            count.gameObject.SetActive(true);
            if (!count.transform.parent.gameObject.activeSelf)
                count.transform.parent.gameObject.SetActive(true);
        }

        private void DisableCount(SpriteRenderer count)
        {
            count.gameObject.SetActive(false);
            if (count.transform.parent.GetComponentsInChildren<SpriteRenderer>().All(spriteRenderer => !spriteRenderer.gameObject.activeSelf))
            {
                count.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
