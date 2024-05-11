using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class AvatarViewLoaderUseCase
    {
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        /*******************************************************************/
        public void Execute()
        {
            _investigatorsProvider.AllInvestigators.ForEach(investigator => _avatarViewsManager.GetVoid().Init(investigator));
        }
    }
}
