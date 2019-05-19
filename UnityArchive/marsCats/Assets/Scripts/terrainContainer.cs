using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]

public class terrainContainer : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider meshCollider;

    private Vector3[] vertice;
    private int[] triangles;   

    // Start is called before the first frame update
    void Awake()
    {
        mesh = new Mesh();
        mesh.name = "Dinamic terrain";
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        gameObject.layer = 14;
    }

    public void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertice;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    public void UpdateMesh(Vector3[] Vertice, int[] Triangles, Vector2[] uvs)
    {
        vertice = Vertice;
        triangles = Triangles;

        mesh.Clear();

        mesh.vertices = vertice;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }

    public Vector3[] Vertice { get => vertice; set => vertice = value; }
    public int[] Triangles { get => triangles; set => triangles = value; }


    /*private void OnDrawGizmos()
    {
        if (vertice == null)
            return;

        for (int i = 0; i < vertice.Length; i++)
        {
            Gizmos.DrawSphere(vertice[i], 0.1f);
        }
    }*/
}
