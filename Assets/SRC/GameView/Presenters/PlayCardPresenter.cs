using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PlayCardPresenter : IPresenter<PlayEffectGameAction>
    {
        [Inject] private readonly AudioComponent _audioComponent;

        public async Task PlayAnimationWith(PlayEffectGameAction playEffectGameAction)
        {
            if (playEffectGameAction.Effect is CardEffect cardEffec)
            {
                _audioComponent.PlayAudio(cardEffec.LocalizableCode);
            }
            await Task.CompletedTask;
        }
    }
}
