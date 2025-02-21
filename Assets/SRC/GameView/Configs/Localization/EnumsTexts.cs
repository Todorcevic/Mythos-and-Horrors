using MythosAndHorrors.GameRules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace MythosAndHorrors.GameView
{
    public class EnumsTexts
    {
        [JsonProperty("CardType")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Deserialized")]
        private readonly List<string> _cardTypesTexts;
        [JsonProperty("Tag")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Deserialized")]
        private readonly List<string> _tagsTexts;
        [JsonProperty("SlotType")]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Deserialized")]
        private readonly List<string> _slotsTexts;

        private Dictionary<Enum, string> _enumsTexts;

        /*******************************************************************/
        public string GetEnumToText<T>(T position) where T : Enum
        {
            if (!_enumsTexts.TryGetValue(position, out string text)) throw new ArgumentException("Enum text not found for code: " + position);
            return text;
        }

        public void ConvertAllListInDictionary()
        {
            _enumsTexts = new();
            IEnumerable<FieldInfo> fields = GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(fieldInfo => fieldInfo.FieldType.IsGenericType && fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>));

            foreach (FieldInfo field in fields)
            {
                JsonPropertyAttribute jsonPropertyAttribute = field.GetCustomAttribute<JsonPropertyAttribute>();
                if (jsonPropertyAttribute == null) continue;
                Type enumType = typeof(CardType).Assembly.GetType($"{typeof(CardType).Namespace}.{jsonPropertyAttribute.PropertyName}");
                if (enumType == null) continue;
                bool enumHasFlag = enumType.GetCustomAttribute<FlagsAttribute>() != null;
                List<string> texts = (List<string>)field.GetValue(this);
                for (int i = 0; i < texts.Count; i++)
                {
                    _enumsTexts.Add((Enum)Enum.ToObject(enumType, enumHasFlag ? 1 << i : i), texts[i]);
                }
            }
        }
    }
}