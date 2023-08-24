namespace GameRules
{
    public record MoveCardDTO(Card Card, Zone Zone, CardMovementType MovementType = CardMovementType.Basic);
}
