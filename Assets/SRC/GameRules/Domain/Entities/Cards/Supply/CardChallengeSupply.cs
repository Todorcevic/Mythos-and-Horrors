using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardChallengeSupply : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public List<Activation<ChallengePhaseGameAction>> AllCommitsActivations { get; private set; } = new();

        /*******************************************************************/
        protected Activation<ChallengePhaseGameAction> CreateChallengeActivation(Func<ChallengePhaseGameAction, Task> logic, Func<ChallengePhaseGameAction, bool> condition, PlayActionType playActionType, Localization localization, Card cardAffected = null)
        {
            Activation<ChallengePhaseGameAction> newActivation = new(this, new Stat(0, false), new GameCommand<ChallengePhaseGameAction>(logic), new GameConditionWith<ChallengePhaseGameAction>(condition), playActionType, cardAffected, localization);
            AllCommitsActivations.Add(newActivation);
            _specificAbilities.Add(newActivation);
            return newActivation;
        }

        protected async Task GainSkillLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(ControlOwner, 1).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(challengePhaseGameAction.StatModifier, 1).Execute();
        }

    }
}
