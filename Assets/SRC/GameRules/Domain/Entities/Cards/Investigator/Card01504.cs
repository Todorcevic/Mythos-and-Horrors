using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class Card01504 : CardInvestigator
    {
        public override async Task StarEffect() => await Task.CompletedTask;

        public override int StarValue() => Owner.FearRecived;

    }
}
