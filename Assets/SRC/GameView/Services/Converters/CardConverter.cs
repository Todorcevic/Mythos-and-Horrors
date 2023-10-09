using System;
using Unity.Plastic.Newtonsoft.Json;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardConverter : JsonConverter
    {
        [Inject] private readonly CardFactory _cardFactory;

        public override bool CanConvert(Type objectType)
        {
            return typeof(Card).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return _cardFactory.CreateCard(reader.Value as string);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not Card card) throw new ArgumentException("Invalid type");

            writer.WriteValue(card.Info.Code);
        }
    }
}
