public interface IUnitManagerState
{
    public IUnitTransformer UTransform { set; get; }
    void Enter(PlacementValidator placementValidator, CellCalculator cellCalculator, IUnitTransformer UTransform);
    void Execute(UnitManager unitManager, SpaceMark target, PlacementSystem board);
    void Exit(UnitManager unitManager);
}
