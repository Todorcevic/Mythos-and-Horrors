using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace MythosAndHorrors.Tools
{
    [Serializable]
    public class Header
    {
        private readonly DataCreatorBase cardInfo;
        private readonly Action<DataCreatorBase> edit;
        private readonly Action<DataCreatorBase> delete;

        public Header(DataCreatorBase cardInfo, Action<DataCreatorBase> edit, Action<DataCreatorBase> delete)
        {
            this.cardInfo = cardInfo;
            this.edit = edit;
            this.delete = delete;
            Code = cardInfo.Code;
            Name = cardInfo.Name;
            Type = cardInfo.CardType.ToString();
        }

        [ReadOnly]
        [GUIColor("GetColor")]
        [SerializeField] private string Code;

        [ReadOnly]
        [GUIColor("GetColor")]
        [SerializeField] private string Name;

        [ReadOnly]
        [GUIColor("GetColor")]
        [SerializeField] private string Type;

        [Button(SdfIconType.Pencil, Name = "")]
        [GUIColor("@Color.yellow")]
        [TableColumnWidth(50, false)]
        private void Edit() => edit.Invoke(cardInfo);

        [Button(SdfIconType.Dash, Name = "")]
        [GUIColor("@Color.red")]
        [TableColumnWidth(50, false)]
        private void Delete() => delete.Invoke(cardInfo);

        private Color GetColor() => cardInfo.IsIncomplete ? Color.red : Color.white;
    }
}