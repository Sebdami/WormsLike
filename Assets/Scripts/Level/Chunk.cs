using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chunk : MonoBehaviour
{

    public GameObject worldGO;
    private World world;

    private List<Vector3> newVertices = new List<Vector3>();
    private List<int> newTriangles = new List<int>();
    private List<Vector2> newUV = new List<Vector2>();

    private float tUnitX = 1.0f/11.0f;
    private float tUnitY = 1.0f / 8.0f;
    private Vector2 tStone = new Vector2(2, 1);
    private Vector2 tGrass = new Vector2(1, 6);
    private Vector2 tDirt = new Vector2(1, 3);
    private Vector2 tGrassTop = new Vector2(1, 6);
    private Vector2 tWater = new Vector2(0, 1);

    private Mesh mesh;
    private MeshCollider col;

    private int faceCount;

    public int chunkSize = 16;

    public int chunkX;
    public int chunkY;
    public int chunkZ;
    public bool update;

    void LateUpdate()
    {
        if (update)
        {
            GenerateMesh();
            update = false;
        }
    }

    void Start()
    {

        world = worldGO.GetComponent("World") as World;
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        GenerateMesh();


    }

    public void GenerateMesh()
    {

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                for (int z = 0; z < chunkSize; z++)
                {
                    //This code will run for every block in the chunk
                    if (Block(x, y, z) != 0)
                    {
                        if (Block(x, y + 1, z) == 0)
                        {
                            //Block above is air
                            CubeTop(x, y, z, Block(x, y, z));
                        }

                        if (Block(x, y - 1, z) == 0)
                        {
                            //Block below is air
                            CubeBot(x, y, z, Block(x, y, z));

                        }

                        if (Block(x + 1, y, z) == 0)
                        {
                            //Block east is air
                            CubeEast(x, y, z, Block(x, y, z));

                        }

                        if (Block(x - 1, y, z) == 0)
                        {
                            //Block west is air
                            CubeWest(x, y, z, Block(x, y, z));

                        }

                        if (Block(x, y, z + 1) == 0)
                        {
                            //Block north is air
                            CubeNorth(x, y, z, Block(x, y, z));

                        }

                        if (Block(x, y, z - 1) == 0)
                        {
                            //Block south is air
                            CubeSouth(x, y, z, Block(x, y, z));

                        }

                    }

                }
            }
        }

        UpdateMesh();
    }

    byte Block(int x, int y, int z)
    {
        return world.Block(x + chunkX, y + chunkY, z + chunkZ);
    }

    Vector2 GetTexturePosFromBlock(byte block)
    {
        Vector2 texturePos = Vector2.zero;
        if (block == 1)
        {
            texturePos = tStone;
        }
        else if (block == 2)
        {
            texturePos = tDirt;
        }
        else if (block == 3)
        {
            texturePos = tGrass;
        }
        return texturePos;
    }

    void CubeTop(int x, int y, int z, byte block)
    {

        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x, y, z));

        Vector2 texturePos = GetTexturePosFromBlock(Block(x, y, z));

        Cube(texturePos);

    }

    void CubeNorth(int x, int y, int z, byte block)
    {

        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = GetTexturePosFromBlock(Block(x, y, z));

        Cube(texturePos);

    }

    void CubeEast(int x, int y, int z, byte block)
    {

        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y, z + 1));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));

        Vector2 texturePos = GetTexturePosFromBlock(Block(x, y, z));

        Cube(texturePos);

    }

    void CubeSouth(int x, int y, int z, byte block)
    {

        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x + 1, y, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));

        Vector2 texturePos = GetTexturePosFromBlock(Block(x, y, z));

        Cube(texturePos);

    }

    void CubeWest(int x, int y, int z, byte block)
    {

        newVertices.Add(new Vector3(x, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y, z + 1));
        newVertices.Add(new Vector3(x, y, z));
        newVertices.Add(new Vector3(x, y - 1, z));

        Vector2 texturePos = GetTexturePosFromBlock(Block(x, y, z));

        Cube(texturePos);

    }

    void CubeBot(int x, int y, int z, byte block)
    {

        newVertices.Add(new Vector3(x, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z));
        newVertices.Add(new Vector3(x + 1, y - 1, z + 1));
        newVertices.Add(new Vector3(x, y - 1, z + 1));

        Vector2 texturePos = GetTexturePosFromBlock(Block(x, y, z));

        Cube(texturePos);

    }

    void Cube(Vector2 texturePos)
    {

        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 1); //2
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4); //1
        newTriangles.Add(faceCount * 4 + 2); //3
        newTriangles.Add(faceCount * 4 + 3); //4

        newUV.Add(new Vector2(tUnitX * texturePos.x + tUnitX, tUnitY * texturePos.y));
        newUV.Add(new Vector2(tUnitX * texturePos.x + tUnitX, tUnitY * texturePos.y + tUnitY));
        newUV.Add(new Vector2(tUnitX * texturePos.x, tUnitY * texturePos.y + tUnitY));
        newUV.Add(new Vector2(tUnitX * texturePos.x, tUnitY * texturePos.y));

        faceCount++; // Add this line
    }

    void UpdateMesh()
    {

        mesh.Clear();
        mesh.vertices = newVertices.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();

        col.sharedMesh = null;
        col.sharedMesh = mesh;

        newVertices.Clear();
        newUV.Clear();
        newTriangles.Clear();
        faceCount = 0;

    }
}