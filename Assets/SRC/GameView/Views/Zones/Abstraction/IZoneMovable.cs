using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameView
{
    public interface IZoneMovable
    {
        Tween MoveCard(CardView card);
    }
}
