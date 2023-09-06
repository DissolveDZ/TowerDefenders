using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseMain : MonoBehaviour
{
    public List<GameObject> Buildings = new List<GameObject>();
    public List<int> Selected = new List<int>();

    public int AddBuilding(GameObject obj)
    {
        Buildings.Add(obj);
        return Buildings.Count-1;
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
