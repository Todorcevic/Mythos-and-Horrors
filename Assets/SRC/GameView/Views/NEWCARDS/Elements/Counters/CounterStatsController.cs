using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class CounterStatsController : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private HealthCounterController _healthController;
        [SerializeField, Required, ChildGameObjectsOnly] private SanityCounterController _sanityController;
        [SerializeField, Required, ChildGameObjectsOnly] private HintCounterController _hintController;
        [SerializeField, Required, ChildGameObjectsOnly] private EldritchCounterController _eldritchController;
        [SerializeField, Required, ChildGameObjectsOnly] private PlotCounterController _plotController;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IDamageable damageable) _healthController.Init(damageable);
            if (card is IFearable fearable) _sanityController.Init(fearable);
            if (card is CardPlace cardPlace) _hintController.Init(cardPlace);
            if (card is IEldritchable eldritchable) _eldritchController.Init(eldritchable);
            if (card is CardPlot cardPlot) _plotController.Init(cardPlot);
        }
    }
}
