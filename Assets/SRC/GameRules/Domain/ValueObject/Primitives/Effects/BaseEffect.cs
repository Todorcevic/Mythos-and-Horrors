using System;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public record BaseEffect : IViewEffect
    {
        private readonly string _description;
        public Func<Task> Logic { get; private set; }
        public string Description => _description ?? Logic.GetInvocationList().First().Method.Name;
        public virtual string CardCode { get; }
        public virtual string CardCodeSecundary { get; }

        /*******************************************************************/
        public BaseEffect(Func<Task> logic, string description = null)
        {
            Logic = logic;
            _description = description;
        }

        /*******************************************************************/
    }
}
