using DG.Tweening;

namespace MythsAndHorrors.GameView
{
    public interface IZoneBehaviour
    {
        public Tween MouseEnter(CardView cardView);
        public Tween MouseExit(CardView cardView);
        public Tween MouseDrag(CardView cardView);
    }
}
