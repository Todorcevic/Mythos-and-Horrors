﻿using System;
using Newtonsoft.Json;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class CardConverter : JsonConverter<Card>
    {
        [Inject] private readonly CardLoaderUseCase _cardLoaderUseCase;

        /*******************************************************************/
        public override Card ReadJson(JsonReader reader, Type objectType, Card existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            _cardLoaderUseCase.Execute(reader.Value as string);

        public override void WriteJson(JsonWriter writer, Card value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Info.Code);
        }
    }
}
