using Assets.Scripts.Units;
using NUnit.Framework;
using Photon.Pun;
using System.Collections.Generic;
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
    private string m_unitA;
    [SerializeField]
    private string m_unitB;
    [SerializeField]
    private UnitPrefabsSO m_prefabs;

    List<SpaceMark> m_spaceMark = new ();
    SpaceMark m_currentUnit = null;
    bool m_currentUnitInstantiate;
    bool m_isProcessing = false;

    public enum UTType { LocalUnitTransformer, PhotonUnitTransformer }
    IUnitTransformer UTransform;

    private void Awake() => 
        SetTransform(LocalUnitTransformerComponent);
    private void OnEnable() => 
        m_cameraSystem.OnTarget += ChooseTarget;
    private void OnDisable() => 
        m_cameraSystem.OnTarget -= ChooseTarget;

    public void SwitchToLocal() => SwitchTransformer(UTType.LocalUnitTransformer);
    public void SwitchToPhoton() => SwitchTransformer(UTType.PhotonUnitTransformer);
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
        //UTransform.SetValues(m_unitA, m_unitB);
        
        var unit1 = m_prefabs.GetPrefabByName(m_unitA);
        var unit2 = m_prefabs.GetPrefabByName(m_unitB);
        Debug.Log($"Settled prefabs {m_unitA} and {m_unitB}");

        UTransform.SetValues(unit1, unit2);
        
    }

    void ChooseTarget(SpaceMark target, bool canInstantiate)
    {
        if (m_isProcessing) return;
        m_isProcessing = true;

        if (m_currentUnit != null)
        {
            if (target.Unit == null)
                UTransform.MoveUnit(m_currentUnit, target, canInstantiate, m_currentUnitInstantiate);
            else 
                UTransform.SwapUnits(m_currentUnit, target, canInstantiate, m_currentUnitInstantiate);

            m_currentUnit = null;
        }
        else
        if (target.Unit == null)
        {
            if (canInstantiate)
            {
                m_spaceMark.Add(target);
                UTransform.CreateUnit(target);
            }
        }
        else
        {
            m_currentUnit = target;
            m_currentUnitInstantiate = canInstantiate;
            UTransform.SelectUnit(m_currentUnit, canInstantiate);
        }
        m_isProcessing = false;
    }
    [Button("Reset Units")]
    void ResetUnits() 
    {
        foreach (SpaceMark mark in m_spaceMark) 
            mark.SetPosition();
    }
    /*
    [Button("Mirror Unit")]
    void MirrorUnit() => UTransform.MoveUnit(m_currentUnit, m_currentUnit.MirroredMark, false);
    */
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
