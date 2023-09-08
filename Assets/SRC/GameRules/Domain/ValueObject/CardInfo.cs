namespace GameRules
{
    public record CardInfo
    {
        public string Code { get; init; }
        public string Name { get; init; }
        public string CardType { get; init; }
        public int? Health { get; init; }
        public int? Damage { get; init; }
        public int? FreeDraw { get; init; }
        public int? CostDiscard { get; init; }
    }
}
