using DG.Tweening;

namespace MythosAndHorrors.GameView
{
    public class ShowSelectorZoneView : ZoneRowView
    {
        public override Tween EnterZone(CardView cardView)
        {
            cardView.ShowBuffsAndEffects();
            cardView.DisableToCenterShow();
            return base.EnterZone(cardView);
        }

        public override Tween ExitZone(CardView cardView)
        {
            cardView.HideBuffsAndEffects();
            cardView.EnableFromCenterShow();
            return base.ExitZone(cardView);
        }
    }
}
