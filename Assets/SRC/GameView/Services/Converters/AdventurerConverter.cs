using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json.Linq;
using Unity.Plastic.Newtonsoft.Json;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AdventurerConverter : JsonConverter
    {
        [Inject] private readonly CardFactory _cardFactory;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Adventurer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            return new Adventurer()
            {
                AdventurerCardCode = _cardFactory.CreateCard(jo["AdventurerCardCode"].Value<string>()),
                CardsCode = jo["CardsCode"].Select(card => _cardFactory.CreateCard(card.Value<string>())).ToList(),
                CardsRequerimentCode = jo["RequerimentCardCodes"].Select(card => _cardFactory.CreateCard(card.Value<string>())).ToList(),
                DeckSize = jo["DeckSize"].Value<int>(),
                Xp = jo["Xp"].Value<int>(),
                Injury = jo["Injury"].Value<int>(),
                Shock = jo["Shock"].Value<int>(),
                DeckBuildingConditions = jo["DeckBuildingConditions"].ToObject<Dictionary<Faction, int>>()
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is not Adventurer adventurer) throw new ArgumentException("Invalid type");

            JObject jo = new()
            {
                {"AdventurerCardCode", adventurer.AdventurerCardCode.Info.Code },
                {"CardsCode", JToken.FromObject(adventurer.CardsCode.Select(card => card.Info.Code)) },
                {"CardsRequerimentCode", JToken.FromObject(adventurer.CardsRequerimentCode.Select(card => card.Info.Code)) },
                { "DeckSize", adventurer.DeckSize },
                { "Xp", adventurer.Xp },
                { "Injury", adventurer.Injury },
                { "Shock", adventurer.Shock },
                { "DeckBuildingConditions", JToken.FromObject(adventurer.DeckBuildingConditions) }
            };

            jo.WriteTo(writer);
        }
    }
}
