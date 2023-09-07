using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cur_cam;
    public Vector3 offset = Vector3.zero;
    public Vector3 last_ray = Vector3.zero;
    public Vector3 cur_ray = Vector3.zero;
    Vector3 cam_desired = Vector3.zero;
    [SerializeField]
    [Range(0f, 100f)]
    private float cam_lerp = 25f;
    [SerializeField]
    private float zoom_dist = 1f;
    [SerializeField]
    private float zoom_offset = 3f;
    public GameObject sphere;
    Vector3 mouse_pos = Vector3.zero;
    Ray ray;
    RaycastHit ray_hit;
    LayerMask building_layer;

    void Awake()
    {
        cur_cam = Camera.main;
        zoom_dist = 1f;
        building_layer = LayerMask.NameToLayer("Building");
    }

    private void Select()
    {
        List<int> selected_ptr = Camera.main.GetComponent<TowerDefenseMain>().Selected;
        List<GameObject> buildings_ptr = Camera.main.GetComponent<TowerDefenseMain>().Buildings;
        GameObject cur_building;
        if (ray_hit.transform.parent == null)
        {
            selected_ptr.Clear();
        }
        else
        {
            cur_building = ray_hit.transform.parent.gameObject;
            if (cur_building.layer == building_layer)
            {
                if (!cur_building.GetComponent<Tower>().selected)
                {
                    bool is_selected = false;
                    for (int i = 0; i < selected_ptr.Count; i++)
                    {
                        if (selected_ptr[i] == cur_building.GetComponent<Tower>().ID)
                        {
                            selected_ptr.RemoveAt(i);
                            is_selected = true;
                        }
                    }
                    if (!is_selected)
                        selected_ptr.Add(cur_building.GetComponent<Tower>().ID);
                }
            }
            else
            {
                selected_ptr.Clear();
            }
        }
        for (int i = 0; i < buildings_ptr.Count; i++)
            {
                buildings_ptr[i].GetComponent<Tower>().selected = false;
            }
            for (int i = 0; i < selected_ptr.Count; i++)
            {
                buildings_ptr[selected_ptr[i]].GetComponent<Tower>().selected = true;
            }
    }

    void Update()
    {
        mouse_pos = Input.mousePosition;
        // Add mouse delta to the mouse position
        if (Input.GetMouseButton(2))
        {
            cam_desired -= new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y")) * 0.05f * (zoom_dist * zoom_dist);
        }
        zoom_dist -= Input.mouseScrollDelta.y * zoom_dist * 0.1f;
        zoom_dist = Mathf.Clamp(zoom_dist, 0.5f, 100);
        cur_cam.transform.position = Vector3.Lerp(cur_cam.transform.position, cam_desired - cur_cam.transform.forward * (zoom_dist * zoom_dist) - cur_cam.transform.forward * zoom_offset, cam_lerp * Time.deltaTime);
        ray = Camera.main.ScreenPointToRay(mouse_pos);
        // Raycast for 3D "cursor" and selecting
        if (Physics.Raycast(ray, out ray_hit, Mathf.Infinity))
        {
            sphere.transform.position = ray_hit.point;
            cur_ray = ray_hit.point;
            if (Input.GetMouseButtonDown(0))
                Select();
        }
    }
}
