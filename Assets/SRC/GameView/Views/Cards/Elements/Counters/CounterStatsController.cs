using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class CounterStatsController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private HealthCounterController _healthController;
        [SerializeField, Required, AssetsOnly] private SanityCounterController _sanityController;
        [SerializeField, Required, AssetsOnly] private HintCounterController _hintController;
        [SerializeField, Required, AssetsOnly] private EldritchCounterController _eldritchController;
        [SerializeField, Required, AssetsOnly] private PlotCounterController _plotController;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IDamageable damageable)
            {
                Instantiate(_healthController, transform).Init(damageable);
            }
            if (card is IFearable fearable)
            {
                Instantiate(_sanityController, transform).Init(fearable);
            }
            if (card is CardPlace cardPlace)
            {
                Instantiate(_hintController, transform).Init(cardPlace);
            }
            if (card is IEldritchable eldritchable)
            {
                Instantiate(_eldritchController, transform).Init(eldritchable);
            }
            if (card is CardPlot cardPlot)
            {
                Instantiate(_plotController, transform).Init(cardPlot);
            }
        }
    }
}
