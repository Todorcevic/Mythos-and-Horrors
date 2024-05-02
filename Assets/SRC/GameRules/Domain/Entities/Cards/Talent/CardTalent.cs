using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class CardTalent : Card
    {
        public int TotalChallengePoints => (Info.Strength ?? 0) + (Info.Agility ?? 0) + (Info.Intelligence ?? 0) + (Info.Power ?? 0) + (Info.Wild ?? 0);

    }
}
