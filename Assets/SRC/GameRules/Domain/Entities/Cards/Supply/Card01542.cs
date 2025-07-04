﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01542 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };
        public Stat StatBuffed { get; private set; }
        public State ActivationUsed { get; private set; }

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            CreateActivation(1, ChooseInvestigatorLogic, ChooseInvestigatorCondition, PlayActionType.Activate, new Localization("Activation_Card01542"));
            CreateBuff(CardToSelect, BuffOn, BuffOff, new Localization(() => BuffLocalizationCode()));
            CreateForceReaction<InvestigatorsPhaseGameAction>(ResetStatCondition, ResetStatLogic, GameActionTime.After);
            ActivationUsed = CreateState(false);
        }

        private string BuffLocalizationCode()
        {
            CardInvestigator cardInvestigator = _investigatorsProvider.GetInvestigatorWithThisStat(StatBuffed).InvestigatorCard;

            if (StatBuffed == cardInvestigator.Strength) return "Buff_Card01542-1";
            else if (StatBuffed == cardInvestigator.Agility) return "Buff_Card01542-2";
            else if (StatBuffed == cardInvestigator.Intelligence) return "Buff_Card01542-3";
            else return "Buff_Card01542-4";
        }

        /*******************************************************************/
        private async Task ResetStatLogic(InvestigatorsPhaseGameAction action)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActivationUsed, false).Execute();
            StatBuffed = null;
        }

        private bool ResetStatCondition(InvestigatorsPhaseGameAction action)
        {
            if (!ActivationUsed.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        private async Task BuffOn(IEnumerable<Card> cardsToBuff)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(StatBuffed, 2).Execute();
        }

        private async Task BuffOff(IEnumerable<Card> cardsToDebuff)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(StatBuffed, 2).Execute();
        }

        private IEnumerable<Card> CardToSelect()
        {
            if (!ActivationUsed.IsActive) return Enumerable.Empty<Card>();
            return new[] { _investigatorsProvider.GetInvestigatorWithThisStat(StatBuffed).InvestigatorCard };
        }

        /*******************************************************************/
        private async Task ChooseInvestigatorLogic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01542"));

            IEnumerable<Investigator> investigators = _investigatorsProvider.GetInvestigatorsInThisPlace(investigator.CurrentPlace);
            foreach (Investigator investigatorToSelect in investigators)
            {
                interactableGameAction.CreateCardEffect(investigatorToSelect.InvestigatorCard, new Stat(0, false), GainSkillInvestigator,
                    PlayActionType.Choose, investigator, new Localization("CardEffect_Card01542"));

                /*******************************************************************/
                async Task GainSkillInvestigator()
                {
                    InteractableGameAction interactableGameAction2 = _gameActionsProvider.Create<InteractableGameAction>()
                        .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01542-1"));
                    interactableGameAction2.CreateCardEffect(investigatorToSelect.InvestigatorCard, new Stat(0, false),
                        () => SetStat(investigatorToSelect.Strength), PlayActionType.Choose, investigator, new Localization("CardEffect_Card01542-1"));
                    interactableGameAction2.CreateCardEffect(investigatorToSelect.InvestigatorCard, new Stat(0, false),
                        () => SetStat(investigatorToSelect.Agility), PlayActionType.Choose, investigator, new Localization("CardEffect_Card01542-2"));
                    interactableGameAction2.CreateCardEffect(investigatorToSelect.InvestigatorCard, new Stat(0, false),
                        () => SetStat(investigatorToSelect.Intelligence), PlayActionType.Choose, investigator, new Localization("CardEffect_Card01542-3"));
                    interactableGameAction2.CreateCardEffect(investigatorToSelect.InvestigatorCard, new Stat(0, false),
                        () => SetStat(investigatorToSelect.Power), PlayActionType.Choose, investigator, new Localization("CardEffect_Card01542-4"));

                    await interactableGameAction2.Execute();

                    /*******************************************************************/
                    async Task SetStat(Stat statSelected)
                    {
                        StatBuffed = statSelected;
                        await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(ActivationUsed, true).Execute();
                        await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
                    }
                };
            }

            await interactableGameAction.Execute();
        }

        private bool ChooseInvestigatorCondition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (ControlOwner != investigator) return false;
            if (Exausted.IsActive) return false;
            return true;
        }
    }
}
