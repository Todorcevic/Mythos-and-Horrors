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

        /*******************************************************************/
        public void Init(Card card)
        {
            SetTags(card.Tags);
            SetDescription(card.Info.Description ?? card.Info.Flavor);
        }

        /*******************************************************************/
        private void SetTags(IEnumerable<Tag> Tags)
        {
            if (!Tags.Any()) _tags.gameObject.SetActive(false);
            _tags.text = string.Empty;
            Tags.ForEach(tag => _tags.text += $"{tag} ");
        }

        private void SetDescription(string description)
        {
            if (string.IsNullOrEmpty(description)) _description.gameObject.SetActive(false);
            _description.text = description.ParseDescription(_cardsProvider);
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
