using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject Guns;
    public Vector3 desired_rot;
    public Vector3 gun_rotation;
    public GameObject target;

    float look_lerp = 0f;

    enum State
    {
        IDLE,
        TARGETING
    }
    State cur_state = State.IDLE;
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 5)
            cur_state = State.TARGETING;
        else
            cur_state = State.IDLE;
        switch (cur_state)
        {
            default:
                look_lerp = Mathf.Lerp(look_lerp, 180f, 1f * Time.deltaTime);
                desired_rot = new Vector3(-90, 0, Mathf.Sin(Time.time * 0.5f) * look_lerp);
                gun_rotation = new Vector3(0, 0, 0);
                break;
            case State.TARGETING:
                Vector2 diff = new Vector2(transform.position.x - target.transform.position.x, transform.position.z - target.transform.position.z);
                float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
                desired_rot = new Vector3(-90, 0, -angle);
                look_lerp = Mathf.Lerp(look_lerp, 0f, 1f * Time.deltaTime);
                break;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(desired_rot), 7.5f * Time.deltaTime);
        Guns.transform.rotation = transform.rotation * Quaternion.Euler(gun_rotation);
    }
}
