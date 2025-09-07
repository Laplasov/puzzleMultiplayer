using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GridRegistry : MonoBehaviour
{
    private static Dictionary<PlacementType, Dictionary<Vector2Int, SpaceMark>> _gridRegister = new();
    public static void RegisterGrid(PlacementType type, Dictionary<Vector2Int, SpaceMark> grid) => 
        _gridRegister[type] = grid;
    public static Dictionary<Vector2Int, SpaceMark> GetGrid(PlacementType type) => 
        _gridRegister.TryGetValue(type, out var grid) ? grid : null;
    public static Dictionary<PlacementType, Dictionary<Vector2Int, SpaceMark>> GetAll() => _gridRegister;
}
