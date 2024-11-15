﻿namespace MythosAndHorrors.GameRules
{
    public interface IDamageable
    {
        Stat Health { get; }
        Stat DamageRecived { get; }
        int HealthLeft => Health.Value - DamageRecived.Value;

        bool CanBeHealed => DamageRecived.Value > 0;
    }
}
