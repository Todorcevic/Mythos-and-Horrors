using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public interface IWeaponAttackable
    {
        Task AttackEnemy(CardCreature creature);
        bool AttackCondition();
    }
}
