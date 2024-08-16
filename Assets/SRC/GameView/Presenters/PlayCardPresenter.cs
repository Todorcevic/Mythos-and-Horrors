using MythosAndHorrors.GameRules;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PlayCardPresenter : IPresenter<PlayEffectGameAction>
    {
        [Inject] private readonly AudioComponent _audioComponent;
        [Inject] private readonly AnimationsManager _animationsManager;

        public async Task PlayAnimationWith(PlayEffectGameAction playEffectGameAction)
        {
            if (playEffectGameAction.Effect is CardEffect cardEffec)
            {
                PlayAnimationSO animation = _animationsManager.GetAnimation(cardEffec.LocalizableCode);
                _audioComponent.PlayAudio(animation.Audio);
            }
            await Task.CompletedTask;
        }
    }
}
