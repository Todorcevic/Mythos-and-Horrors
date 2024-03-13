using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ReturnCardPresenter : IPresenter<ChooseInvestigatorGameAction>, IPresenter<InvestigateGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<ChooseInvestigatorGameAction>.PlayAnimationWith(ChooseInvestigatorGameAction playEffectGA)
        {
            await _moveCardHandler.ReturnCard(playEffectGA.InvestigatorSelected.AvatarCard);
        }

        async Task IPresenter<InvestigateGameAction>.PlayAnimationWith(InvestigateGameAction playEffectGA)
        {
            await _moveCardHandler.ReturnCard(playEffectGA.CardPlace);
        }
    }
}
