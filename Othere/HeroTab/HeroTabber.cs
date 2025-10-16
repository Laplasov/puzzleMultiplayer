using UnityEngine;
using System;
using System.Collections.Generic;

public class HeroTabber : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_placeHolders;
    [SerializeField] private GameObject m_prefab;

    void Start()
    {

        foreach (GameObject go in m_placeHolders) 
        {
            var hero = Instantiate(m_prefab, go.transform);
            hero.transform.position = go.transform.position;
        }
    }
}
