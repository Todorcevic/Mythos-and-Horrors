using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class EffectController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private List<EffectView> _effectViews;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public void AddEffects(List<Effect> effects)
        {
            foreach (Effect effect in effects)
            {
                EffectView effectView = GetEffectView();
                effectView.SetDescription(effect.Description);
                effectView.SetAvatarLeft(_avatarViewsManager.Get(effect.Investigator)?.Image);
                effectView.SetAvatarRight(_avatarViewsManager.Get(effect.InvestigatorAffected)?.Image);
            }
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
