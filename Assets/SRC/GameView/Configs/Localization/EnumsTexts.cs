using MythosAndHorrors.GameRules;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MythosAndHorrors.GameView
{
    public class EnumsTexts
    {
        [JsonProperty("CardType")] private List<string> _cardTypesTexts;
        [JsonProperty("Tag")] private List<string> _tagsTexts;
        private Dictionary<CardType, string> _dictionaryCardTypes;
        private Dictionary<Tag, string> _dictionaryTags;

        public Dictionary<CardType, string> DictionaryCardTypes => _dictionaryCardTypes ??= ConvertEnumToDictionary<CardType>(_cardTypesTexts);
        public Dictionary<Tag, string> DictionaryTags => _dictionaryTags ??= ConvertEnumToDictionary<Tag>(_tagsTexts);

        /*******************************************************************/
        private Dictionary<T, string> ConvertEnumToDictionary<T>(List<string> texts) where T : Enum
        {
            bool enumHasFlag = typeof(T).GetCustomAttribute<FlagsAttribute>() != null;
            Dictionary<T, string> _dictionaryCardTypes = new();
            for (int i = 0; i < texts.Count; i++)
            {
                _dictionaryCardTypes.Add((T)Enum.ToObject(typeof(T), enumHasFlag ? 1 << i : i), texts[i]);
            }
            return _dictionaryCardTypes;
        }
    }
}