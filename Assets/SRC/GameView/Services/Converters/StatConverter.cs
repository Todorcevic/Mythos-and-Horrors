using System;
using Newtonsoft.Json;
using MythosAndHorrors.GameRules;

namespace MythosAndHorrors.GameView
{
    public class StatConverter : JsonConverter<Stat>
    {
        public override Stat ReadJson(JsonReader reader, Type objectType, Stat existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) throw new JsonSerializationException("Expected an integer value, but got null.");
            if (reader.Value is int intValue) return new Stat(intValue, false);
            if (reader.Value is long longValue) return new Stat((int)longValue, false);

            throw new JsonSerializationException($"Expected integer or long, but got {reader.Value.GetType()}.");
        }

        public override void WriteJson(JsonWriter writer, Stat value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Value);
        }
    }
}
