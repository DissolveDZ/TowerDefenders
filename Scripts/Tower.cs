using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public bool selected = false;
    public int ID = 0;
    public enum Type
    {
        NONE,
        ECO,
        DEFENSIVE,
        OFFENSIVE
    } Type type = Type.NONE;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
