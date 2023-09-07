using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.HighDefinition;
using static UnityEngine.ParticleSystem;

public class Turret : Tower
{
    public GameObject Guns;
    public GameObject RotationPole;
    public GameObject RadiusSphere;
    public ParticleSystem Particles;
    public ParticleSystem Bullet;
    public GameObject target;
    public Vector3 desired_rot;
    public Vector3 gun_rotation;
    public List<ParticleSystem> particles = new List<ParticleSystem>();
    private bool shooting = false;
    public float damage = 10f;
    public float firing_speed = 0.25f;
    [SerializeField]
    int enemy_layer;
    IEnumerator couroutine;

    [SerializeField]
    [Range(0f, 100f)]
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
        enemy_layer = 1 << LayerMask.NameToLayer("Enemy");
        selected = false;
        type = Type.OFFENSIVE;
    }


    void AddParticle(Vector3 temp_pos, Quaternion temp_rot)
    {
        particles.Add(Instantiate(Particles, temp_pos, temp_rot, transform));
        particles[particles.Count - 1].Play();
        particles.Add(Instantiate(Bullet, temp_pos, temp_rot, transform));
        particles[particles.Count - 1].Play();
    }
    private IEnumerator Attack(float speed)
    {
        yield return new WaitForSeconds(speed);
        shooting = false;
        Vector3 temp_pos = new Vector3();
        Quaternion temp_rot = new Quaternion();
        temp_pos = Guns.transform.position - Guns.transform.right;
        temp_rot = Quaternion.Euler(new Vector3(Guns.transform.rotation.eulerAngles.x - 90, Guns.transform.rotation.eulerAngles.y + 90, Guns.transform.rotation.eulerAngles.z));
        bool add_particle = true;

        for (int i = 0; i < particles.Count; i++)
        {
            if (!particles[i].isPlaying && !particles[i + 1].isPlaying)
            {
                add_particle = false;
                particles[i].transform.position = temp_pos;
                particles[i].transform.rotation = temp_rot;
                particles[i].time = 0;
                particles[i].Play();
                particles[i + 1].transform.position = temp_pos;
                particles[i + 1].transform.rotation = temp_rot;
                particles[i + 1].time = 0;
                particles[i + 1].Play();
                break;
            }
        }

        if (add_particle)
        {
            AddParticle(temp_pos, temp_rot);
        }
        // perform an actual raycast since the tower can attack multiple enemies at once
        RaycastHit ray_hit;
        if (Physics.Raycast(Guns.transform.position, (target.transform.position - Guns.transform.position).normalized, out ray_hit, Mathf.Infinity, enemy_layer))
        {
            if (ray_hit.transform.parent.gameObject.TryGetComponent<Enemy>(out Enemy temp))
            {
                temp.health -= damage;
            }
        }

        Debug.DrawRay(Guns.transform.position, (target.transform.position - Guns.transform.position).normalized * 1000, Color.red, 0.1f);
        //Debug.Log("Shoot!!");
    }

    void Update()
    {
        Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + 10, transform.position.z), Color.red);
        if (Vector3.Distance(RotationPole.transform.position, target.transform.position) < radius * 0.5)
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
                if (!shooting)
                {
                    couroutine = Attack(firing_speed);
                    shooting = true;
                    StartCoroutine(couroutine);
                }
                break;
        }
        if (cur_state != State.TARGETING && shooting)
        {
            shooting = false;
            StopCoroutine(couroutine);
        }
        RadiusSphere.transform.localScale = new Vector3(radius, radius, radius);
        RotationPole.transform.rotation = Quaternion.Lerp(RotationPole.transform.rotation, Quaternion.Euler(desired_rot), 7.5f * Time.deltaTime);
        Guns.transform.rotation = RotationPole.transform.rotation * Quaternion.Euler(gun_rotation);
    }
}