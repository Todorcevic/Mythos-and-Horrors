using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tuesday
{
    public class CreateRow : MonoBehaviour
    {
        [OnValueChanged("PosicionateCards")]
        [SerializeField] float cardSeparator = 0.002f;

        List<CardView> allCards;

        /*******************************************************************/
        private void PosicionateCards()
        {
            allCards = GetComponentsInChildren<CardView>().ToList();
            ResetPosition();
            float xOffset = 0f;
            float yOffset = 0f;
            foreach (CardView card in allCards)
            {    
                card.transform.position += new Vector3(xOffset, yOffset, 0);
                xOffset += cardSeparator;
                yOffset += ViewValues.CARD_THICKNESS;
            }
        }

        private void ResetPosition()
        {
            allCards.ForEach(card => card.transform.position = transform.position);
        }
    }
}
