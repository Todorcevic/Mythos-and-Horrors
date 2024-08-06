namespace MythosAndHorrors.GameRules
{
    public interface IViewEffect : IViewEffectDescription
    {
        public string CardCode { get; }
        public string CardCodeSecundary { get; }
    }
}
