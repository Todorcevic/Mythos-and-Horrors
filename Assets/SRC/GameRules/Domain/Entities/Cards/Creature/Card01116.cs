﻿using System.Collections.Generic;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01116 : CardCreature, ITarget, IStalker, ICounterAttackable
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        Investigator ITarget.TargetInvestigator => _investigatorsProvider.GetTopInvestigatorsStrength;
        public override IEnumerable<Tag> Tags => new[] { Tag.Ghoul, Tag.Humanoid, Tag.Monster, Tag.Elite };
    }
}
