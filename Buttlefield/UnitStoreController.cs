using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class UnitStoreController : MonoBehaviour
{
    [SerializeField]
    UnitManager m_unitManager;

    [SerializeField]
    Store m_store;

    [SerializeField]
    UnitPrefabsSO m_unitsPrefabs;

    [SerializeField]
    Image m_timerBar;

    [SerializeField]
    TMP_Text m_timer;

    [SerializeField]
    TMP_Text m_waves;

    [SerializeField]
    RectTransform m_victory;

    [SerializeField]
    RectTransform m_defeat;

    SpaceMark[] m_spaceMark;
    List<GameObject> m_units;
    System.Random m_random;
    EnemySpawner m_enemySpawner;

    public Action<string> OnGetPrefab;

    float m_roundTime = 45f;
    float m_damageTimer = 3f;

    bool m_isBattle;
    float m_currentTime;
    float m_damageTimerTick = 0f;

    void Awake()
    {
        m_store.OnBuyUnit += CreateUnit;
        m_units = m_unitsPrefabs.GetAllPrefab();
        m_random = new System.Random();
        m_unitManager.OnRound += SetBattleUI;
        m_enemySpawner = m_unitManager.EnemySpawner;
        m_unitManager.OnLooser += ShowWiner;
    }
    void Start()
    {
        Init();
        Reroll();
        m_waves.text = $"{m_enemySpawner.Index + 1}/{m_enemySpawner.PreparedEnemiesSO.Length}";
    }
    private void Update()
    {
        if (m_isBattle)
            UpdateAnimation();
    }

    void OnDisable()
    {
        m_store.OnBuyUnit -= CreateUnit;
        m_unitManager.OnRound -= SetBattleUI;
        m_unitManager.OnLooser -= ShowWiner;
    }

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
    public void SetBattleUI(bool isBattle)
    {
        m_isBattle = isBattle;
        m_currentTime = m_roundTime;
        m_timer.text = GetTime();
        m_timerBar.fillAmount = 1f;
        m_waves.text = $"{m_enemySpawner.Index + 1}/{m_enemySpawner.PreparedEnemiesSO.Length}"; 
    }
    void UpdateAnimation()
    {
        m_currentTime -= Time.deltaTime;

        bool isNegative = m_currentTime < 0;
        m_timer.text = GetTime();
        if (isNegative)
        {
            m_damageTimerTick += Time.deltaTime;
            if (m_damageTimerTick >= m_damageTimer)
            {
                foreach (var unitStats in m_unitManager.UnitLogics) 
                {
                    var damage = unitStats.UnitStatsLogic.HP / 10;
                    unitStats.UnitStatsLogic.OnHit(damage, DamageType.Hazard, null);
                }
                m_damageTimerTick = 0f;
            }
        }

        m_timerBar.fillAmount = m_currentTime / m_roundTime;
    }

    public void ShowWiner(Owner owner) => StartCoroutine(ShowWinerConcurrent(owner));
    IEnumerator ShowWinerConcurrent(Owner owner)
    {
        RectTransform winner = owner == Owner.Enemy ? m_victory : m_defeat;

        winner.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        winner.gameObject.SetActive(false);
    }

    string GetTime()
    {
        bool isNegative = m_currentTime < 0;
        float absoluteTime = Mathf.Abs(m_currentTime);

        int minutes = Mathf.FloorToInt(absoluteTime / 60f);
        int seconds = Mathf.FloorToInt(absoluteTime % 60f);

       return isNegative ? $"-{minutes:00}:{seconds:00}" : $"{minutes:00}:{seconds:00}";
    }
}
