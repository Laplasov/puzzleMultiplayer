using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BasicAttackCommand : IUnitCommand
{
    public void Execute(UnitLogic unitLogic, UnitCommandConfig commandConfig, ITargetCommand target)
    {
        SpaceMark[] unitMarks = target.Execute(unitLogic, commandConfig);
        if (unitMarks == null || unitMarks.Length == 0)
        {
            Debug.Log("No enemy, on attack.");
            return;
        }

        SpaceMark unitMarkCurrent = unitLogic.GetMySpaceMark();
        UnitStats unitStatsTarget = unitMarks[0].Unit.GetComponent<UnitStats>();
        UnitStats unitStatsCurrent = unitLogic.UnitStatsLogic;

        var attack = unitLogic.UnitStatsLogic.ATK;
        var WasHP = unitStatsTarget.CurrentHP;
        unitStatsTarget.OnHit(attack);
        Debug.Log($"Was {WasHP} become {unitStatsTarget.CurrentHP} - Attack {attack}");
    }
}
