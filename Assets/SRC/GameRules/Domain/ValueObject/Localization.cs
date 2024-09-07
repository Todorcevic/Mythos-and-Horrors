namespace MythosAndHorrors.GameRules
{
    public class Localization
    {
        public string Code { get; }
        public string[] Args { get; }

        /*******************************************************************/
        public Localization(string code, params string[] args)
        {
            Code = code;
            Args = args;
        }
    }
}
