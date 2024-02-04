using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class InvestigatorLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public void Execute(string investigatorFilePath)
        {
            Investigator newInvestigator = _jsonService.CreateDataFromFile<Investigator>(investigatorFilePath);
            _investigatorsProvider.AddInvestigator(newInvestigator);
            _avatarViewsManager.GetVoid().Init(newInvestigator);
        }
    }
}
