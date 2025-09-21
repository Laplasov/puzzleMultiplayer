using UnityEngine;

public enum Owner { Ally, Enemy, Neutral }
public interface IOwnership
{
    public Owner Ownership { get; set; }
}

