using MythsAndHorrors.GameRules;
using System.Collections.Generic;

namespace MythsAndHorrors.GameView
{
    public class SaveData
    {
        public List<string> InvestigatorsSelected { get; set; }
        public string SceneSelected { get; set; }
        public Dificulty DificultySelected { get; set; }
        public Laguaje LaguajeSelected { get; set; }
    }
}
