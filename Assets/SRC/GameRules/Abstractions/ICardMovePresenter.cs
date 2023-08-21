namespace GameRules
{
    public interface ICardMovePresenter
    {
        void MoveCardToZone(string cardId, ZoneType gameZone);
        void MoveCardToZoneWithPreview(string cardId, ZoneType gameZone);
    }
}
