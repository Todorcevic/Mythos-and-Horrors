using System;

namespace MythosAndHorrors.GameRules
{
    public class Localization
    {
        private readonly string _code;
        private readonly string[] _args;
        private readonly Func<string[]> _funcArgs;
        private readonly Func<string> _funcCode;

        public string Code => _funcCode != null ? _funcCode() : _code;
        public string[] Args => _funcArgs != null ? _funcArgs() : _args;

        /*******************************************************************/
        public Localization(string code, params string[] args)
        {
            _code = code;
            _args = args;
        }

        public Localization(Func<string> code, params string[] args)
        {
            _funcCode = code;
            _args = args;
        }

        public Localization(string code, Func<string[]> args)
        {
            _code = code;
            _funcArgs = args;
        }
    }
}
