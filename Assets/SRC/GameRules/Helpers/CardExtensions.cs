using System;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public static class CardExtensions
    {
        public static CardPlace GetPlaceToStalkerMove(this IStalker stalker, IEnumerable<Investigator> investigatorsInplay)
        {
            if (stalker is ITarget target && target.IsUniqueTarget) return target.TargetInvestigator.IsInPlay ?
                    stalker.CurrentPlace.DistanceTo(target.TargetInvestigator.CurrentPlace).path :
                    stalker.CurrentPlace;

            Dictionary<Investigator, CardPlace> finalResult = new();
            (CardPlace path, int distance) winner = (default, int.MaxValue);

            foreach (Investigator investigator in investigatorsInplay)
            {
                (CardPlace path, int distance) result = stalker.CurrentPlace.DistanceTo(investigator.CurrentPlace);
                if (result.distance == winner.distance) finalResult.Add(investigator, result.path);
                else if (result.distance < winner.distance)
                {
                    finalResult.Clear();
                    finalResult.Add(investigator, result.path);
                    winner = result;
                }
            }

            if (stalker is ITarget targetCreature && finalResult.TryGetValue(targetCreature.TargetInvestigator, out CardPlace place))
                return place;

            return finalResult.First().Value;
        }

        public static (CardPlace path, int distance) DistanceTo(this CardPlace origin, CardPlace cardPlace)
        {
            if (origin == null) throw new ArgumentNullException(nameof(origin), "Cardplace is NULL");
            CardPlace[] currentPath = new CardPlace[12];
            List<CardPlace> locationsCheck = new();
            int distance = 0;
            return FindPath(new[] { origin }, cardPlace);

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
                return (currentPath[0], int.MaxValue);
            }
        }
    }
}