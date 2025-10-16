using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicHealthCommand : IUnitCommand
{
    public void Execute(UnitLogic unitLogic, UnitCommandConfig commandConfig, ITargetCommand target)
    {
        UnitStats unitStatsCurrent = unitLogic.UnitStatsLogic;
        unitStatsCurrent.DamageTaken.TryDequeue(out var damageTaken);

        string inflict;
        switch (damageTaken.damageType)
        {
            case DamageType.Unit:
                inflict = damageTaken.unitLogic.UnitStatsLogic.Name;
                break;
            case DamageType.Hazard:
                inflict = "Hazard";
                break;
            default:
                inflict = "No identity";
                break;
        }
        /*
        Debug.Log($"{unitStatsCurrent.Name} was damaged, " +
            $"current HP {unitStatsCurrent.CurrentHP}.\n" +
            $"Damage {damageTaken.damage}," +
            $"Damage type {damageTaken.damageType.ToString("G")}, " +
            $"Unit inflict {inflict}");
        */
    }
}
