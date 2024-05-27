using MythosAndHorrors.GameRules;
using System;
using System.Linq;
using System.Threading.Tasks;
using Zenject;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class FakePayAsGroupPresenter : IAsGroupPresenter
    {
        private const float TIMEOUT = 3f;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;


        private PayHintsToGoalGameAction _payHintsToGoalGameAction;
        private TaskCompletionSource<Dictionary<Card, int>> waitForClicked = new();

        Dictionary<Investigator, int> amaunt;

        /*******************************************************************/
        public IEnumerator ClickAvatarUpDown(Investigator investigator, bool isUp)
        {
            float startTime = Time.realtimeSinceStartup;
            while (Time.realtimeSinceStartup - startTime < TIMEOUT && !AnyEffectInCard()) yield return null;
            if (isUp) amaunt[investigator]++;
            else amaunt[investigator]--;

            /*******************************************************************/
            bool AnyEffectInCard() => amaunt?.Any() ?? false;
        }

        /*******************************************************************/
        public IEnumerator ClickedMainButonPayHint()
        {
            yield return null;
            Dictionary<Card, int> resultComplete = amaunt.ToDictionary(x => (Card)x.Key.AvatarCard, x => x.Value);
            waitForClicked.SetResult(resultComplete);
        }

        public IEnumerator CancelMainButton()
        {
            yield return null;
            waitForClicked.SetResult(null);
        }

        /*******************************************************************/
        public async Task<Dictionary<Card, int>> SelectWith(GameAction gamAction)
        {
            if (gamAction is not PayHintsToGoalGameAction payHintsToGoalGameAction) return default;
            _payHintsToGoalGameAction = payHintsToGoalGameAction;
            amaunt = new();
            payHintsToGoalGameAction.InvestigatorsToPay.ForEach(investigator => amaunt.Add(investigator, 0));

            await Task.WhenAny(waitForClicked.Task, Task.Delay(3000));

            if (!waitForClicked.Task.IsCompleted)
                throw new TimeoutException("The operation has exceeded. Timeout.");

            Dictionary<Card, int> resultToSend = waitForClicked.Task.Result;
            waitForClicked = new TaskCompletionSource<Dictionary<Card, int>>();
            return resultToSend;
        }
    }
}