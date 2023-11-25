using System;
using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class GameStateService
    {
        public List<string> AdventurersSelected { get; set; } = new List<string>() { "01501", "01502", "01503", "01504" };

        //public List<Adventurer> Adventurers { get; set; } = new List<Adventurer>()
        //{
        //    new Adventurer() { AdventurerCardCode = new Card() { Info = new CardInfo() { Code = "01501" } } },
        //    new Adventurer() { AdventurerCardCode = new Card() { Info = new CardInfo() { Code = "01502" } } },
        //    new Adventurer() { AdventurerCardCode = new Card() { Info = new CardInfo() { Code = "01503" } } },
        //    new Adventurer() { AdventurerCardCode = new Card() { Info = new CardInfo() { Code = "01504" } } }
        //};

        public string SceneSelected { get; set; } = "COREScene1";
        public Chapter CurrentChapter { get; set; }
        public Scene CurrentScene { get; set; }
        public GameAction CurrentAction { get; private set; }

        /*******************************************************************/
        public void SetCurrentAction(GameAction gameAction)
        {
            CurrentAction = gameAction ?? throw new ArgumentNullException(nameof(gameAction) + " gameAction cant be null");
        }
    }
}
