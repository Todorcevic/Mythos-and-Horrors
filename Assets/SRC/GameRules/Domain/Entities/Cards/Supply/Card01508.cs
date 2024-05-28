using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01508 : CardSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Item };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            //CreateBuff(CardsToBuff, ActivationLogic, Deactivationlogic);
        }

        /*******************************************************************/
        //private IEnumerable<Card> CardsToBuff()
        //{
        //    throw new NotImplementedException();
        //}

        //private async Task ActivationLogic(IEnumerable<Card> enumerable)
        //{
        //    await Task.CompletedTask;
        //}

        //private async Task Deactivationlogic(IEnumerable<Card> enumerable)
        //{
        //    await Task.CompletedTask;
        //}

        /*******************************************************************/
    }
}
