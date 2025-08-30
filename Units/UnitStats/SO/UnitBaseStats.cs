using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "UnitBaseStats", menuName = "Scriptable Objects/UnitBaseStats")]
public class UnitBaseStats : ScriptableObject
{
    [SerializeField]
    private string _Name;
    [SerializeField]
    private int _HP;
    [SerializeField]
    private int _ATK;
    public string Name => _Name;
    public int HP => _HP;
    public int ATK => _ATK;
}
