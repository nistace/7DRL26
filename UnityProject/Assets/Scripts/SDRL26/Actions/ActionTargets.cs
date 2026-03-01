using System;

namespace SDRL26.Battlers.Actions
{
    public enum ActionTarget
    {
        Self = 0,
        AllyAliveFirst = 10,
        AllyAliveLast = 11,
        EnemyAliveFirst = 50,
        EnemyAliveLast = 51,
    }
}