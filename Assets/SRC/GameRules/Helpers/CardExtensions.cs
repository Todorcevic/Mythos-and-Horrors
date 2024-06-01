using System;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public static class CardExtensions
    {
        //public static void CreateReaction<T>(this Card card, Func<T, bool> condition, Action<T> logic, bool isPermanent = true) where T : GameAction
        //{
        //    card.Reactions.Add(new Reaction<T>(condition, logic, isPermanent));
        //}

        //public static Stat CreateStat(this Card card, int value)
        //{
        //    return new Stat(value, card);
        //}

        //public static State CreateState(this Card card, bool value)
        //{
        //    return new State(value, card);
        //}

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