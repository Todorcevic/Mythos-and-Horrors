using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythosAndHorrors.GameView.NEWS
{
    public class SupporterCardView : CardView
    {
        //[SerializeField, Required, ChildGameObjectsOnly] private InvestigatorHealthController _healthController;
        //[SerializeField, Required, ChildGameObjectsOnly] private InvestigatorSanityController _sanityController;

        /*******************************************************************/
        protected override void SetSpecific()
        {
            //CardSupply cardSupply = Card as CardSupply;
            //_healthController.Init(Card.Owner);
            //_sanityController.Init(Card.Owner);
        }
    }
}
