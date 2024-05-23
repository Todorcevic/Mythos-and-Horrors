using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class BuffsProvider
    {
        [Inject] private readonly CardsProvider _cardsProvider;

        private IEnumerable<Buff> AllBuffs => _cardsProvider.AllCards.SelectMany(card => card.AllBuffs);

        /*******************************************************************/
        public async Task ExecuteAllBuffs()
        {
            foreach (Buff buff in AllBuffs) await buff.Execute();
        }

        public void UndoAllBuffs()
        {
            foreach (Buff buff in AllBuffs) buff.Undo();
        }

        public IEnumerable<Buff> GetBuffsAffectToThisCard(Card cardAffected) =>
            AllBuffs.Where(buff => buff.CurrentCardsAffected.Contains(cardAffected));

        public IEnumerable<Buff> GetBuffsForThisCardMaster(Card cardMaster) =>
            AllBuffs.Where(buff => buff.CardMaster == cardMaster);
    }
}
