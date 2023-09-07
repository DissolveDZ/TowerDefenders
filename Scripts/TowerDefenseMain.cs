using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class TowerDefenseMain : MonoBehaviour
{
    public SplineContainer path;
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> Buildings = new List<GameObject>();
    public List<int> Selected = new List<int>();

    public int AddBuilding(GameObject obj)
    {
        Buildings.Add(obj);
        return Buildings.Count-1;
    }
    public int AddEnemy(GameObject enemy)
    {
        Enemies.Add(enemy);
        Enemies[Enemies.Count-1].transform.GetComponent<SplineAnimate>().Container = path;
        return Enemies.Count-1;
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
