using System.Collections.Generic;
using System.Linq;
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
                for (int i = 0; i < challengeGameAction.TotalDifferenceValue * -1; i++)
                {
                    if (investigator.IsInPlay) await FailEffect();

                    /*******************************************************************/
                    async Task FailEffect()
                    {
                        InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, $"Choose: {challengeGameAction.TotalDifferenceValue * -1 - i} left", investigator);
                        interactableGameAction.Create().SetCard(investigator.InvestigatorCard)
                            .SetLogic(TakeDamageAndFear)
                            .SetDescription("Take Damage and Fear")
                            .SetInvestigator(investigator);

                        foreach (Card card in investigator.DiscardableCardsInHand)
                        {
                            interactableGameAction.Create().SetCard(card)
                                .SetLogic(Discard)
                                .SetDescription("Discard")
                                .SetInvestigator(investigator);

                            async Task Discard()
                            {
                                await _gameActionsProvider.Create(new DiscardGameAction(card));
                            }
                        }

                        await _gameActionsProvider.Create(interactableGameAction);

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
