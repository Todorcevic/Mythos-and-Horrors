using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01597 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Madness };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            await _gameActionRepository.Create<PayResourceGameAction>().SetWith(investigator, investigator.Resources.Value).Start();
        }
    }
}
