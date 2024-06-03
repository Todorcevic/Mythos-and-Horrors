namespace MythosAndHorrors.GameRules
{
    public interface IViewEffectDescription
    {
        public string Description { get; }
    }

    public interface IViewEffect : IViewEffectDescription
    {
        public string CardCode { get; }
        public string CardCodeSecundary { get; }
    }
}
