using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMap {

    int width;
    int height;
    int depth;
    float cubeSize;

    Mesh mapMesh;
    public List<Vector3> meshVertices = new List<Vector3>();
    public List<int> meshTriangles = new List<int>();
    public List<Vector2> meshUV = new List<Vector2>();

    public int[][][] VoxelTerrain;

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

    public int Depth
    {
        get
        {
            return depth;
        }

        set
        {
            depth = value;
        }
    }

    public float CubeSize
    {
        get
        {
            return cubeSize;
        }

        set
        {
            cubeSize = value;
        }
    }

    public Mesh MapMesh
    {
        get
        {
            return mapMesh;
        }

        set
        {
            mapMesh = value;
        }
    }

    public VoxelMap(int _width, int _height, int _depth, float _cubeSize)
    {
        width = _width;
        height = _height;
        depth = _depth;
        cubeSize = _cubeSize;
        VoxelTerrain = new int[width][][];
        for(int x = 0; x < width; x++)
        {
            VoxelTerrain[x] = new int[height][];
            for(int y = 0; y < height; y++)
            {
                VoxelTerrain[x][y] = new int[depth];
            }
        }
    }

    public void GenerateFilledTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    VoxelTerrain[x][y][z] = 1;
                }
            }
        }
    }

    Mesh CreateMesh()
    {
        if (mapMesh)
        {
            GameObject.Destroy(mapMesh);
        }
        mapMesh = new Mesh();
        meshVertices = new List<Vector3>();
        meshTriangles = new List<int>();
        meshUV = new List<Vector2>();



        return mapMesh;
    }

    void GetCubeVertices(bool )
    {

    }


    Mesh UpdateMesh()
    {
        if (!mapMesh)
        {
            Debug.LogError("Mesh needs to be created beforehand with CreateMesh()");
            return null;
        }

        List<Vector3> vertices = new List<Vector3>();
        mapMesh.GetVertices(vertices);

        

        return mapMesh;
    }

    public GameObject IntantiateMap()
    {
        GameObject map = new GameObject("VoxelMap");

    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            for (int z = 0; z < depth; z++)
            {
                    GameObject prefab = BlockDatabase.GetBlockPrefabByIndex(VoxelTerrain[x][y][z]);
                    if (prefab == null)
                        continue;

                    GameObject go = GameObject.Instantiate(prefab, map.transform);
                    go.transform.position = new Vector3(x * cubeSize, y * cubeSize, z * cubeSize);
                    go.transform.localScale = Vector3.one * cubeSize;
            }
        }
    }

    return map;
    }
}

