using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class CardMovePresenter 
    {
        [Inject] private readonly ZonesManager _zonesManager;
        [Inject] private readonly CardsManager _cardsManager;

        /*******************************************************************/
        public void MoveCardToZone(string cardId, string zoneId)
        {

        }
    }
}
