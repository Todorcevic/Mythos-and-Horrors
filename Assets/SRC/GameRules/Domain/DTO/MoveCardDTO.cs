namespace GameRules
{
    public record MoveCardDTO(Card Card, Zone Zone, CardMovementAnimation MovementType = CardMovementAnimation.Basic);
}
