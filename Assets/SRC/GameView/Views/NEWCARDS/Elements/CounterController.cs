using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class CounterController : MonoBehaviour
    {
        private Sprite _count;
        [SerializeField, Required, AssetsOnly] private Sprite _voidCount;
        [SerializeField, Required, ChildGameObjectsOnly] private List<SpriteRenderer> _allCounts;

        private IEnumerable<SpriteRenderer> AllActives => _allCounts.Where(spriteRenderer => spriteRenderer.gameObject.activeSelf);
        private IEnumerable<SpriteRenderer> AllFilled => _allCounts.Where(spriteRenderer => spriteRenderer.sprite != _voidCount);
        private IEnumerable<SpriteRenderer> AllVoid => _allCounts.Where(spriteRenderer => spriteRenderer.sprite == _voidCount);

        /*******************************************************************/
        public void SetWith(Sprite sprite, int amount)
        {
            _count = sprite;
            foreach (SpriteRenderer spriteRenderer in _allCounts.Take(amount))
            {
                spriteRenderer.sprite = _count;
                spriteRenderer.gameObject.SetActive(true);
            }
        }

        /*******************************************************************/
        public void EnableOne()
        {
            AllActives.Last().gameObject.SetActive(true);
        }

        public void DisableOne()
        {
            AllActives.Last().gameObject.SetActive(false);
        }

        public void FillOne()
        {
            AllVoid.First().sprite = _count;
        }

        public void VoidOne()
        {
            AllFilled.Last().sprite = _voidCount;
        }
    }
}
