namespace MythosAndHorrors.GameRules
{
    public class Card01603 : CardCreature, IStalker, ITarget
    {
        public Investigator TargetInvestigator => Owner;
    }
}
