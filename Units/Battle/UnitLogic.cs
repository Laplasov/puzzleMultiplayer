using NUnit.Framework.Internal.Commands;
using System.Linq;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    UnitStatsBarUI m_UI;
    UnitStats m_unitStats;
    public UnitStats UnitStatsLogic => m_unitStats;
    public CommandBuilder CommandBuilder { private get; set; }
    public IUnitCommand CommandSP { get; set; }
    public IUnitCommand CommandSPD { get; set; }
    public IUnitCommand CommandHP { get; set; }
    public ITargetCommand TargetSP { get; set; }
    public ITargetCommand TargetSPD { get; set; }
    public ITargetCommand TargetHP { get; set; }

    [SerializeField]
    public UnitCommandConfig CommandSPConfig;
    [SerializeField]
    public UnitCommandConfig CommandSPDConfig;
    [SerializeField]
    public UnitCommandConfig CommandHPConfig;

    private void Awake()
    {
        m_UI = GetComponent<UnitStatsBarUI>();
        m_unitStats = GetComponent<UnitStats>();
    }
    [Button("Build")]
    public void Build() => CommandBuilder.Build(this);
    [Button("Toggle")]
    public void ToggleUI() => m_UI.Toggle();
    public void SetCommand(IUnitCommand command, UnitCommands unitCommands)
    {
        CommandSPConfig.UnitCommandEnum = unitCommands;
        command = CommandBuilder.GetCommand(CommandSPConfig.UnitCommandEnum);
        Build();
    }
    public void SetTarget(ITargetCommand target, TargetCommand unitTarget)
    {
        CommandSPConfig.TargetCommandEnum = unitTarget;
        target = CommandBuilder.GetTarget(CommandSPConfig.TargetCommandEnum);
        Build();
    }

    private void Start()
    {
        m_UI.OnSP += HandleSP;
        m_UI.OnSPD += HandleSPD;
        m_UI.OnHP += HandleHP;
        m_UI.OnHP += OnUnitDead;
    }
    private void OnDestroy()
    {
        m_UI.OnSP -= HandleSP;
        m_UI.OnSPD -= HandleSPD;
        m_UI.OnHP -= HandleHP;
        m_UI.OnHP -= OnUnitDead;
    }
    private void HandleSP() => CommandSP?.Execute(this, CommandSPConfig, TargetSP);
    private void HandleSPD() => CommandSPD?.Execute(this, CommandSPDConfig, TargetSPD);
    private void HandleHP() => CommandHP?.Execute(this, CommandHPConfig, TargetHP);

    public Vector2Int GetMyPosition()
    {
        var width = m_unitStats.GetSize().x - 1;

        if (m_unitStats.Ownership == Owner.Ally)
            return GetMySpaceMark().Dimension + new Vector2Int(0, width);
        else
            return GetMySpaceMark().Dimension - new Vector2Int(0, width);
    }

    public SpaceMark GetMySpaceMark()
    {
        var units = GridRegistry.Instance.GetAllUnits(PlacementType.Battlefield);
        return units.FirstOrDefault(u => u.Item1 == m_unitStats && u.Item2.PointerMark == null).Item2;
    }
    public void OnUnitDead()
    {
        if (m_unitStats.CurrentHP >= 0) return; 
        //ToggleUI();
        m_UI.IsDead = true;
        m_unitStats.IsDead = true;
    }
}
