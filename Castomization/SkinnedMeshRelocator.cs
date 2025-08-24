using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class SkinnedMeshRelocator : MonoBehaviour
{

    [SerializeField] SkinnedMeshRenderer[] Prefab;
    [SerializeField] SkinnedMeshRenderer OriginalSkinnedMesh;
    [SerializeField] Transform Root;
    SkinnedMeshRenderer spawnMesh;
    int m_index = 0;

    void Start()
    {
        spawnMesh = Instantiate(Prefab[m_index], transform);
        spawnMesh.bones = OriginalSkinnedMesh.bones;
        spawnMesh.rootBone = Root;
    }

    public void OnClothChange()
    {
        Debug.Log("Cloth Changed!");

        Destroy(spawnMesh.gameObject);

        if (m_index == Prefab.Length - 1)
            m_index = 0;
        else 
            m_index++;

        spawnMesh = Instantiate(Prefab[m_index], transform);
        spawnMesh.bones = OriginalSkinnedMesh.bones;
        spawnMesh.rootBone = Root;
    }
}
