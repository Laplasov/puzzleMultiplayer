using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public class UnitStoreController : MonoBehaviour
{
    [SerializeField]
    UnitManager m_unitManager;

    [SerializeField]
    Store m_store;

    [SerializeField]
    UnitPrefabsSO m_unitsPrefabs;

    SpaceMark[] m_spaceMark;
    List<GameObject> m_units;
    System.Random m_random;

    public Action<string> OnGetPrefab;

    void Awake()
    {
        m_store.OnBuyUnit += CreateUnit;
        m_units = m_unitsPrefabs.GetAllPrefab();
        m_random = new System.Random();
    }
    void Start()
    {
        Init();
        Reroll();
    }

    void OnDisable() => m_store.OnBuyUnit -= CreateUnit;
    int GetRandom() => m_random.Next(0, m_units.Count);
    void Init()
    {
        var collection = GridRegistry.Instance.GetGrid(PlacementType.UnitHolder)
            .Where(kvp => kvp.Key.y == 0);

        m_spaceMark = new SpaceMark[collection.Count()];
        var i = 0;
        foreach (var item in collection)
        {
            m_spaceMark[i] = item.Value;
            ++i;
        }
        m_store.OnGetFreeMark = GetFreeMark;
    }
    public void CreateUnit(GameObject unit)
    {
        SpaceMark vacantMark = GetFreeMark();
        if (vacantMark != null)
            m_unitManager.CreateUnit(vacantMark, unit.name, Owner.Ally);
    }
    SpaceMark GetFreeMark()
    {
        SpaceMark vacantMark = null;
        foreach (var mark in m_spaceMark)
        {
            if (mark.Unit == null)
            {
                vacantMark = mark;
                break;
            }
        }
        if (vacantMark == null)
            Debug.Log("No Vacant Marks!");
        return vacantMark;
    }

    public void Reroll()
    {
        m_store.ClearAllUnits();
        for (var i = 0; i < m_store.SlotContents.Length; i++ )
            m_store.CreateUnitSlot(m_units[GetRandom()]);
    }
}
