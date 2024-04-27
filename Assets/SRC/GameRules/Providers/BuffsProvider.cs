using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class BuffsProvider
    {
        private readonly List<Buff> _allBuffs = new();

        /*******************************************************************/
        public Buff Create()
        {
            Buff buff = new();
            _allBuffs.Add(buff);
            return buff;
        }

        /*******************************************************************/
        public async Task ExecuteAllBuffs()
        {
            foreach (Buff buff in _allBuffs) await buff.Execute();
        }

        public IEnumerable<Buff> GetBuffsForThisCard(Card cardAffected) =>
            _allBuffs.FindAll(buff => buff.CurrentCardsAffected.Contains(cardAffected));
    }
}
