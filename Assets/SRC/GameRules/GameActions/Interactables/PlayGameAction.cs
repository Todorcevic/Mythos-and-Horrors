using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class PlayGameAction : InteractableGameAction
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        public override List<Card> ActivableCards => _cardsProvider.GetPlayableCards();

        /*******************************************************************/
    }
}

