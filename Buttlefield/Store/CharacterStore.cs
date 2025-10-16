using System;
using System.Reflection;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterStore : MonoBehaviour
{
    [SerializeField]
    TMP_Text _Name;
    [SerializeField]
    TMP_Text _ATK;
    [SerializeField]
    TMP_Text _HP;
    [SerializeField]
    TMP_Text _SP;
    [SerializeField]
    TMP_Text _INIT;

    [HideInInspector]
    public GameObject Prefab;
    [HideInInspector]
    public UnityEvent<int> OnReturnToStore;
    [HideInInspector]
    public Func<SpaceMark> OnGetFreeMark;
    int _index;

    public UnityEvent<int> SetPrefab(GameObject prefab, int index)
    {
        Prefab = prefab;
        _index = index;
        var baseStats = prefab.GetComponent<UnitStats>();

        GetComponent<Button>().onClick
            .AddListener(() => {
                if (OnGetFreeMark != null && OnGetFreeMark?.Invoke() == null) return;
                OnReturnToStore?.Invoke(_index);
                OnReturnToStore.RemoveAllListeners();
            });

        _Name.text = baseStats.Name;
        _ATK.text = baseStats.ATK.ToString();
        _HP.text = baseStats.HP.ToString();
        _SP.text = baseStats.SP.ToString();
        _INIT.text = baseStats.SPD.ToString();

        return OnReturnToStore;
    }

}
