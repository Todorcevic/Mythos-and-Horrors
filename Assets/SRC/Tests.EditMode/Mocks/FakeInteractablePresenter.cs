using MythosAndHorrors.GameRules;
using System.Threading.Tasks;

namespace MythosAndHorrors.EditMode.Tests
{
    public class FakeInteractablePresenter : IInteractablePresenter
    {
        public Effect EffectSelected { get; set; }

        public Task<Effect> SelectWith(GameAction gamAction)
        {
            return Task.FromResult(EffectSelected);
        }
    }
}