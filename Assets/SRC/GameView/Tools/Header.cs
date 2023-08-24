#pragma warning disable IDE0051, IDE0052 // Remove unread private members
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using GameRules;

namespace Tools
{
    [Serializable]
    public class Header
    {
        private readonly CardDataModel cardInfo;
        private readonly Action<CardDataModel> selection;

        public Header(CardDataModel cardInfo, Action<CardDataModel> selection)
        {
            this.cardInfo = cardInfo;
            this.selection = selection;
            Id = cardInfo.Id;
            Name = cardInfo.Name;
            CardType = cardInfo.Type.ToString();
        }

        [ReadOnly]
        [TableColumnWidth(15)]
        [GUIColor("GetColor")]
        [SerializeField] private string Id;

        [ReadOnly]
        [TableColumnWidth(100)]
        [GUIColor("GetColor")]
        [SerializeField] private string Name;

        [ReadOnly]
        [TableColumnWidth(40)]
        [GUIColor("GetColor")]
        [SerializeField] public string CardType;

        [Button]
        [GUIColor("GetColor")]
        [TableColumnWidth(100)]
        private void Show() => selection.Invoke(cardInfo);

        private Color GetColor() => cardInfo.IsComplete ? Color.green : Color.yellow;
    }
}
#pragma warning restore IDE0051, IDE0052 // Remove unused private members