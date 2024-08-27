using System;
using Newtonsoft.Json;
using MythosAndHorrors.GameRules;

namespace MythosAndHorrors.GameView
{
    public class StateConverter : JsonConverter<State>
    {
        public override State ReadJson(JsonReader reader, Type objectType, State existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            new((bool)reader.Value);

        public override void WriteJson(JsonWriter writer, State value, JsonSerializer serializer)
        {
            writer.WriteValue(value.IsActive);
        }
    }
}
