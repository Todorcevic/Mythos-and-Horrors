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
            if (!Tags.Any()) _tags.gameObject.SetActive(false);
            _tags.text = string.Empty;
            Tags.ForEach(tag => _tags.text += $"{_textsManager.GetTagText(tag)} ");
        }

        private void SetDescription(Card card)
        {
            string finalDescription = card.Info.Description ?? card.Info.Flavor;
            if (string.IsNullOrEmpty(finalDescription)) _description.gameObject.SetActive(false);
            _description.text = finalDescription.ParseDescription(_cardsProvider, _textsManager);
        }

        public Tween BlankAnimation()
        {
            return _description.DOFade(0, ViewValues.DEFAULT_FADE);
        }

        public Tween UnblankAnimation()
        {
            return _description.DOFade(1, ViewValues.DEFAULT_FADE);
        }
    }
}
