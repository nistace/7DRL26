using System;

namespace SDRL26.Battlers.Actions
{
    public enum ActionTarget
    {
        Self = 0,
        FirstAlly = 10,
        LastAlly = 11,
        AllyWithLowestHealth = 12,
        FirstEnemy = 50,
        LastEnemy = 51,
        EnemyWithLowestHealth = 52
    }
}