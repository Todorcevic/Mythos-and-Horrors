namespace MythsAndHorrors.GameView
{
    public interface IZoneBahaviour
    {
        public void OnClicked(CardView cardView);
        public void OnMouseEnter(CardView cardView);
        public void OnMouseExit(CardView cardView);
        public void OnMouseDrag(CardView cardView);
    }
}
