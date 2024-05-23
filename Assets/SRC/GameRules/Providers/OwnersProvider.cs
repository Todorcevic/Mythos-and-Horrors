using System;
using System.Collections.Generic;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class OwnersProvider
    {
        private readonly List<Owner> _owners = new();

        public List<Owner> AllOwners => _owners.ToList();

        /*******************************************************************/
        public void AddOwner(Owner owner)
        {
            if (_owners.Contains(owner)) throw new InvalidOperationException("Investigator already added");
            _owners.Add(owner);
        }
    }
}
