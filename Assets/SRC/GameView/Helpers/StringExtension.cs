using System;

namespace MythosAndHorrors.GameView
{
    public static class StringExtension
    {
        public static string ParseViewWith(this string str, params string[] args)
        {
            if (str == null) throw new ArgumentNullException(nameof(str));

            for (int i = 0; i < args.Length; i++)
            {
                str = str.Replace($"{{arg{i}}}", args[i]);
            }
            return str;
        }
    }
}
