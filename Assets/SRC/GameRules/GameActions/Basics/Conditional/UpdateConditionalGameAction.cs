using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{

    public class UpdateConditionalGameAction : GameAction
    {
        private bool? _oldValue;

        public bool? Value { get; private set; }
        public Conditional Conditional { get; private set; }

        /*******************************************************************/
        public UpdateConditionalGameAction SetWith(Conditional conditional, bool? value)
        {
            Conditional = conditional;
            Value = value;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _oldValue = Conditional.CurrentActivate;
            Conditional.UpdateActivationTo(Value);
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            Conditional.UpdateActivationTo(_oldValue);
            await base.Undo();
        }
    }
}
