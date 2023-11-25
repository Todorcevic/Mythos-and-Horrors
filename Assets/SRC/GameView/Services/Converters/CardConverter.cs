using System;
using Unity.Plastic.Newtonsoft.Json;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class CardConverter : JsonConverter<Card>
    {
        [Inject] private readonly CardsProvider _cardProvider;
        [Inject] private readonly CardFactory _cardFactory;

        /*******************************************************************/
        public override Card ReadJson(JsonReader reader, Type objectType, Card existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Card newCard = _cardFactory.CreateCard(reader.Value as string);
            _cardProvider.AddCard(newCard);
            return newCard;
        }

        public override void WriteJson(JsonWriter writer, Card value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Info.Code);
        }
    }
}
