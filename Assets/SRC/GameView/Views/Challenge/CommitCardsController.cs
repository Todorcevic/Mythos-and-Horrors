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
        [SerializeField, Required, ChildGameObjectsOnly] private List<CardChallengeView> _cardChallenges;

        /*******************************************************************/
        public void ClearAll() => _cardChallenges.ForEach(cardChallenge => cardChallenge.Disable());

        public void ShowAll(IEnumerable<CommitableCard> commitCards, ChallengeType challengeType)
        {
            _layoutGroup.enabled = true;
            _cardChallenges.FindAll(cardChallengeView => !commitCards.Contains(cardChallengeView.Card)).ForEach(cardChallenge => cardChallenge.Disable());
            Sequence spawnCommitsCardAnimation = DOTween.Sequence();
            foreach (CommitableCard commitCard in commitCards.Where(card => !_cardChallenges.Select(cardChallenge => cardChallenge.Card).Contains(card)))
            {
                spawnCommitsCardAnimation.Join(GetFreeCardChallenge().SetCard(commitCard, challengeType, commitCard.GetChallengeValue(challengeType)));
            }
            spawnCommitsCardAnimation.OnComplete(() => _layoutGroup.enabled = false);
        }

        private CardChallengeView CreateCardChallenge()
        {
            CardChallengeView cardChallenge = Instantiate(_cardChallenges.First(), transform);
            cardChallenge.Disable();
            _cardChallenges.Add(cardChallenge);
            return cardChallenge;
        }

        private CardChallengeView GetFreeCardChallenge() => _cardChallenges.FirstOrDefault(cardChallenge => cardChallenge.Card == null) ?? CreateCardChallenge();
    }
}

