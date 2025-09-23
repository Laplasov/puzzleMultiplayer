using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PreparedEnemiesSO", menuName = "Scriptable Objects/PreparedEnemiesSO")]
public class PreparedEnemiesSO : ScriptableObject
{
    [SerializeField]
    private List<EnemyGroup> groupOfEnemies = new List<EnemyGroup>();
    public List<EnemyGroup> GroupOfEnemies => groupOfEnemies;
}