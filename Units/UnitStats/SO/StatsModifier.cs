using UnityEngine;

[CreateAssetMenu(fileName = "StatsModifier", menuName = "Scriptable Objects/StatsModifier")]
public class StatsModifier : ScriptableObject
{
    public StatsModifier(int hp, int atk)
    {
        _HP = hp;
        _ATK = atk;
    }
    [SerializeField]
    private int _HP;
    [SerializeField]
    private int _ATK;
    public int HP => _HP;
    public int ATK => _ATK;
}