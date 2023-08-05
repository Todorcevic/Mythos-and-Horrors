using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

namespace GameView
{
    public class CreateDeck : MonoBehaviour
    {
        [OnValueChanged("PosicionateCards")]
        [SerializeField] float cardThickness = 0.002f;


        List<CardView> allCards;

        private void PosicionateCards()
        {
            allCards = GetComponentsInChildren<CardView>().ToList();
            ResetPosition();
            float yOffset = 0f;
            foreach (CardView card in allCards)
            {
                yOffset += cardThickness;
                card.transform.position += new Vector3(0, yOffset, 0);
            }
        }

        private void ResetPosition()
        {
            allCards.ForEach(card => card.transform.position = transform.position);
        }
    }
}
