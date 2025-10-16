using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridRegistry
{
    protected GridRegistry() { }
    private static GridRegistry _instance;
    public static GridRegistry Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GridRegistry();
            }
            return _instance;
        }
        private set { _instance = value; }
    }

    private Dictionary<PlacementType, Dictionary<Vector2Int, SpaceMark>> _gridRegister = new();
    public void RegisterGrid(PlacementType type, Dictionary<Vector2Int, SpaceMark> grid) => 
        _gridRegister[type] = grid;
    public Dictionary<Vector2Int, SpaceMark> GetGrid(PlacementType type) => 
        _gridRegister.TryGetValue(type, out var grid) ? grid : null;
    public SpaceMark GetMark(PlacementType type, Vector2Int vector) => GetGrid(type)[vector];
    public Dictionary<PlacementType, Dictionary<Vector2Int, SpaceMark>> GetAll() => _gridRegister;

    public event Func<(UnitStats, SpaceMark)> OnGetUnit;
    public List<(UnitStats, SpaceMark)> GetAllUnitsWithType(PlacementType type)
    {
        if (OnGetUnit == null) return new List<(UnitStats, SpaceMark)>();

        var results = new List<(UnitStats, SpaceMark)>();
        foreach (Func<(UnitStats, SpaceMark)> subscriber in OnGetUnit.GetInvocationList())
        {
            var result = subscriber();
            if (result.Item1 != null && result.Item2.Type == type) results.Add(result);
        }
        return results;
    }

    public List<(UnitStats, SpaceMark)> GetAllUnits()
    {
        if (OnGetUnit == null) return new List<(UnitStats, SpaceMark)>();

        var results = new List<(UnitStats, SpaceMark)>();
        foreach (Func<(UnitStats, SpaceMark)> subscriber in OnGetUnit.GetInvocationList())
        {
            var result = subscriber();
            if (result.Item1 != null) results.Add(result);
        }
        return results;
    }

}
