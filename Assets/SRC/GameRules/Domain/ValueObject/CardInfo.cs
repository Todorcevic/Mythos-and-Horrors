namespace MythsAndHorrors.GameRules
{
    public record CardInfo
    {
        public string Code { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public CardType CardType { get; init; }
        public string[] Tags { get; init; } //Subtypes
        //public IReadOnlyList<string> Tags { get; init; } //Subtypes
        public int? Health { get; init; }
        public int? Sanity { get; init; }
        public int? Cost { get; init; }
        public int? Quantity { get; init; }

    }
}
