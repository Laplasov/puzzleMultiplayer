using System;
using TMPro;
using UnityEngine;

[System.Serializable]
public class LoadSlot : MonoBehaviour
{
    [SerializeField]
    public TMP_Text FileName;
    [SerializeField]
    public TMP_Text LastSave;
    [SerializeField]
    public TMP_Text TotalTimePlay;
    [SerializeField]
    public int Index;
}
