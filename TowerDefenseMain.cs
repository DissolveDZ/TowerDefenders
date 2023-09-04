using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDefenseMain : MonoBehaviour
{
    public List<GameObject> Buildings = new List<GameObject>();
    public List<uint> Selected = new List<uint>();

    public uint AddBuilding(GameObject obj)
    {
        Buildings.Add(obj);
        return (uint)Buildings.Count-1;
    }
    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
