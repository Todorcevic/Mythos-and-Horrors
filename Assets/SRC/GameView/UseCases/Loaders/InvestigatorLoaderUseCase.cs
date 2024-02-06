using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InvestigatorLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;
        [Inject] private readonly CardsProvider _cardProvider;

        /*******************************************************************/
        public void Execute(string investigatorFilePath)
        {
            Investigator newInvestigator = _jsonService.CreateDataFromFile<Investigator>(investigatorFilePath);
            _investigatorsProvider.AddInvestigator(newInvestigator);
            _avatarViewsManager.GetVoid().Init(newInvestigator);
            CreateCardAvatar(newInvestigator);
        }

        private void CreateCardAvatar(Investigator newInvestigator)
        {
            CardAvatar newCardAvatar = _reactionablesProvider.Create(typeof(CardAvatar), new object[] { newInvestigator.InvestigatorCard.Info }) as CardAvatar;
            newInvestigator.Init(newCardAvatar);
            _cardProvider.AddCard(newCardAvatar);
        }
    }
}
