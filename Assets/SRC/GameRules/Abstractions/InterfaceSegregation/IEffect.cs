namespace MythsAndHorrors.GameRules
{
    public interface IEffect
    {
        public string CardCode { get; }
        public string CardCodeAffected { get; }
        public string Description { get; }
    }
}
