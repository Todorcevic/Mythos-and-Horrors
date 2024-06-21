using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ActivationsProvider
    {
        private readonly List<Activation<Investigator>> _activations = new();

        /*******************************************************************/
        public Activation<Investigator> CreateActivation(Card card, int turnCost, Func<Investigator, Task> logic, Func<Investigator, bool> condition, PlayActionType playActionType)
        {
            Activation<Investigator> newActivation = new(card, new Stat(turnCost, false), new GameCommand<Investigator>(logic), new GameConditionWith<Investigator>(condition), playActionType);
            _activations.Add(newActivation);
            return newActivation;
        }

        /*******************************************************************/
        public IEnumerable<Activation<Investigator>> GetActivationsFor(Card card) => _activations.FindAll(activation => activation.Card == card);
    }
}
