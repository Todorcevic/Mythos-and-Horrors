using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class CounterController : MonoBehaviour
    {
        private readonly List<SpriteRenderer> _allCounts = new();
        [SerializeField, Required, AssetsOnly] private Sprite _fillCount;
        [SerializeField, Required, AssetsOnly] private Sprite _voidCount;
        [SerializeField, Required, AssetsOnly] private GameObject _column;

        public int AmountEnable => _allCounts.Count(spriteRenderer => spriteRenderer.gameObject.activeSelf);
        public SpriteRenderer LastShowed => _allCounts.LastOrDefault(spriteRenderer => spriteRenderer.sprite == _fillCount);
        public SpriteRenderer FirstVoid => _allCounts.FirstOrDefault(spriteRenderer => spriteRenderer.sprite == _voidCount);

        /*******************************************************************/
        public void EnableThisAmount(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                if (i >= _allCounts.Count) CreateNewColumn();
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

        private void CreateNewColumn()
        {
            _allCounts.AddRange(Instantiate(_column, transform).GetComponentsInChildren<SpriteRenderer>());
        }
    }
}
