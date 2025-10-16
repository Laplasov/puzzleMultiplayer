using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarkStatus : MonoBehaviour
{
    [SerializeField]
    TMP_Text _NAME;
    [SerializeField]
    TMP_Text _ATK;
    [SerializeField]
    TMP_Text _HP;
    [SerializeField]
    TMP_Text _SP;
    [SerializeField]
    TMP_Text _INIT;

    SpaceMark m_currentSpaceMark;
    public void SetShowStats(Vector3 cellCenter, Dictionary<Vector3, SpaceMark> GridMarks)
    {
        SpaceMark value;
        if (GridMarks.TryGetValue(cellCenter, out value) && value.Unit != null)
        {
            if (m_currentSpaceMark == value) return;
            m_currentSpaceMark = value;
            gameObject.SetActive(true);
            SetStats(value.Unit.GetComponent<UnitStats>());
        }
        else
        {
            if (m_currentSpaceMark == null) return;
            m_currentSpaceMark = null;
            gameObject.SetActive(false);
        }
    }

    void SetStats(UnitStats unit)
    {
        _NAME.text = unit.Name;
        _ATK.text = unit.ATK.ToString();
        _HP.text = unit.HP.ToString();
        _SP.text = unit.SP.ToString();
        _INIT.text = unit.SPD.ToString();
    }

}
