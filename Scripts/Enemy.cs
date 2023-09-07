using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            GetComponent<SplineAnimate>().ElapsedTime = 0;
            health = 100f;
        }
    }
}
