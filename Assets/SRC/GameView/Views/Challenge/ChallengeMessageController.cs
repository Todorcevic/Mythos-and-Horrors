using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ChallengeMessageController : MonoBehaviour
    {
        private readonly List<ChallengeMessageView> _allMessage = new();
        private List<(string value, string description, Sprite image)> _lastDropTokensInfo;
        [SerializeField, Required, ChildGameObjectsOnly] ChallengeMessageView _messageViewPrefab;

        /*******************************************************************/
        public void ShowThisToken(string value, string description, Sprite sprite)
        {
            ClearAllMessage();
            CreateMessage(value, description, sprite);
        }

        public void ShowDropTokens(List<(string value, string description, Sprite sprite)> dropInfo)
        {
            ClearAllMessage();
            if (dropInfo == null) return;
            _lastDropTokensInfo = dropInfo;
            dropInfo.ForEach(info => CreateMessage(info.value, info.description, info.sprite));
        }

        public void ShowLastDropTokens() => ShowDropTokens(_lastDropTokensInfo);

        public void ResetAll()
        {
            ClearAllMessage();
            _lastDropTokensInfo = null;
        }

        private void CreateMessage(string value, string description, Sprite sprite)
        {
            ChallengeMessageView newMessage = ZenjectHelper.Instantiate(_messageViewPrefab, transform);
            newMessage.SetMessage(value, description, sprite);
            _allMessage.Add(newMessage);
        }

        private void ClearAllMessage()
        {
            _allMessage.ForEach(message => Destroy(message.gameObject));
            _allMessage.Clear();
        }
    }
}

