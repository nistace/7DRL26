namespace SDRL26.Battles.Battlers
{
   public enum ActionTarget
   {
      Self = 0,
      FirstAlly = 10,
      LastAlly = 11,
      AllyWithLowestHealth = 12,
      RandomAlly = 13,
      FirstDeadAlly = 14,
      AllyWithMostMissingHealth = 15,
      ÀllyWithLowestHealthRatio = 16,
      FirstEnemy = 50,
      LastEnemy = 51,
      EnemyWithLowestHealth = 52,
      RandomEnemy = 53,
      EnemyWithMostMissingHealth = 54,
      EnemyWithMostHealth = 55,
      EnemyWithLowestHealthRatio = 56
   }
}