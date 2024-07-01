using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01158 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Curse };

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            ChallengePhaseGameAction challengeGameAction = null;
            challengeGameAction = new(investigator.Power, 5, "Ulmodoth Wrat", this, failEffect: MultiFailEffect);
            await _gameActionsProvider.Create(challengeGameAction);

            /*******************************************************************/
            async Task MultiFailEffect()
            {
                for (int i = 0; i < challengeGameAction.ResultChallenge.TotalDifferenceValue * -1; i++)
                {
                    if (investigator.IsInPlay) await FailEffect();

                    /*******************************************************************/
                    async Task FailEffect()
                    {
                        InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                            .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, $"Choose: {challengeGameAction.ResultChallenge.TotalDifferenceValue * -1 - i} left");
                        interactableGameAction.CreateEffect(investigator.InvestigatorCard, new Stat(0, false), TakeDamageAndFear, PlayActionType.Choose, playedBy: investigator);

                        foreach (Card card in investigator.DiscardableCardsInHand)
                        {
                            interactableGameAction.CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, playedBy: investigator);

                            /*******************************************************************/
                            async Task Discard() => await _gameActionsProvider.Create(new DiscardGameAction(card));
                        }

                        await interactableGameAction.Start();

                        /*******************************************************************/
                        async Task TakeDamageAndFear()
                        {
                            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountDamage: 1, amountFear: 1));
                        }
                    }
                }
            }


        }
    }
}
