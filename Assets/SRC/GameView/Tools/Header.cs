#pragma warning disable IDE0051, IDE0052// Remove unused private members
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace Tools
{
    [Serializable]
    public class Header
    {
        private readonly DataCreatorBase cardInfo;
        private readonly Action<DataCreatorBase> selection;

        public Header(DataCreatorBase cardInfo, Action<DataCreatorBase> selection)
        {
            this.cardInfo = cardInfo;
            this.selection = selection;
            Code = cardInfo.Code;
            Name = cardInfo.Name;
        }

        [ReadOnly]
        [GUIColor("GetColor")]
        [SerializeField] private string Code;

        [ReadOnly]
        [GUIColor("GetColor")]
        [SerializeField] private string Name;

        [Button]
        [GUIColor("GetColor")]
        [TableColumnWidth(100, false)]
        private void Show() => selection.Invoke(cardInfo);

        private Color GetColor() => cardInfo.IsComplete ? Color.green : Color.yellow;
    }
}
#pragma warning restore IDE0051, IDE0052// Remove unused private members
