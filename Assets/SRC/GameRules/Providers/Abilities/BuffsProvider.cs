using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class BuffsProvider
    {
        [Inject] private readonly TextsProvider _textsProvider;
        private readonly List<Buff> _buffs = new();

        private IEnumerable<Buff> AllBuffs => _buffs.ToList();

        /*******************************************************************/
        public Buff CreateBuff(Card card, Func<IEnumerable<Card>> cardsToBuff, Func<IEnumerable<Card>, Task> activationLogic,
            Func<IEnumerable<Card>, Task> deactivationLogic, string code, params string[] descriptionArgs)
        {
            Buff newBuff = new(card, cardsToBuff, new GameCommand<IEnumerable<Card>>(activationLogic),
                new GameCommand<IEnumerable<Card>>(deactivationLogic), _textsProvider.GetLocalizableText(code, descriptionArgs));
            _buffs.Add(newBuff);
            return newBuff;
        }

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
