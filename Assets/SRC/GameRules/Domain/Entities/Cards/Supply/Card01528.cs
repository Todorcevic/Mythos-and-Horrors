using ModestTree;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01528 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Police };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);
            CreateBuff(CardsToBuff, GainStrenghtActivationLogic, GainStrenghtDeactivationLogic);
            CreateFastActivation(Logic, Condition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool Condition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Exausted.IsActive) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.CreaturesInSamePlace.Any()) return false;
            return true;
        }

        private async Task Logic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Creature");
            interactableGameAction.CreateCancelMainButton();
            foreach (CardCreature cardCreature in investigator.CreaturesInSamePlace)
            {
                interactableGameAction.CreateEffect(cardCreature, new Stat(0, false), SelecteCreature, PlayActionType.Choose, investigator);

                async Task SelecteCreature()
                {
                    await _gameActionsProvider.Create(new UpdateStatesGameAction(Exausted, true));
                    await _gameActionsProvider.Create(new HarmToCardGameAction(this, this, amountDamage: 1));
                    await _gameActionsProvider.Create(new HarmToCardGameAction(cardCreature, this, amountDamage: 1));
                }
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }

        /*******************************************************************/
        private async Task GainStrenghtDeactivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new DecrementStatGameAction(cardInvestigator.Strength, 1));
        }

        private async Task GainStrenghtActivationLogic(IEnumerable<Card> cardsToBuff)
        {
            CardInvestigator cardInvestigator = cardsToBuff.OfType<CardInvestigator>().First();
            await _gameActionsProvider.Create(new IncrementStatGameAction(cardInvestigator.Strength, 1));
        }

        private IEnumerable<Card> CardsToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : new Card[0];

        /*******************************************************************/
    }
}
