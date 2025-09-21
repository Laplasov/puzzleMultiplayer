using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UnitStats : MonoBehaviour, IPlacementRule, IOwnership
{
    [SerializeField]
    private UnitBaseStats unitBaseStats;
    [SerializeField]
    public PlacementRule placementRule;
    [SerializeField]
    public Vector2Int Size;
    public List<StatsModifier> StatsModifier;

    public string Name => unitBaseStats.name;
    public int HP => unitBaseStats.HP;
    public int ATK => unitBaseStats.ATK;
    public int SP => unitBaseStats.SP;
    public int SPD => unitBaseStats.SPD;
    public int HPMOD => unitBaseStats.HP + (StatsModifier?.Sum(x => x.HP) ?? 0);
    public int ATKMOD => unitBaseStats.ATK + (StatsModifier?.Sum(x => x.ATK) ?? 0);

    public int CurrentHP {  get; private set; }
    public Owner Ownership { get; set; } = Owner.Neutral;
    void Awake() => ResetHP();
    public void ResetHP() => CurrentHP = HP;

    [Button("Get Stats")]
    void GetStats()
    {
        Debug.Log($"Name: {Name}\n" +
            $"HP: {HP}\n" +
            $"ATK: {ATK}\n" +
            $"HPMOD: {HPMOD}\n" +
            $"ATKMOD: {ATKMOD}\n");
    }

    [Button("Add Stats Modifier")]
    void AddStatsModifier()
    {
        StatsModifier.Add(new StatsModifier(10,10));
        GetStats();
    }
    public PlacementRule GetPlacementRule() => placementRule;
    public Vector2Int GetSize() => Size;
}
