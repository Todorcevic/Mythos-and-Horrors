using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class CounterStatsController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private HealthCounterController _healthControllerPrefab;
        [SerializeField, Required, AssetsOnly] private SanityCounterController _sanityControllerPrefab;
        [SerializeField, Required, AssetsOnly] private HintCounterController _hintControllerPrefab;
        [SerializeField, Required, AssetsOnly] private EldritchCounterController _eldritchControllerPrefab;
        [SerializeField, Required, AssetsOnly] private PlotCounterController _plotControllerPrefab;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IDamageable damageable)
            {
                Instantiate(_healthControllerPrefab, transform).Init(damageable);
            }
            if (card is IFearable fearable)
            {
                Instantiate(_sanityControllerPrefab, transform).Init(fearable);
            }
            if (card is CardPlace cardPlace)
            {
                Instantiate(_hintControllerPrefab, transform).Init(cardPlace);
            }
            if (card is IEldritchable eldritchable)
            {
                Instantiate(_eldritchControllerPrefab, transform).Init(eldritchable);
            }
            if (card is CardPlot cardPlot)
            {
                Instantiate(_plotControllerPrefab, transform).Init(cardPlot);
            }
        }
    }
}
