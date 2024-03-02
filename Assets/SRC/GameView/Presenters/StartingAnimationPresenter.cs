using System.Threading.Tasks;
using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class StartingAnimationPresenter : IPresenter<ChooseInvestigatorGameAction>, IPresenter<InvestigateGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        async Task IPresenter<ChooseInvestigatorGameAction>.PlayAnimationWith(ChooseInvestigatorGameAction playEffectGA)
        {
            await _moveCardHandler.ReturnCard(playEffectGA.ActiveInvestigator.AvatarCard);
        }

        async Task IPresenter<InvestigateGameAction>.PlayAnimationWith(InvestigateGameAction playEffectGA)
        {
            await _moveCardHandler.ReturnCard(playEffectGA.CardPlace);
        }
    }
}
