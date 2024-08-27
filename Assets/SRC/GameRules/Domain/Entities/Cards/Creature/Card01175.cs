using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01175 : CardCreature, IStalker, ITarget
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderByDescending(investigator => investigator.FearRecived.Value).First();

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.Byakhee };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateBuff(CardToBuff, ActivationBuff, DeactivationBuff,code: "Buff_Card01175");
        }

        /*******************************************************************/
        private async Task DeactivationBuff(IEnumerable<Card> enumerable)
        {
            Dictionary<Stat, int> stats = new()
            {
                { Strength, 1 },
                { Agility, 1 }
            };
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(stats).Execute();
        }

        private async Task ActivationBuff(IEnumerable<Card> enumerable)
        {
            Dictionary<Stat, int> stats = new()
            {
                { Strength, 1 },
                { Agility, 1 }
            };
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(stats).Execute();
        }

        private IEnumerable<Card> CardToBuff()
        {
            return Condition() ? new[] { this } : Enumerable.Empty<Card>();

            bool Condition()
            {
                if (!IsInPlay.IsTrue) return false;
                if (!IsConfronted) return false;
                if (ConfrontedInvestigator.SanityLeft > 4) return false;

                return true;
            }
        }

        /*******************************************************************/

    }
}
