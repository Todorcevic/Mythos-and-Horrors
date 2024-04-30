using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01603 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public bool IsOnlyOneTarget => true;
        public Investigator TargetInvestigator => Owner;

        /*******************************************************************/
    }
}
