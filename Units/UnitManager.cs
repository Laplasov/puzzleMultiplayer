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
    private GameObject m_unitA;
    [SerializeField]
    private GameObject m_unitB;

    List<SpaceMark> m_spaceMark = new List<SpaceMark>();

    SpaceMark m_currentUnit = null;
    bool m_currentUnitInstantiate;
    bool m_isProcessing = false;

    IUnitTransformer UTransform; // = new LocalUnitTransformer();

    [SerializeField]
    private MonoBehaviour unitTransformerComponent;
    private void OnEnable() => m_cameraSystem.OnTarget += ChooseTarget;

    private void OnDisable() => m_cameraSystem.OnTarget -= ChooseTarget;

    private void Awake()
    {
        /*
        var Transformer = new GameObject("PhotonUnitTransformer").AddComponent<PhotonUnitTransformer>();
        Transformer.SetValues(m_unitA, m_unitB);
        UTransform = Transformer;
        */

        if (unitTransformerComponent != null && unitTransformerComponent is IUnitTransformer transformer)
            UTransform = transformer;

        UTransform.SetValues(m_unitA, m_unitB);
    }

    void ChooseTarget(SpaceMark target, bool canInstantiate)
    {
        if (m_isProcessing) return;
        m_isProcessing = true;

        if (m_currentUnit != null)
        {
            if (target.Unit == null)
                UTransform.MoveUnit(m_currentUnit, target, canInstantiate);
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
            UTransform.SelectUnit(m_currentUnit);
        }
        m_isProcessing = false;
    }


    [Button("Reset Units")]
    void ResetUnits() 
    {
        foreach (SpaceMark mark in m_spaceMark) 
            mark.SetPosition();
    }

    [Button("Mirror Unit")]
    void MirrorUnit()
    {
        UTransform.MoveUnit(m_currentUnit, m_currentUnit.MirroredMark, false);
    }

}
