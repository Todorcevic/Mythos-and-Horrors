using DG.Tweening;
using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MythosAndHorrors.GameView
{
    public class CommitCardsController : MonoBehaviour
    {
        [SerializeField, Required, AssetsOnly] private ChallengeTokensManager _challengeTokensManager;
        [SerializeField, Required, ChildGameObjectsOnly] private HorizontalLayoutGroup _layoutGroup;
        [SerializeField, Required, ChildGameObjectsOnly] private RectTransform _rectTransform;
        [SerializeField, Required, ChildGameObjectsOnly] private List<CommitChallengeView> _commitChallenges;

        /*******************************************************************/
        public void ClearAll() => _commitChallenges.ForEach(cardChallenge => cardChallenge.Disable());

        public void ShowAll(IEnumerable<CommitableCard> commitCards, ChallengeType challengeType)
        {
            _layoutGroup.enabled = true;
            _commitChallenges.FindAll(cardChallengeView => !commitCards.Contains(cardChallengeView.CommitableCard)).ForEach(cardChallenge => cardChallenge.Disable());
            Sequence spawnCommitsCardAnimation = DOTween.Sequence();
            foreach (CommitableCard commitCard in commitCards.Where(card => !_commitChallenges.Select(cardChallenge => cardChallenge.CommitableCard).Contains(card)))
            {
                spawnCommitsCardAnimation.Join(GetFreeCardChallenge().SetCard(commitCard, challengeType, commitCard.GetChallengeValue(challengeType)));
            }
            spawnCommitsCardAnimation.OnComplete(() => _layoutGroup.enabled = false);
        }

        private CommitChallengeView GetFreeCardChallenge() => _commitChallenges.FirstOrDefault(cardChallenge => cardChallenge.CommitableCard == null) ?? CreateCardChallenge();

        private CommitChallengeView CreateCardChallenge()
        {
            CommitChallengeView cardChallenge = Instantiate(_commitChallenges.First(), transform);
            cardChallenge.Disable();
            _commitChallenges.Add(cardChallenge);
            return cardChallenge;
        }
    }
}

