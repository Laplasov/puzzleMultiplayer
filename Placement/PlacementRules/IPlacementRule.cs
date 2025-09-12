using UnityEngine;

public interface IPlacementRule
{
    PlacementRule GetPlacementRule();
    Vector2Int GetSize();

}

