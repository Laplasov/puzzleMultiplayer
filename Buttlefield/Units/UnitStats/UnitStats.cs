using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum DamageType { Hazard, Unit}
public struct Damage
{
    public int damage;
    public DamageType damageType;
    public UnitLogic unitLogic;
}
public class UnitStats : MonoBehaviour, IPlacementRule, IOwnership
{
    [SerializeField]
    private UnitBaseStats unitBaseStats;
    [SerializeField]
    public PlacementRule placementRule;
    [Header("X - Width; Y - Height")]
    [SerializeField]
    public Vector2Int Size;
    public List<StatsModifier> StatsModifier;

    public string Name => name;
    public int HP => unitBaseStats.HP;
    public int ATK => unitBaseStats.ATK;
    public int SP => unitBaseStats.SP;
    public int SPD => unitBaseStats.SPD;
    public int HPMOD => unitBaseStats.HP + (StatsModifier?.Sum(x => x.HP) ?? 0);
    public int ATKMOD => unitBaseStats.ATK + (StatsModifier?.Sum(x => x.ATK) ?? 0);
    public int CurrentHP {  get; private set; }
    public Owner Ownership { get; set; } = Owner.Neutral;
    public bool IsDead { get; set; } = false;
    public Queue<Damage> DamageTaken = new Queue<Damage>();
    void Awake() => ResetHP();
    public void ResetHP() => CurrentHP = HP;
    public DamageType OnHit(int Damage, DamageType type, UnitLogic unitLogic)
    {
        var damage = new Damage();
        damage.damage = Damage;
        damage.damageType = type;
        damage.unitLogic = unitLogic;

        DamageTaken.Enqueue(damage);

        CurrentHP = Mathf.Max(0, CurrentHP - Damage);
        return type;
    }

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
