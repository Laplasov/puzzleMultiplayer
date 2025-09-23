using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum UnitCommands 
{
    BasicCommand, 
    BasicAttackCommand,
    BasicSpellCommand,
    BasicHealthCommand
}
public enum TargetCommand
{
    ForwardTargetCommand,
    EnemyAllTargetCommand,
    FirstRowTargetCommand,
    CenterRowTargetCommand,
    LastRowTargetCommand,
    BackwardTargetCommand
}
public enum TargetScope
{
    FirstThree,
    Full,
    One,
    None
}

[System.Serializable]
public struct UnitCommandConfig
{
    public UnitCommands UnitCommandEnum;
    public TargetCommand TargetCommandEnum;
    public TargetScope TargetScopeEnum;
    public TargetScope TargetBlockEnum;
}
public class CommandBuilder 
{
    Dictionary<UnitCommands, IUnitCommand> m_commands = new Dictionary<UnitCommands, IUnitCommand>()
{
    { UnitCommands.BasicCommand, new BasicCommand() },
    { UnitCommands.BasicAttackCommand, new BasicAttackCommand() },
    { UnitCommands.BasicSpellCommand, new BasicSpellCommand() },
    { UnitCommands.BasicHealthCommand, new BasicHealthCommand() }
};
    Dictionary<TargetCommand, ITargetCommand> m_targets = new Dictionary<TargetCommand, ITargetCommand>()
{
    {TargetCommand.ForwardTargetCommand, new ForwardTargetCommand() },
    { TargetCommand.EnemyAllTargetCommand, new EnemyAllTargetCommand() },
    { TargetCommand.FirstRowTargetCommand, new FirstRowTargetCommand() },
    { TargetCommand.CenterRowTargetCommand, new CenterRowTargetCommand() },
    { TargetCommand.LastRowTargetCommand, new LastRowTargetCommand() },
    { TargetCommand.BackwardTargetCommand, new BackwardTargetCommand() }
};

    public void Build(UnitLogic unitLogic)
    {
        unitLogic.CommandSP = m_commands[unitLogic.CommandSPConfig.UnitCommandEnum];
        unitLogic.TargetSP = m_targets[unitLogic.CommandSPConfig.TargetCommandEnum];

        unitLogic.CommandSPD = m_commands[unitLogic.CommandSPDConfig.UnitCommandEnum];
        unitLogic.TargetSPD = m_targets[unitLogic.CommandSPDConfig.TargetCommandEnum];

        unitLogic.CommandHP = m_commands[unitLogic.CommandHPConfig.UnitCommandEnum];
        unitLogic.TargetHP = m_targets[unitLogic.CommandHPConfig.TargetCommandEnum];
    }
    public IUnitCommand GetCommand(UnitCommands command) => m_commands[command];
    public ITargetCommand GetTarget(TargetCommand target) => m_targets[target];

}