using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardAdversityLimbo : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        private bool IsPeril => HasThisTag(Tag.Deprivation);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            IsInPlay = new(() => CurrentZone.ZoneType == ZoneType.Limbo);
            CreateBuff(CardsToBuff, ActivationLogic, DeactivationLogic, new Localization("Buff_CardAdversityLimbo"));
        }

        /*******************************************************************/
        private async Task DeactivationLogic(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private async Task ActivationLogic(IEnumerable<Card> enumerable) => await Task.CompletedTask;

        private IEnumerable<Card> CardsToBuff()
        {
            if (IsInPlay.IsFalse) return Enumerable.Empty<Card>();
            return _investigatorsProvider.AllInvestigatorsInPlay.Where(investigator => investigator.Isolated.IsActive)
                .Select(investigator => investigator.InvestigatorCard);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => _chaptersProvider.CurrentScene.LimboZone;

        public override async Task PlayRevelationFor(Investigator investigator)
        {
            if (IsPeril) await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator.Isolated, true).Execute();
            await ObligationLogic(investigator);
            if (IsPeril) await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(investigator.Isolated, false).Execute();
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        /*******************************************************************/
        protected abstract Task ObligationLogic(Investigator investigator);
    }
}
