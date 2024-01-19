using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class BasicPlayGameAction : InteractableGameAction
    {
        public async Task Run()
        {
            await Start();
        }
    }
}
