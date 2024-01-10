using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public class CardCondition : Card
    {
        public int TotalChallengePoints => (Info.Strength ?? 0) + (Info.Agility ?? 0) + (Info.Intelligence ?? 0) + (Info.Power ?? 0) + (Info.Wild ?? 0);

    }
}
