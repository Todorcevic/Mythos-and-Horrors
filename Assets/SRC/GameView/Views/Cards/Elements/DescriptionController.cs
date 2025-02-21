using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class DescriptionController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _tags;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _description;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _flavor;
        [SerializeField, Required, ChildGameObjectsOnly] private SpriteRenderer _background;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly TextsManager _textsManager;

        /*******************************************************************/
        public void Init(Card card)
        {
            SetTags(card.Tags);
            SetDescription(card);
        }

        /*******************************************************************/
        private void SetTags(IEnumerable<Tag> Tags)
        {
            _tags.gameObject.SetActive(Tags.Any());
            _tags.text = string.Empty;
            Tags.ForEach(tag => _tags.text += $"{_textsManager.GetEnumToText(tag)} ");
        }

        public void SetDescription(Card card)
        {
            _flavor.gameObject.SetActive(!string.IsNullOrEmpty(card.CurrentFlavor));
            _flavor.text = $"“{card.CurrentFlavor ?? string.Empty}”";
            _description.gameObject.SetActive(!string.IsNullOrEmpty(card.CurrentDescription));
            _description.text = card.CurrentDescription?.ParseDescription(_cardsProvider, _textsManager, card.ControlOwner) ?? string.Empty;
        }

        public Tween BlankAnimation()
        {
            return _description.DOFade(0, ViewValues.MID_TIME_ANIMATION);
        }

        public Tween UnblankAnimation()
        {
            return _description.DOFade(1, ViewValues.MID_TIME_ANIMATION);
        }

        public Tween ExaustAnimation()
        {
            return _background.material.DOColor(ViewValues.EXAUST_COLOR, ViewValues.MID_TIME_ANIMATION);
        }

        public Tween UnexaustAnimation()
        {
            return _background.material.DOColor(Color.white, ViewValues.MID_TIME_ANIMATION);
        }
    }
}
