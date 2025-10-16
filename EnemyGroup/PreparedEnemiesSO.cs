using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PreparedEnemiesSO", menuName = "Scriptable Objects/PreparedEnemiesSO")]
public class PreparedEnemiesSO : ScriptableObject
{
    [SerializeField]
    private int time;
    [SerializeField]
    private List<EnemyGroup> groupOfEnemies = new List<EnemyGroup>();
    public List<EnemyGroup> GroupOfEnemies => groupOfEnemies;
    public int Time => time;
}