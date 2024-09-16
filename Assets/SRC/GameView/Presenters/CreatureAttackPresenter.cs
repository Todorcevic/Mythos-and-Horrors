using Zenject;
using System.Threading.Tasks;
using MythosAndHorrors.GameRules;
using DG.Tweening;
using System.Linq;

namespace MythosAndHorrors.GameView
{
    public class CreatureAttackPresenter : IPresenter<CreatureAttackGameAction>
    {
        [Inject] private readonly MoveCardHandler _moveCardHandler;

        /*******************************************************************/
        public async Task PlayAnimationWith(CreatureAttackGameAction gameAction)
        {
            await DOTween.Sequence()
                .Join(_moveCardHandler.MoveCardWithPreviewToZone(gameAction.Creature, gameAction.Investigator.InvestigatorZone))
                .Append(_moveCardHandler.ReturnCard(gameAction.Creature)).AsyncWaitForCompletion();
        }
    }
}
