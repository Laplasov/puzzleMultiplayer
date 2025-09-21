using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public enum UnitCommands { BasicCommand }
public enum TargetCommand { ForwardTargetCommand, EnemyAllTargetCommand }
public class CommandBuilder 
{
    Dictionary<UnitCommands, IUnitCommand> m_commands = new Dictionary<UnitCommands, IUnitCommand>()
{
    { UnitCommands.BasicCommand, new BasicCommand() }
};
    Dictionary<TargetCommand, ITargetCommand> m_targets = new Dictionary<TargetCommand, ITargetCommand>()
{
    { TargetCommand.ForwardTargetCommand, new ForwardTargetCommand() },
    { TargetCommand.EnemyAllTargetCommand, new EnemyAllTargetCommand() }
};

    public void Build(UnitLogic unitLogic)
    {
        unitLogic.CommandSP = m_commands[unitLogic.UnitCommandsEnum];
        unitLogic.TargetSP = m_targets[unitLogic.TargetCommandEnum];
    }
    public IUnitCommand GetCommand(UnitCommands command) => m_commands[command];
    public ITargetCommand GetTarget(TargetCommand target) => m_targets[target];

}
