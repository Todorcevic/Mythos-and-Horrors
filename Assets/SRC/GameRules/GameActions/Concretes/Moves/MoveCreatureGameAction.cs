using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveCreatureGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionRepository;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public IStalker Creature { get; }

        /*******************************************************************/
        public MoveCreatureGameAction(IStalker creature)
        {
            Creature = creature;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionRepository.Create(new MoveCardsGameAction((Card)Creature, CardPlaceToMove().OwnZone));
        }

        private CardPlace CardPlaceToMove()
        {
            Dictionary<Investigator, CardPlace> finalResult = new();
            (CardPlace path, int distance) winner = (default, int.MaxValue);

            foreach (Investigator investigator in _investigatorsProvider.AllInvestigatorsInPlay)
            {
                (CardPlace path, int distance) result = InitializerFindPath(new[] { Creature.CurrentPlace }, investigator.CurrentPlace);
                if (result.distance == winner.distance) finalResult.Add(investigator, result.path);
                else if (result.distance < winner.distance)
                {
                    finalResult.Clear();
                    finalResult.Add(investigator, result.path);
                    winner = result;
                }
            }

            if (Creature is ITarget target && finalResult.TryGetValue(target.Investigator, out CardPlace place))
                return place;

            return finalResult.First().Value;
        }

        private (CardPlace path, int distance) InitializerFindPath(IEnumerable<CardPlace> listLocation, CardPlace moveToLocation)
        {
            CardPlace[] currentPath = new CardPlace[12];
            List<CardPlace> locationsCheck = new();
            int distance = 0;
            return FindPath(listLocation, moveToLocation);

            (CardPlace path, int distance) FindPath(IEnumerable<CardPlace> listLocation, CardPlace moveToLocation)
            {
                List<CardPlace> listToCheck = new();
                foreach (CardPlace location in listLocation)
                {
                    currentPath[distance] = location;
                    if (location == moveToLocation) return (currentPath[1] ?? currentPath[0], distance);
                    locationsCheck.Add(location);
                    listToCheck.AddRange(location.ConnectedPlacesToMove
                        .Where(cardPlace => !locationsCheck.Contains(cardPlace) && !listToCheck.Contains(cardPlace)));
                }
                distance++;
                if (listToCheck.Count > 0) return FindPath(listToCheck, moveToLocation);
                return (currentPath[0], 99);
            }
        }
    }
}
