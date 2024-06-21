using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CardChallengeSupply : CardSupply
    {
        public List<Activation<ChallengePhaseGameAction>> AllCommitsActivations { get; private set; } = new();

        /*******************************************************************/
        protected Activation<ChallengePhaseGameAction> CreateChallengeActivation(Func<ChallengePhaseGameAction, Task> logic, Func<ChallengePhaseGameAction, bool> condition, PlayActionType playActionType)
        {
            Activation<ChallengePhaseGameAction> newActivation = new(this, new Stat(0, false), new GameCommand<ChallengePhaseGameAction>(logic), new GameConditionWith<ChallengePhaseGameAction>(condition), playActionType);
            AllCommitsActivations.Add(newActivation);
            _specificAbilities.Add(newActivation);
            return newActivation;
        }

    }
}
