using Assets.Scripts.Units;
using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using System.Linq;
public class UnitManager : MonoBehaviour
{
    [SerializeField]
    private CameraMovement m_cameraSystem;
    [SerializeField]
    private MonoBehaviour LocalUnitTransformerComponent;
    [SerializeField]
    private MonoBehaviour PhotonUnitTransformerComponent;
    [SerializeField]
    private UnitPrefabsSO m_prefabs;
    [SerializeField]
    CellCalculator m_cellCalculator;

    List<SpaceMark> m_spaceMarks = new ();
    bool m_isProcessing = false;
    PlacementValidator m_placementValidator = new ();
    IUnitTransformer UTransform;
    CommandBuilder m_commandBuilder;
    public enum UTType { LocalUnitTransformer, PhotonUnitTransformer }
    public IUnitManagerState State { get; set; }
    public List<UnitLogic> UnitLogics { get; set; } = new ();

    private void Awake() => 
        SetTransform(LocalUnitTransformerComponent);
    private void Start()
    {
        State = new PlacementState();
        m_commandBuilder = new CommandBuilder();
        State.Enter(m_placementValidator, m_cellCalculator, UTransform);
    }

    private void OnEnable() => 
        m_cameraSystem.OnTarget += ChooseTarget;
    private void OnDisable() => 
        m_cameraSystem.OnTarget -= ChooseTarget;

    public void SwitchToLocal()
    {
        SwitchTransformer(UTType.LocalUnitTransformer);
        State.UTransform = UTransform;
    }
    public void SwitchToPhoton()
    {
        SwitchTransformer(UTType.PhotonUnitTransformer);
        State.UTransform = UTransform;
    }
    void ChooseTarget(SpaceMark target, PlacementSystem board)
    {
        if (m_isProcessing) return;
        m_isProcessing = true;
        State.Execute(this, target, board);
        m_isProcessing = false;
    }
    public void NextState() => State.Exit(this);
    void SwitchTransformer(UTType type)
    {
        switch (type) 
        { 
            case UTType.LocalUnitTransformer:
                SetTransform(LocalUnitTransformerComponent);
                break;
            case UTType.PhotonUnitTransformer: 
                SetTransform(PhotonUnitTransformerComponent);
                break;
        }
        ClearUnits();
    }

    public void CreateUnit(SpaceMark target, string unitName, Owner owner)
    {
        m_spaceMarks.Add(target);
        var unit = UTransform.CreateUnit(target, unitName);
        unit.GetComponent<UnitStats>().Ownership = owner;
        UnitLogic unitLogic = unit.GetComponent<UnitLogic>();
        UnitLogics.Add(unitLogic);
        unitLogic.CommandBuilder = m_commandBuilder;
        unitLogic.Build();
        if (owner == Owner.Enemy)
        {
            unit.GetComponent<UnitStatsBarUI>().RotateCanvas();
            unit.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
    }
    void SetTransform(MonoBehaviour transform)
    {
        if (transform != null && transform is IUnitTransformer transformer)
            UTransform = transformer;
        UTransform.SetValues(m_prefabs);
    }

    public bool IsUnitsAlive(Owner Ownership)
    {
        var units = GridRegistry.Instance.GetAllUnits(PlacementType.Battlefield);
        foreach (var unit in units)
        {
            if(unit.Item1.Ownership == Ownership && unit.Item1.IsDead == false)
                return true;
        }
        return false;
    }
    [Button("Toggle Units")]
    public void ToggleUnits()
    {
        foreach (UnitLogic UnitLogic in UnitLogics) UnitLogic.ToggleUI();
    }

    [Button("Clear Unit")]
    public void ClearUnits()
    {
    foreach(PlacementType Type in GridRegistry.Instance.GetAll().Keys)
        foreach(SpaceMark mark in GridRegistry.Instance.GetGrid(Type).Values)
            if (mark.Unit != null)
            {
                Destroy(mark.Unit);
                mark.Unit = null;
            }
    }
    [Button("Reset Units")]
    void ResetUnits() 
    {
        foreach (SpaceMark mark in m_spaceMarks) 
            mark.SetPosition();
    }
    [Button("Count Marks")]
    void CountMarks()
    {
        foreach (PlacementType Type in GridRegistry.Instance.GetAll().Keys)
            foreach (SpaceMark mark in GridRegistry.Instance.GetGrid(Type).Values)
                Debug.Log(mark.Dimension);
    }
    [Button("All units in marks")]
    void AllUnitsInMarks()
    {
        var units = GridRegistry.Instance.GetAllUnits(PlacementType.Battlefield);
        string result = $"Result - \n";
        foreach (var unit in units) 
        {
            result += $"Name of unit - {unit.Item1.Name}; Dimension - {unit.Item2.Dimension}\n";
        }
        Debug.Log(result);
    }
}
