using System;

namespace MythosAndHorrors.GameRules
{
    public class EffectLocalization
    {
        public string Code { get; }
        public string Description { get; }
    }


    public class Localization
    {
        private readonly string _code;
        private readonly Func<string> _funcCode;

        public string Code => _funcCode != null ? _funcCode() : _code;
        public string[] Args { get; }

        /*******************************************************************/
        public Localization(string code, params string[] args)
        {
            _code = code;
            Args = args;
        }

        public Localization(Func<string> code, params string[] args)
        {
            _funcCode = code;
            Args = args;
        }
    }
}
