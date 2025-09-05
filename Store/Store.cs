using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Store : MonoBehaviour
{
    [SerializeField]
    public RectTransform[] SlotContents = new RectTransform[5];
    [SerializeField]
    GameObject _empty;
    [SerializeField]
    GameObject _character;

    enum SlotState { Empty, Unit }
    SlotState[] _slotStates = new SlotState[5];
    Stack<GameObject> _emptyStack = new();

    public event Action<GameObject> OnBuyUnit;


    bool IsEmptySlot(int index) => _slotStates[index] == SlotState.Empty;

    void Awake()
    {
        for (int i = 0; i < SlotContents.Length; i++)
        {
            var emptyObj = Instantiate(_empty, SlotContents[i].transform);
            _slotStates[i] = SlotState.Empty;
        }
    }

    void ClearSlotSilent(int index)
    {
        if (IsEmptySlot(index))
            return;
        SetEmptyInSlot(index);
    }

    void ClearSlot(int index)
    {
        if (IsEmptySlot(index))
            return;

        var child = SlotContents[index].transform.GetChild(0).GameObject();
        var prefab = child.GetComponent<CharacterStore>().Prefab;
        OnBuyUnit?.Invoke(prefab);
        SetEmptyInSlot(index);
    }

    void SetEmptyInSlot(int index)
    {
        var existingChild = SlotContents[index].transform.GetChild(0).GameObject();
        if (existingChild != null)
            DestroyImmediate(existingChild);

        GameObject emptySlot;

        if (_emptyStack.Count > 0)
            emptySlot = _emptyStack.Pop();
        else
            emptySlot = Instantiate(_empty);

        emptySlot.transform.SetParent(SlotContents[index].transform, false);
        emptySlot.SetActive(true);
        _slotStates[index] = SlotState.Empty;
    }

    public void CreateUnitSlot(GameObject prefab)
    {
        for (int i = 0; i < SlotContents.Length; i++)
        {
            if (IsEmptySlot(i))
            {
                var child = SlotContents[i].transform.GetChild(0).GameObject();

                _emptyStack.Push(child);
                child.transform.SetParent(transform, false);
                child.SetActive(false);

                var newUnit = Instantiate(_character, SlotContents[i].transform);
                newUnit.GetComponent<CharacterStore>()
                       .SetPrefab(prefab, i)
                       .AddListener(ClearSlot);

                _slotStates[i] = SlotState.Unit;
                return;
            }
        }
        Debug.Log("No empty slots available!");
    }

    [Button("ClearAllUnits")]
    public void ClearAllUnits()
    {
        for (int i = 0; i < SlotContents.Length; i++)
            ClearSlotSilent(i);
    }
}