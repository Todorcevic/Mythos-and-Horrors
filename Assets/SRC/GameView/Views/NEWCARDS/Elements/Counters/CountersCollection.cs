using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class CountersCollection : MonoBehaviour
    {
        [SerializeField, Required, ChildGameObjectsOnly] private HealthCounterController _healthController;
        [SerializeField, Required, ChildGameObjectsOnly] private SanityCounterController _sanityController;
        [SerializeField, Required, ChildGameObjectsOnly] private HealthCounterController _doubleHealthController;
        [SerializeField, Required, ChildGameObjectsOnly] private HintCounterController _hintController;
        [SerializeField, Required, ChildGameObjectsOnly] private EldritchCounterController _eldritchController;

        /*******************************************************************/
        public void Init(Card card)
        {
            if (card is IDamageable damageable)
            {
                if (damageable.Health.Value > 10)
                    _doubleHealthController.Init(damageable);
                else
                    _healthController.Init(damageable);
            }

            if (card is IFearable fearable) _sanityController.Init(fearable);

            if (card is CardPlace cardPlace) _hintController.Init(cardPlace);

            if (card is CardPlot cardPlot) _eldritchController.Init(cardPlot);
        }
    }
}
