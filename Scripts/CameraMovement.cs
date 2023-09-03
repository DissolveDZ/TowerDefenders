using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // I always initiate to zero since i don't want to mess with unallocated memory but i don't know how unity handles this, should be fine if i don't
    public Camera cur_cam;
    public Vector3 offset = Vector3.zero;
    public Vector3 last_ray = Vector3.zero;
    public Vector3 cur_ray = Vector3.zero;
    Vector3 cam_desired = Vector3.zero;
    public float cam_lerp = 25f;
    public float zoom_dist = 1f;
    public float zoom_offset = 3f;
    public GameObject sphere;
    Vector3 mouse_pos = Vector3.zero;
    Vector2 last_mouse = Vector2.zero;
    Vector2 mouse_delta = Vector2.zero;
    Ray ray;
    RaycastHit ray_hit;

    void Start()
    {
        cur_cam = Camera.main;
        zoom_dist = 1f;
    }

    void Update()
    {       
        // bit shift layer mask
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        mouse_pos = Input.mousePosition;
        if (Input.GetMouseButton(2))
        {
            cam_desired -= new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * 0.05f * (zoom_dist * zoom_dist);
        }
        // Add mouse delta to the mouse position
        zoom_dist -= Input.mouseScrollDelta.y * zoom_dist * 0.1f;
        zoom_dist = Mathf.Clamp(zoom_dist, 0.5f, 100);
        cur_cam.transform.position = Vector3.Lerp(cur_cam.transform.position, cam_desired - cur_cam.transform.forward * (zoom_dist * zoom_dist) + cur_cam.transform.forward * zoom_offset, cam_lerp * Time.deltaTime);
        ray = Camera.main.ScreenPointToRay(mouse_pos);
        // Raycast
        if (Physics.Raycast(ray, out ray_hit, Mathf.Infinity))
        {
            sphere.transform.position = ray_hit.point;
            cur_ray = ray_hit.point;
        }
        // Calc mouse delta
        mouse_delta = new Vector2(mouse_pos.x, mouse_pos.y) - last_mouse;
        last_mouse = new Vector2(mouse_pos.x, mouse_pos.y);
        last_ray = cur_ray;
    }
}
