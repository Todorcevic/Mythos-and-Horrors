using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01158 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Curse };
        public Stat ChoiseRemaining { get; private set; }

        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            ChallengePhaseGameAction challengeGameAction = _gameActionsProvider.Create<ChallengePhaseGameAction>();
            await challengeGameAction.SetWith(investigator.Power, 5, "Ulmodoth Wrat" + Info.Name, this, failEffect: MultiFailEffect).Execute();

            /*******************************************************************/
            async Task MultiFailEffect()
            {
                ChoiseRemaining = new Stat(challengeGameAction.ResultChallenge.TotalDifferenceValue * -1, false);

                while (ChoiseRemaining.Value > 0)
                {
                    await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(ChoiseRemaining, 1).Execute();
                    if (investigator.IsInPlay) await FailEffect();

                    /*******************************************************************/
                    async Task FailEffect()
                    {
                        InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                            .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Card01158", DescriptionParams());

                        interactableGameAction.CreateEffect(investigator.InvestigatorCard, new Stat(0, false), TakeDamageAndFear, PlayActionType.Choose, playedBy: investigator);
                        foreach (Card card in investigator.DiscardableCardsInHand)
                        {
                            interactableGameAction.CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, playedBy: investigator);

                            /*******************************************************************/
                            async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                        }

                        await interactableGameAction.Execute();

                        /*******************************************************************/
                        async Task TakeDamageAndFear() =>
                            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, this, amountDamage: 1, amountFear: 1).Execute();

                        string DescriptionParams() => (ChoiseRemaining.Value + 1).ToString();
                    }
                }
            }
        }
    }
}
