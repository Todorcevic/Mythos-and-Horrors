using DG.Tweening;

namespace MythsAndHorrors.GameView
{
    public class NullZoneView : ZoneView
    {
        public override Tween EnterCard(CardView cardView) => DOTween.Sequence();

        public override Tween ExitCard(CardView cardView) => DOTween.Sequence();

        public override Tween MouseEnter(CardView cardView) => DOTween.Sequence();

        public override Tween MouseExit(CardView cardView) => DOTween.Sequence();

        public override Tween MouseDrag(CardView cardView) => DOTween.Sequence();
    }
}
