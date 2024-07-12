using MythosAndHorrors.GameRules;
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
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        public int EffectsAmount => _effectViews.Count(effectView => !effectView.IsEmpty);

        /*******************************************************************/
        public void AddEffects(IEnumerable<IViewEffect> effects)
        {
            foreach (IViewEffect effect in effects)
            {
                EffectView effectView = GetEffectView();
                effectView.SetDescription(effect.Description);
                effectView.SetAvatarLeft(_avatarViewsManager.GetByCode(effect.CardCode)?.Image);
                effectView.SetAvatarRight(_avatarViewsManager.GetByCode(effect.CardCodeSecundary)?.Image);
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
            EffectView newEffectView = Instantiate(_effectViews.First(), _effectViews.First().transform.parent);
            _effectViews.Add(newEffectView);
            return newEffectView;
        }
    }
}
