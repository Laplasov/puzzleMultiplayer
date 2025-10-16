using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "SceneDataBridge")]
public class SceneDataBridgeSO : ScriptableObject
{
    public PreparedEnemiesSO[] PreparedEnemiesSO { set; get; }

}