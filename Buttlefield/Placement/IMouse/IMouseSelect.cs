using UnityEngine;

public interface IMouseSelect 
{
    SpaceMark OnMouseSelect(Vector3 mouseWorldPosition, out PlacementSystem CanInstantiate);
    //SpaceMark OnMouseSelect(Vector3 mouseWorldPos);
}
