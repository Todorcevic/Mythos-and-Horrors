using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : DrawGameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public int Amount { get; private set; }

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, Card cardDrawed)
            => throw new NotImplementedException();

        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new DrawGameAction SetWith(Investigator investigator, IEnumerable<Card> cardsDrawed)
         => throw new NotImplementedException();

        public DrawDangerGameAction SetWith(Investigator investigator) => SetWith(investigator, 1);

        public DrawDangerGameAction SetWith(Investigator investigator, int amount)
        {
            Amount = amount;
            base.SetWith(investigator, _chaptersProvider.CurrentScene.DangerDeckZone.Cards.TakeLast(amount));
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();
        }
    }
}
