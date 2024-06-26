using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01548 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;


        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Criminal };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);

            CreateForceReaction<PlayInvestigatorGameAction>(Condition, Logic, GameActionTime.Before);
        }

        private async Task Logic(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(playInvestigatorGameAction.ActiveInvestigator.CurrentTurns, 1));
        }

        private bool Condition(PlayInvestigatorGameAction playInvestigatorGameAction)
        {
            if (!IsInPlay) return false;
            if (playInvestigatorGameAction.ActiveInvestigator != ControlOwner) return false;
            return true;
        }

        /*******************************************************************/
    }
}
