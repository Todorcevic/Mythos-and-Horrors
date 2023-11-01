namespace MythsAndHorrors.GameView
{
    public interface IZoneBehaviour
    {
        public void OnMouseEnter(CardView cardView);
        public void OnMouseExit(CardView cardView);
        public void OnMouseDrag(CardView cardView);
    }
}
