using UnityEngine;

[System.Serializable]
public class JSONStatsModifier
{
    public JSONStatsModifier(StatsModifier statsModifier, string guid)
    {
        StatsModifier = statsModifier;
        GUID = guid;
    }
    public string GUID { get; private set; }
    public StatsModifier StatsModifier { get; set; }
}
