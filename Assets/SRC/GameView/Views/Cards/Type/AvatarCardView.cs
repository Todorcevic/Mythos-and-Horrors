using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class AvatarCardView : CardView
    {
        [Title(nameof(AvatarCardView))]
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _health;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _resource;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _hint;
        [SerializeField, Required, ChildGameObjectsOnly] private StatView _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] private PayAsGroupController _payAsGroupController;

        public Investigator Investigator => Card.Owner;
        public int PayAsGroupValue => _payAsGroupController.CurrentValue;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            _health.SetStat(Investigator.Health);
            _resource.SetStat(Investigator.Resources);
            _hint.SetStat(Investigator.Hints);
            _sanity.SetStat(Investigator.Sanity);
            _payAsGroupController.Init();
        }

        public void ShowPayAsGroup()
        {
            _payAsGroupController.Show();
        }

        public void HidePayAsGroup()
        {
            _payAsGroupController.Hide();
        }
    }
}
