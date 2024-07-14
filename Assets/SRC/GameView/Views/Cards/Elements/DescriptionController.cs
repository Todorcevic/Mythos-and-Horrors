using DG.DemiEditor;
using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class DescriptionController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _tags;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshPro _description;

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
            Tags.ForEach(tag => _tags.text += $"[{tag}]");
        }

        private void SetDescription(string description)
        {
            if (description.IsNullOrEmpty()) _description.gameObject.SetActive(false);
            _description.text = description;
        }

        public Tween BlankAnimation() //TODO: Implementar animacion de fade out
        {
            return DOTween.Sequence();
        }

        public Tween UnblankAnimation() //TODO: Implementar animacion de fade in
        {
            return DOTween.Sequence();
        }
    }
}
