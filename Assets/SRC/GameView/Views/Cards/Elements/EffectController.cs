﻿using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class EffectController : MonoBehaviour
    {
        private const float REVERSE_OFFSET_Z = 0.003f;
        private const float OFFSET_Z = -0.001f;

        [SerializeField, Required, ChildGameObjectsOnly] private List<EffectView> _effectViews;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly TextsManager _textsProvider;

        public int EffectsAmount => _effectViews.Count(effectView => !effectView.IsEmpty);

        /*******************************************************************/
        public void AddEffects(IEnumerable<IViewEffect> effects)
        {
            foreach (IViewEffect effect in effects)
            {
                EffectView effectView = GetEffectView();
                effectView.SetDescription(_textsProvider.GetLocalizableText(effect.Localization));
                if (effect.CardCode != null)
                    effectView.SetPictureLeft(_cardViewsManager.GetCardView(effect.CardCode)?.Picture);
                if (effect.CardCodeSecundary != null)
                    effectView.SetPictureRight(_cardViewsManager.GetCardView(effect.CardCodeSecundary)?.Picture);
            }
        }

        public void Clear() => _effectViews.FindAll(effectView => !effectView.IsEmpty)
            .ForEach(effectView => effectView.Clear());

        public void Rotate(bool faceDown)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, faceDown ? 180 : 0, transform.localEulerAngles.z);
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, faceDown ? REVERSE_OFFSET_Z : OFFSET_Z);
        }

        private EffectView GetEffectView() => _effectViews.Find(effectView => effectView.IsEmpty) ?? CreateNew();

        private EffectView CreateNew()
        {
            EffectView newEffectView = ZenjectHelper.Instantiate(_effectViews.First(), _effectViews.First().transform.parent);
            _effectViews.Add(newEffectView);
            return newEffectView;
        }
    }
}
