using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionOffset : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    void Update()
    {
        transform.position = transform.position + offset;
    }
}
