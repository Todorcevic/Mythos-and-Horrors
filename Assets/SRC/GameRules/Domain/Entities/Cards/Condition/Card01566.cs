﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01566 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell };
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand | PlayActionType.Elude;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction chooseEnemy = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01566"));

            foreach (CardCreature creature in investigator.AllTypeCreaturesConfronted)
            {
                chooseEnemy.CreateCardEffect(creature, new Stat(0, false), EludeCreature, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01566"), cardAffected: this);

                async Task EludeCreature()
                {
                    EludeCreatureGameAction eludeGameAction = _gameActionsProvider.Create<EludeCreatureGameAction>().SetWith(investigator, creature);
                    eludeGameAction.ChangeStat(investigator.Power);
                    eludeGameAction.SuccesEffects.Add(SuccesEffet);
                    await eludeGameAction.Execute();

                    List<ChallengeTokenType> dazzle = new() { ChallengeTokenType.Ancient, ChallengeTokenType.Creature, ChallengeTokenType.Cultist, ChallengeTokenType.Danger, ChallengeTokenType.Fail };
                    if (eludeGameAction.ResultChallenge.TokensRevealed.Any(token => dazzle.Contains(token.TokenType)))
                    {
                        await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(investigator.CurrentActions, 1).Execute();
                    }
                }

                async Task SuccesEffet() => await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(creature, this, amountDamage: 1).Execute();
            }

            await chooseEnemy.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!ControlOwner.AllTypeCreaturesConfronted.Any()) return false;
            return true;
        }
    }
}
