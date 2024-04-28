using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace, IActivable
    {
        public List<Activation> Activations { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Activations = new()
            {
                new(new Stat(1), ResignActivate, ResignConditionToActivate),
                new(new Stat(1), ParleyActivate, ParleyConditionToActivate)
            };
        }

        /*******************************************************************/
        private async Task ResignActivate(Investigator activeInvestigator)
        {
            //throw new System.NotImplementedException();
            await Task.CompletedTask;
        }

        private bool ResignConditionToActivate(Investigator activeInvestigator)
        {
            //throw new System.NotImplementedException();
            return false;
        }

        private async Task ParleyActivate(Investigator activeInvestigator)
        {
            //throw new System.NotImplementedException();
            await Task.CompletedTask;
        }

        private bool ParleyConditionToActivate(Investigator activeInvestigator)
        {
            //throw new System.NotImplementedException();
            return false;
        }

        /*******************************************************************/
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);

            if (gameAction is OneInvestigatorTurnGameAction oneInvestigatorTurnGameAction)
            {
                Effect moveEffect = oneInvestigatorTurnGameAction.MoveEffects.Find(effect => effect.Card == this);
                if (!CanMove()) oneInvestigatorTurnGameAction.RemoveEffect(moveEffect);
            }
        }
        /*******************************************************************/
        private bool CanMove()
        {
            if (!Revealed.IsActive) return false;
            return true;
        }
    }
}
