﻿using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record Effect : IViewEffect
    {
        private static readonly Effect _nullEffect = new(null, null, "Null Effect", () => true, () => Task.CompletedTask);
        public static Effect ContinueEffect => _nullEffect;

        public IEffectable Effectable { get; init; }
        public Investigator Investigator { get; init; }
        public Investigator InvestigatorAffected { get; init; }
        public Func<bool> CanPlay { get; private set; }
        public Func<Task> Logic { get; init; }
        public string CardCode => Investigator?.Code;
        public string Description { get; init; }
        public string CardCodeSecundary => InvestigatorAffected?.Code;

        /*******************************************************************/
        public Effect(IEffectable effectable, Investigator investigator, string description, Func<bool> canPlay, Func<Task> logic, Investigator investigatorAffected = null)
        {
            Effectable = effectable;
            Investigator = investigator;
            InvestigatorAffected = investigatorAffected;
            CanPlay = canPlay;
            Logic = logic;
            Description = description;
        }
    }
}