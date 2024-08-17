using MythosAndHorrors.GameRules;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class InitialSettings : MonoBehaviour
    {
        [InjectOptional] private readonly bool _normalExecution = true;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [SerializeField] private bool _withoutAudio;
        [SerializeField] private bool _withoutMulligan;
        [SerializeField, Range(1, 64)] private float _gameVelocity = 1;
        [SerializeField] private Languaje _languaje;

        /*******************************************************************/
        private void Awake()
        {
            if (!_normalExecution) return;
            if (_withoutAudio)
            {
                AudioListener.volume = 0;
            }
            if (_withoutMulligan)
            {
                _reactionablesProvider.CreateReaction<MulliganGameAction>(CancelMulliganCondition, CancelMulligan, GameActionTime.Before);

                async Task CancelMulligan(MulliganGameAction mulliganGameAction)
                {
                    mulliganGameAction.Cancel();
                    await Task.CompletedTask;
                }

                bool CancelMulliganCondition(MulliganGameAction _) => true;
            }

            Time.timeScale = _gameVelocity;
        }
    }
}
