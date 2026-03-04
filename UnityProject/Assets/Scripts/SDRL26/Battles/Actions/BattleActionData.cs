using System.Collections.Generic;

namespace SDRL26.Battles.Actions
{
   public class BattleActionData
   {
      public enum MeasurableData
      {
         DamageDealt = 0,
      }

      public IActionPerformer ActionDoer { get; }

      public Dictionary<MeasurableData, int> Measurables { get; } = new();

      public int this[MeasurableData measurableData] => Measurables.GetValueOrDefault(measurableData);

      public BattleActionData(IActionPerformer action_doer)
      {
         ActionDoer = action_doer;
      }

      public void AddMeasurable(MeasurableData measurable, int dataChange) => Measurables[measurable] = this[measurable] + dataChange;
   }
}