using Unity.Plastic.Newtonsoft.Json;

namespace MythsAndHorrors.GameRules
{
    public record CardInfo
    {
        [JsonProperty("Code")] public string Code { get; init; }
        [JsonProperty("Name")] public string Name { get; init; }
        [JsonProperty("Name2")] public string Name2 { get; init; }
        [JsonProperty("Description")] public string Description { get; init; }
        [JsonProperty("Description2")] public string Description2 { get; init; }
        [JsonProperty("Flavor")] public string Flavor { get; init; }
        [JsonProperty("Flavor2")] public string Flavor2 { get; init; }
        [JsonProperty("CardType")] public CardType CardType { get; init; }
        [JsonProperty("Faction")] public Faction Faction { get; init; }
        [JsonProperty("Slots")] public SlotType[] Slots { get; init; }
        [JsonProperty("Tags")] public string[] Tags { get; init; }
        [JsonProperty("Health")] public int? Health { get; init; }
        [JsonProperty("Sanity")] public int? Sanity { get; init; }
        [JsonProperty("Cost")] public int? Cost { get; init; }
        [JsonProperty("Quantity")] public int? Quantity { get; init; }
        [JsonProperty("PackCode")] public string PackCode { get; init; }
        [JsonProperty("SceneCode")] public string SceneCode { get; init; }
        [JsonProperty("Strength")] public int? Strength { get; init; }
        [JsonProperty("Agility")] public int? Agility { get; init; }
        [JsonProperty("Intelligence")] public int? Intelligence { get; init; }
        [JsonProperty("Power")] public int? Power { get; init; }
        [JsonProperty("Wild")] public int? Wild { get; init; }
        [JsonProperty("EnemyDamage")] public int? EnemyDamage { get; init; }
        [JsonProperty("EnemyFear")] public int? EnemyFear { get; init; }
        [JsonProperty("Xp")] public int? Xp { get; init; }
        [JsonProperty("Victory")] public int? Victory { get; init; }
        [JsonProperty("Enigma")] public int? Enigma { get; init; }
        [JsonProperty("Hints")] public int? Hints { get; init; }
        [JsonProperty("Eldritch")] public int? Eldritch { get; init; }
        [JsonProperty("HealthPerInvestigator")] public bool? HealthPerInvestigator { get; init; }
    }
}
