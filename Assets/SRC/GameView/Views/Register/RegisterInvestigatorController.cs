using MythosAndHorrors.GameRules;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class RegisterInvestigatorController : MonoBehaviour
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [SerializeField, Required, ChildGameObjectsOnly] private TextMeshProUGUI _investigatorName;
        [SerializeField, Required, ChildGameObjectsOnly] private Image _investigatorImage;
        [SerializeField, Required, ChildGameObjectsOnly] private RegisterStatController _xp;
        [SerializeField, Required, ChildGameObjectsOnly] private RegisterStatController _injuries;
        [SerializeField, Required, ChildGameObjectsOnly] private RegisterStatController _shocks;
        [SerializeField, Required, ChildGameObjectsOnly] private RegisterCardView _registerCardPrefab;
        [SerializeField, Required, ChildGameObjectsOnly] private Transform _registerCardContainer;

        /*******************************************************************/
        public async void SetInvestigator(Investigator investigator)
        {
            await _investigatorImage.LoadCardSprite(investigator.Code);
            _investigatorName.text = investigator.InvestigatorCard.Info.Name;
            _xp.ShowXP(investigator.Xp.Value);
            _injuries.ShowXP(investigator.Injury.Value);
            _shocks.ShowXP(investigator.Shock.Value);
            ShowVictoriesCard(investigator);
            gameObject.SetActive(true);
        }

        private void ShowVictoriesCard(Investigator investigator)
        {
            IEnumerable<Card> victoryCards = _chaptersProvider.CurrentScene.VictoryZone.Cards.Where(card => card.Info.Victory > 0);
            IEnumerable<IVictoriable> investigatorVictoryCards = _cardsProvider.AllCards.OfType<IVictoriable>()
                .Where(victoriableCard => victoriableCard.IsVictoryComplete && victoriableCard.InvestigatorsVictoryAffected.Contains(investigator));

            IEnumerable<Card> all = victoryCards.Union(investigatorVictoryCards.Cast<Card>());
            foreach (Card victoriable in all)
            {
                RegisterCardView newRegisterCard = ZenjectHelper.Instantiate(_registerCardPrefab, _registerCardContainer);
                newRegisterCard.Set((victoriable).Info.Code, (victoriable.Info.Victory ?? 0) + (victoriable as IVictoriable)?.Victory ?? 0);
                newRegisterCard.gameObject.SetActive(true);
            }

        }
    }
}
