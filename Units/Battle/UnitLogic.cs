using NUnit.Framework.Internal.Commands;
using System.Linq;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    UnitStatsBarUI m_UI;
    UnitStats m_unitStats;
    public CommandBuilder CommandBuilder { private get; set; }
    public IUnitCommand CommandSP { get; set; }
    public IUnitCommand CommandSPD { get; set; }
    public IUnitCommand CommandHP { get; set; }
    public ITargetCommand TargetSP { get; set; }
    public ITargetCommand TargetSPD { get; set; }
    public ITargetCommand TargetHP { get; set; }

    [SerializeField]
    public UnitCommands UnitCommandsEnum = UnitCommands.BasicCommand;
    [SerializeField]
    public TargetCommand TargetCommandEnum = TargetCommand.EnemyAllTargetCommand;

    private void Awake()
    {
        m_UI = GetComponent<UnitStatsBarUI>();
        m_unitStats = GetComponent<UnitStats>();
    }
    public void Build() => CommandBuilder.Build(this);

    public void SetCommand(IUnitCommand command, UnitCommands unitCommands)
    {
        UnitCommandsEnum = unitCommands;
        command = CommandBuilder.GetCommand(UnitCommandsEnum);
        Build();
    }
    public void SetTarget(ITargetCommand target, TargetCommand unitTarget)
    {
        TargetCommandEnum = unitTarget;
        target = CommandBuilder.GetTarget(TargetCommandEnum);
        Build();
    }

    private void Start()
    {
        m_UI.OnSP += HandleSP;
        m_UI.OnSPD += HandleSPD;
        m_UI.OnHP += HandleHP;
    }
    private void OnDestroy()
    {
        m_UI.OnSP -= HandleSP;
        m_UI.OnSPD -= HandleSPD;
        m_UI.OnHP -= HandleHP;
    }
    private void HandleSP() => CommandSP?.ExecuteOnSP(this);
    private void HandleSPD() => CommandSPD?.ExecuteOnSPD(this);
    private void HandleHP() => CommandHP?.ExecuteOnHP(this);

    public Vector2Int GetMyPosition() => GetMySpaceMark().Dimension;
    public SpaceMark GetMySpaceMark()
    {
        var units = GridRegistry.Instance.GetAllUnits(PlacementType.Battlefield);
        return units.FirstOrDefault(u => u.Item1 == m_unitStats).Item2;
    }

}
