using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CounterStatsController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private HealthCounterController _healthControllerPrefab;
        [SerializeField, Required, AssetsOnly] private SanityCounterController _sanityControllerPrefab;
        [SerializeField, Required, AssetsOnly] private BasicCounterController _hintControllerPrefab;
        [SerializeField, Required, AssetsOnly] private BasicCounterController _eldritchControllerPrefab;
        [SerializeField, Required, AssetsOnly] private PlotCounterController _plotControllerPrefab;
        [Inject] private readonly StatableManager _statableManager;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IDamageable damageable)
            {
                HealthCounterController newHealController = ZenjectHelper.Instantiate(_healthControllerPrefab, transform);
                newHealController.Init(damageable);
                _statableManager.Add(newHealController);
            }
            if (card is IFearable fearable)
            {
                SanityCounterController newFearController = ZenjectHelper.Instantiate(_sanityControllerPrefab, transform);
                newFearController.Init(fearable);
                _statableManager.Add(newFearController);
            }
            if (card is CardPlace cardPlace)
            {
                BasicCounterController newHintController = ZenjectHelper.Instantiate(_hintControllerPrefab, transform);
                newHintController.Init(cardPlace.Keys);
                _statableManager.Add(newHintController);
            }
            if (card is IEldritchable eldritchable)
            {
                BasicCounterController newEldritchController = ZenjectHelper.Instantiate(_eldritchControllerPrefab, transform);
                newEldritchController.Init(eldritchable.Eldritch);
                _statableManager.Add(newEldritchController);
            }
            if (card is CardPlot cardPlot)
            {
                PlotCounterController newPlotController = ZenjectHelper.Instantiate(_plotControllerPrefab, transform);
                newPlotController.Init(cardPlot);
                _statableManager.Add(newPlotController);
            }
        }
    }
}
