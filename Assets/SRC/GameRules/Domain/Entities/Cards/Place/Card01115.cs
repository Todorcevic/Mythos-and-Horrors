namespace MythsAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        public override bool CanMoveWithThis(Investigator investigator)
        {
            if (!Revealed.IsActive) return false;
            return base.CanMoveWithThis(investigator);
        }

        //async Task IStartReactionable.WhenBegin(GameAction gameAction)
        //{
        //    if (gameAction is InteractableGameAction interactableGA)
        //    {
        //        if (interactableGA.Parent is OneInvestigatorTurnGameAction oneInvestigatorGA)
        //        {
        //            Effect effectToModify = oneInvestigatorGA.MoveToPlaceEffects.Find(e => e.Card == this);
        //            effectToModify.ConcatCondition(() => Revealed.IsActive);
        //        }
        //    }

        //    await Task.CompletedTask;
        //}
    }
}
