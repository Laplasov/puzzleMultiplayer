using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class OnPlayerCollision : MonoBehaviour
{
    bool _isTargetable = true;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!enabled) return;
        EnemyWorld enemy = hit.gameObject.GetComponent<EnemyWorld>();
        if (enemy != null && _isTargetable)
        {
            StartCoroutine(WaitTarget());
            Debug.Log("Player collided with enemy: " + enemy.name);
            enemy.LoadBattle();
        }
    }
    IEnumerator WaitTarget()
    {
        _isTargetable = false;
        yield return new WaitForSeconds(3);
        _isTargetable = true;
    }
}
