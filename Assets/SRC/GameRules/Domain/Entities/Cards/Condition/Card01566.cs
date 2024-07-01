using System.Collections.Generic;
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
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Enemy");
            chooseEnemy.CreateCancelMainButton();

            foreach (CardCreature creature in investigator.AllTypeCreaturesConfronted)
            {
                chooseEnemy.CreateEffect(creature, new Stat(0, false), EludeCreature, PlayActionType.Choose, investigator);

                async Task EludeCreature()
                {
                    EludeCreatureGameAction eludeGameAction = new(investigator, creature);
                    eludeGameAction.ChangeStat(investigator.Power);
                    eludeGameAction.SuccesEffects.Add(SuccesEffet);
                    await _gameActionsProvider.Create(eludeGameAction);

                    List<ChallengeTokenType> dazzle = new() { ChallengeTokenType.Ancient, ChallengeTokenType.Creature, ChallengeTokenType.Cultist, ChallengeTokenType.Danger, ChallengeTokenType.Fail };
                    if (eludeGameAction.ResultChallenge.TokensRevealed.Any(token => dazzle.Contains(token.TokenType)))
                    {
                        await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(investigator.CurrentTurns, 1).Start();
                    }
                }

                async Task SuccesEffet() => await _gameActionsProvider.Create(new HarmToCardGameAction(creature, this, amountDamage: 1));
            }

            await chooseEnemy.Start();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!ControlOwner.AllTypeCreaturesConfronted.Any()) return false;
            return true;
        }
    }
}
