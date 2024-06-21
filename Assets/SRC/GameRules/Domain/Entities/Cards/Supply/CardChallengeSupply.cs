using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CardChallengeSupply : CardSupply
    {
        public List<ChallengeActivation> AllCommitsActivations { get; private set; } = new();

        /*******************************************************************/
        protected ChallengeActivation CreateCommitActivation(Func<ChallengePhaseGameAction, Task> logic, Func<ChallengePhaseGameAction, bool> condition, PlayActionType playActionType)
        {
            ChallengeActivation newActivation = new(this, new Stat(0, false), new GameCommand<ChallengePhaseGameAction>(logic), new GameConditionWith<ChallengePhaseGameAction>(condition), playActionType);
            AllCommitsActivations.Add(newActivation);
            _specificAbilities.Add(newActivation);
            return newActivation;
        }

    }
}
