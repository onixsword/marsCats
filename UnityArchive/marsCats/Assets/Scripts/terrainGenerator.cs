using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class terrainGenerator : MonoBehaviour
{
    [Header("Export Values")]
    private terrainContainer[,] terrainPositions;
    private Vector3[] vertice;
    private Vector2[] uvs;
    private int[] triangles;
    
    [Header("Generation values")]
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private GameObject terrainContainerPrefab;
    [SerializeField] private Texture2D heightMap;
    [SerializeField] private Vector2 terrainSize;
    [SerializeField] private float maxHeight;

    [Header("navMesh")]
    [SerializeField] private LayerMask navMeshLayer;
    NavMeshData navData;
    NavMeshDataInstance navMeshDataInstance;


    // Start is called before the first frame update
    void Start()
    {
        terrainPositions = new terrainContainer[heightMap.width/(int)terrainSize.x, heightMap.height/(int)terrainSize.y];
        for (int z = 0; z < heightMap.height / (int)terrainSize.y; z++)
        {
            for (int x = 0; x < heightMap.width / (int)terrainSize.x; x++)
            {
                CreateShape(new Vector2(x, z));
            }
        }

        navMeshBuild(new Vector3(0f, -maxHeight, 0f), new Vector3(heightMap.width, maxHeight * 2, heightMap.height), navMeshLayer);
    }

    void CreateShape(Vector2 terrainPositionInArray)
    {
        vertice = new Vector3[((int)terrainSize.x + 1) * ((int)terrainSize.y + 1)];
        uvs = new Vector2[vertice.Length];

        for (int i = 0, z = 0; z <= (int)terrainSize.y; z++)
        {
            for(int x = 0; x <= (int)terrainSize.x; x++)
            {
                float y = heightMap.GetPixel((int)terrainPositionInArray.x * (int)terrainSize.x + x, (int)terrainPositionInArray.y * (int)terrainSize.x + z).r * maxHeight;
                uvs[i] = new Vector2((terrainPositionInArray.x * terrainSize.x + x) / (terrainSize.x*(heightMap.width / terrainSize.x)), (terrainPositionInArray.y * terrainSize.y + z) / (terrainSize.y * (heightMap.height / terrainSize.y)));
                vertice[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[(int)terrainSize.x * (int)terrainSize.y * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < (int)terrainSize.y; z++)
        {
            for(int x = 0; x < (int)terrainSize.x; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + (int)terrainSize.x + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + (int)terrainSize.x + 1;
                triangles[tris + 5] = vert + (int)terrainSize.x + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        terrainPositions[(int)terrainPositionInArray.x, (int)terrainPositionInArray.y] = GameObject.Instantiate(terrainContainerPrefab,
            new Vector3((int)terrainPositionInArray.x * (int)terrainSize.x - heightMap.width / 2, -maxHeight, (int)terrainPositionInArray.y * (int)terrainSize.y - heightMap.height / 2), 
            Quaternion.identity).GetComponent<terrainContainer>();
        terrainPositions[(int)terrainPositionInArray.x, (int)terrainPositionInArray.y].UpdateMesh(vertice, triangles, uvs);
        terrainPositions[(int)terrainPositionInArray.x, (int)terrainPositionInArray.y].transform.parent = transform;
    }

    void navMeshBuild(Vector3 navMeshPosition, Vector3 buildSize, LayerMask navMeshLayer)
    {
        navMeshDataInstance.Remove();

        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        NavMeshBuilder.CollectSources(new Bounds(navMeshPosition, buildSize), navMeshLayer, NavMeshCollectGeometry.RenderMeshes, 0, new List<NavMeshBuildMarkup>(), sources);
        navData = NavMeshBuilder.BuildNavMeshData(
            NavMesh.GetSettingsByIndex(1),
            sources,
            new Bounds(Vector3.zero, new Vector3(10000, 10000, 10000)),
            Vector3.down,
            Quaternion.Euler(Vector3.up));

        navMeshDataInstance = NavMesh.AddNavMeshData(navData);
    }

    private void OnDrawGizmos()
    {
    }
}
