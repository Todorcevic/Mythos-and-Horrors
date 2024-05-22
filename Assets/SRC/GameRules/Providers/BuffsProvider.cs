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

        public void Add(IEnumerable<Buff> buffs)
        {
            _allBuffs.AddRange(buffs);
        }

        public void Remove(IEnumerable<Buff> buffs)
        {
            foreach (Buff buffToRemove in buffs) _allBuffs.Remove(buffToRemove);
        }

        /*******************************************************************/
        public async Task ExecuteAllBuffs()
        {
            foreach (Buff buff in _allBuffs.ToList()) await buff.Execute();
        }

        public async Task DeactiveAllBuffs()
        {
            foreach (Buff buff in _allBuffs.ToList()) await buff.Deactive();
        }

        public void UndoAllBuffs()
        {
            foreach (Buff buff in _allBuffs) buff.Undo();
        }

        public IEnumerable<Buff> GetBuffsAffectToThisCard(Card cardAffected) =>
            _allBuffs.FindAll(buff => buff.CurrentCardsAffected.Contains(cardAffected));

        public IEnumerable<Buff> GetBuffsForThisCardMaster(Card cardMaster) =>
            _allBuffs.FindAll(buff => buff.CardMaster == cardMaster);
    }
}
