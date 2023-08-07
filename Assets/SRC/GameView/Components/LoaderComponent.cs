using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace GameView
{
    public class LoaderComponent : MonoBehaviour
    {
        [Inject] private readonly CardFactory _cardFactory;

        private void Start()
        {
            _cardFactory.CreateCard();
        }
    }
}
