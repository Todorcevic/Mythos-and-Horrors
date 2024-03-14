using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionRepository;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Investigator Investigator { get; }
        public Card CardDrawed { get; private set; }

        /*******************************************************************/
        public DrawDangerGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardDrawed = _chaptersProvider.CurrentScene.CardDangerToDraw;
            await _gameActionRepository.Create(new UpdateStatesGameAction(CardDrawed.FaceDown, false));


            await _gameActionRepository.Create(new MoveCardsGameAction(CardDrawed, _chaptersProvider.CurrentScene.LimboZone)); 
            //TODO: Resolve card (Revelation, Creature, etc...)
        }
    }
}
