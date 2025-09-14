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

    List<SpaceMark> m_spaceMark = new ();
    SpaceMark m_currentUnit = null;
    PlacementSystem m_currentBoard;
    bool m_isProcessing = false;
    Vector2Int SingeSize = new Vector2Int(1,1);
    PlacementValidator m_placementValidator = new PlacementValidator();
    public enum UTType { LocalUnitTransformer, PhotonUnitTransformer }
    IUnitTransformer UTransform;

    private void Awake() => 
        SetTransform(LocalUnitTransformerComponent);

    private void OnEnable() => 
        m_cameraSystem.OnTarget += ChooseTarget;
    private void OnDisable() => 
        m_cameraSystem.OnTarget -= ChooseTarget;

    public void SwitchToLocal() => 
        SwitchTransformer(UTType.LocalUnitTransformer);
    public void SwitchToPhoton() => 
        SwitchTransformer(UTType.PhotonUnitTransformer);

    public void CreateUnit(SpaceMark target, string unitName)
    {
        m_spaceMark.Add(target);
        UTransform.CreateUnit(target, unitName);
    }

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
    void SetTransform(MonoBehaviour transform)
    {
        if (transform != null && transform is IUnitTransformer transformer)
            UTransform = transformer;
        UTransform.SetValues(m_prefabs);
    }

    void ChooseTarget(SpaceMark target, PlacementSystem board)
    {
        if (m_isProcessing) return;
        m_isProcessing = true;

        if (m_currentUnit != null)
        {
            bool shouldMove, shouldSwap;
            bool canPerformAction = m_placementValidator.CanPerformAction(m_currentUnit, target, board, out shouldMove, out shouldSwap);

            if (!canPerformAction)
            {
                m_isProcessing = false;
                return;
            }

            if (shouldMove)
            {
                UTransform.MoveUnit(m_currentUnit, target, board, m_currentBoard);
            }
            else if (shouldSwap)
            {
                UTransform.SwapUnits(m_currentUnit, target, board, m_currentBoard);
            }
            else
            {
                m_isProcessing = false;
                return;
            }

            m_currentUnit = null;
            m_cellCalculator.Rule = PlacementRule.Full;
            m_cellCalculator.UnitSize = SingeSize;
        }
        else if (target.Unit != null)
        {
            SpaceMark unitToSelect = m_placementValidator.GetUnitToSelect(target);

            IPlacementRule UnitRule = unitToSelect.Unit.GetComponent<IPlacementRule>();
            m_cellCalculator.Rule = UnitRule.GetPlacementRule();
            m_cellCalculator.UnitSize = UnitRule.GetSize();

            m_currentUnit = unitToSelect;
            m_currentBoard = board;
            UTransform.SelectUnit(m_currentUnit, board);
        }
        m_isProcessing = false;
    }

    [Button("Reset Units")]
    void ResetUnits() 
    {
        foreach (SpaceMark mark in m_spaceMark) 
            mark.SetPosition();
    }
    [Button("Clear Unit")]
    public void ClearUnits()
    {
    foreach(PlacementType Type in GridRegistry.GetAll().Keys)
        foreach(SpaceMark mark in GridRegistry.GetGrid(Type).Values)
            if (mark.Unit != null)
            {
                Destroy(mark.Unit);
                mark.Unit = null;
            }
    }
    [Button("Count Marks")]
    void CountMarks()
    {
        foreach (PlacementType Type in GridRegistry.GetAll().Keys)
            foreach (SpaceMark mark in GridRegistry.GetGrid(Type).Values)
                Debug.Log(mark.Dimension);
    }

}
