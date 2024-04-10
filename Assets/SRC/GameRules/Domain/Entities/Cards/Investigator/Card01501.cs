using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class Card01501 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly BuffsProvider _buffsProvider;

        /*******************************************************************/
        [Inject]
        public void Init()
        {

        }

        public override async Task StarEffect() => await Task.CompletedTask;

        public override int StarValue() => Owner.CurrentPlace.Hints.Value;


        /*******************************************************************/

    }
}
