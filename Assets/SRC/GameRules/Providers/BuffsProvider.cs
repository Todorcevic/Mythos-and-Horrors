using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class BuffsProvider
    {
        [Inject] private readonly DiContainer _diContainer;
        private readonly List<Buff> _allBuffs = new();

        /*******************************************************************/
        public Buff Create(Card cardMaster, string description, Func<List<Card>> cardAffected, Func<Card, Task> activationLogic, Func<Card, Task> deactivationLogic)
        {
            Buff buff = _diContainer.Instantiate<Buff>(new List<object> { cardMaster, description, cardAffected, activationLogic, deactivationLogic });
            _allBuffs.Add(buff);
            return buff;
        }

        public async Task CheckAllBuffs(GameAction gameAction)
        {
            foreach (Buff buff in _allBuffs)
            {
                await buff.Check();
            }
        }
        /*******************************************************************/
        public List<Buff> GetBuffsForThisCard(Card cardAffected) =>
            _allBuffs.FindAll(buff => buff.CardsToBuff.Invoke().Contains(cardAffected));
    }
}
