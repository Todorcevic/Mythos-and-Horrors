using System;

namespace MythosAndHorrors.GameRules
{
    public static class StringExtension
    {
        public static string ParseViewWith(this string text, params string[] args)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            for (int i = 0; i < args.Length; i++)
            {
                text = text.Replace($"{{arg{i}}}", args[i]);
            }
            return text;
        }
    }
}
