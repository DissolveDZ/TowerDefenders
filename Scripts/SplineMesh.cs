using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class SplineMesh : MonoBehaviour
{
    [SerializeField]
    private SplineContainer spline_container;

    [SerializeField]
    private int spline_index;

    [SerializeField]
    [Range(0f, 10f)]
    private float width;

    [SerializeField]
    private float steps;

    [SerializeField]
    private float height;

    public MeshFilter mesh_filter;

    List<Vector3> positions_left;
    List<Vector3> positions_right;
    float3 position;
    float3 tangent;
    float3 up_vector;

    private void Awake()
    {
        GetVerts();
        BuildMesh();
    }
    private void OnEnable()
    {
        Spline.Changed += OnSplineChanged;
        GetVerts();
    }
    private void OnDisable()
    {
        Spline.Changed -= OnSplineChanged;
    }
    private void OnSplineChanged(Spline arg1, int arg2, SplineModification arg3)
    {
        GetVerts();
        BuildMesh();
    }
    private void GetVerts()
    {
        positions_left = new List<Vector3>();
        positions_right = new List<Vector3>();

        float step = 1f / (float)steps;
        for (int i = 0; i < steps; i++)
        {
            float ID = step * i;
            // using tangent as the forward direction
            spline_container.Evaluate(0, ID, out position, out tangent, out up_vector);
            float3 right = Vector3.Cross(tangent, up_vector).normalized;
            positions_left.Add(position + -right * width);
            positions_right.Add(position + right * width);
        }
    }
    // generate mesh by creating triangles and uvs
    private void BuildMesh()
    {
        int length = positions_right.Count;

        Vector3[] verts = new Vector3[length * 2];
        Vector2[] uvs = new Vector2[verts.Length];
        int numTris = 2 * (length - 1);
        int[] tris = new int[numTris * 3];
        int vertIndex = 0;
        int triIndex = 0;

        for (int i = 0; i < length; i++)
        {
            verts[vertIndex] = positions_right[i];
            verts[vertIndex + 1] = positions_left[i];

            float percent = i / (float)(length - 1);
            float v = 1 - Mathf.Abs(2 * percent - 1);
            uvs[vertIndex] = new Vector2(0, v);
            uvs[vertIndex + 1] = new Vector2(1, v);

            if (i < length - 1)
            {
                tris[triIndex] = vertIndex;
                tris[triIndex + 1] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 2] = vertIndex + 1;

                tris[triIndex + 3] = vertIndex + 1;
                tris[triIndex + 4] = (vertIndex + 2) % verts.Length;
                tris[triIndex + 5] = (vertIndex + 3) % verts.Length;
            }

            vertIndex += 2;
            triIndex += 6;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.name = "road mesh";
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh_filter.mesh = mesh;
    }

    private void OnDrawGizmos()
    {
        Handles.matrix = transform.localToWorldMatrix;
        for (int i = 0; i < positions_left.Count; i++)
        {
            Handles.SphereHandleCap(0, positions_left[i], Quaternion.identity, 0.25f, EventType.Repaint);
            Handles.SphereHandleCap(0, positions_right[i], Quaternion.identity, 0.25f, EventType.Repaint);
        }
    }
}
