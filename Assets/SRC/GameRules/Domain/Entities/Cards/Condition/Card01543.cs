﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01543 : CardConditionPlayFromHand
    {
        [Inject] private readonly InvestigatorsProvider investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override bool IsFast => true;
        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01543"));

            foreach (Investigator investigatorToChoose in investigator.CurrentPlace.InvestigatorsInThisPlace)
            {
                interactable.CreateCardEffect(investigatorToChoose.AvatarCard, new Stat(0, false), Draw, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01543"));

                /*******************************************************************/
                async Task Draw()
                {
                    await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigatorToChoose).Execute();
                    await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigatorToChoose).Execute();
                    await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigatorToChoose).Execute();
                }
            }
            await interactable.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;
    }
}
