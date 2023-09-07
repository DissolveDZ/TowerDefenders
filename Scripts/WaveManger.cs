using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Splines;

public class WaveManger : MonoBehaviour
{
    public GameObject enemy_prefab;
    public float spawn_time = 1f;
    public uint max_enemies = 10;
    private TowerDefenseMain tower_defense;
    private void Awake()
    {
        tower_defense = Camera.main.GetComponent<TowerDefenseMain>();
    }
    IEnumerator Start()
    {
        while (true)
        {
            tower_defense.AddEnemy(Instantiate(enemy_prefab, transform.position, Quaternion.identity, transform));
            print("add");
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        
    }
}
