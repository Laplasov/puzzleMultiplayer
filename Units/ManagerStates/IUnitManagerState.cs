public interface IUnitManagerState
{
    public IUnitTransformer UTransform { set; get; }
    void Enter(UnitManager unitManager, PlacementValidator placementValidator, CellCalculator cellCalculator, IUnitTransformer UTransform);
    void Execute(SpaceMark target, PlacementSystem board);
    void Exit();
}
