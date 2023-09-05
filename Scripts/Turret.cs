using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Turret : Tower
{
    public GameObject Guns;
    public GameObject RotationPole;
    public GameObject RadiusSphere;
    public GameObject target;
    public Vector3 desired_rot;
    public Vector3 gun_rotation;

    [SerializeField]
    [Range(0f, 40f)]
    private float radius;
    float look_lerp = 0f;
    enum State
    {
        IDLE,
        TARGETING
    }
    State cur_state = State.IDLE;

    private void Awake()
    {
        ID = Camera.main.GetComponent<TowerDefenseMain>().AddBuilding(gameObject);
        selected = false;
    }

    void Update()
    {
        if (Vector3.Distance(RotationPole.transform.position, target.transform.position) < radius*0.5)
            cur_state = State.TARGETING;
        else
            cur_state = State.IDLE;
        if (selected)
        {
            RadiusSphere.SetActive(true);
        }
        else
            RadiusSphere.SetActive(false);
        switch (cur_state)
        {
            default:
                look_lerp = Mathf.Lerp(look_lerp, 180f, 1f * Time.deltaTime);
                desired_rot = new Vector3(-90, 0, Mathf.Sin(Time.time * 0.5f) * look_lerp);
                gun_rotation = new Vector3(0, 0, 0);
                break;
            case State.TARGETING:
                Vector2 diff = new Vector2(RotationPole.transform.position.x - target.transform.position.x, RotationPole.transform.position.z - target.transform.position.z);
                float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                desired_rot = new Vector3(-90, 0, -angle);
                look_lerp = Mathf.Lerp(look_lerp, 0f, 1f * Time.deltaTime);
                break;
        }
        RadiusSphere.transform.localScale = new Vector3(radius, radius, radius);
        RotationPole.transform.rotation = Quaternion.Lerp(RotationPole.transform.rotation, Quaternion.Euler(desired_rot), 7.5f * Time.deltaTime);
        Guns.transform.rotation = RotationPole.transform.rotation * Quaternion.Euler(gun_rotation);
    }
}
