using UnityEngine;
using System.Collections;
using System;
using Object = UnityEngine.Object;

public class World : MonoBehaviour
{
    public GameObject chunk;
    public Chunk[,,] chunks;  //Changed from public GameObject[,,] chunks;
    public int chunkSizexy = 16;
    public int chunkSizez = 6;
    public byte[,,] data;
    public int worldX = 16;
    public int worldY = 16;
    public int worldZ = 16;

    public AnimationCurve sideViewHeightModifier = new AnimationCurve(new Keyframe(0.0f, 0.0f, 0.5f, 0.5f), new Keyframe(0.5f, 1.0f, 0f, 0f), new Keyframe(1.0f, 0.0f, -0.5f, -0.5f));
    public float heightMultiplier = 2.0f;
    public AnimationCurve depthHeightModifier = new AnimationCurve(new Keyframe(0.0f, 0.0f, 1.0f, 1.0f), new Keyframe(0.5f, 1.0f, 0f, 0f), new Keyframe(1.0f, 0.0f, -1f, -1f));
    public float slope = 3.0f;

    void Start ()
	{
		data = new byte[worldX, worldY, worldZ];
   
		for (int x=0; x<worldX; x++) {
			for (int z=0; z<worldZ; z++) {
				int stone = PerlinNoise (x, 0, z, 10, 3, 1.2f);
				stone += PerlinNoise (x, 300, z, 18, 4, 0) + 7;
				int dirt = PerlinNoise (x, 200, z, 25, 4, 0) + 1;
                int grass = PerlinNoise(x, 100, z, 48, 3, 0) + 1;
                for (int y=0; y<worldY; y++) {
                    float toEvaluate = y / sideViewHeightModifier.Evaluate((float)x / (float)worldX) / heightMultiplier + slope / depthHeightModifier.Evaluate((float)z / (float)worldZ);

                    if (toEvaluate <= stone) {
						data [x, y, z] = 1;
					} else if (toEvaluate <= dirt + stone) {
						data [x, y, z] = 2;
					}
                    else if(toEvaluate <= dirt + grass + stone)
                        data[x, y, z] = 3;

                }
			}
		}
   
   
		chunks = new Chunk[Mathf.FloorToInt (worldX / chunkSizexy), Mathf.FloorToInt (worldY / chunkSizexy), Mathf.FloorToInt (worldZ / chunkSizez)];
		
   
	}
	
    public Vector3 TransformWorldPosToLocal(Vector3 worldPos)
    {
        return transform.worldToLocalMatrix.MultiplyPoint3x4(worldPos);
    }

	public void GenColumn(int x, int z){
		for (int y=0; y<chunks.GetLength(1); y++) {
      
			//Create a temporary Gameobject for the new chunk instead of using chunks[x,y,z]
			GameObject newChunk = Instantiate (chunk, new Vector3 ((x * chunkSizexy - 0.5f) * transform.localScale.x,  (y * chunkSizexy + 0.5f) * transform.localScale.y, (z * chunkSizez - 0.5f) * transform.localScale.z), new Quaternion (0, 0, 0, 0)) as GameObject;
            newChunk.transform.SetParent(transform);
            newChunk.transform.localScale = Vector3.one;
			chunks [x, y, z] = newChunk.GetComponent<Chunk>();
			chunks [x, y, z].worldGO = gameObject;
			chunks [x, y, z].chunkSize = chunkSizexy;
			chunks [x, y, z].chunkX = x * chunkSizexy;
			chunks [x, y, z].chunkY = y * chunkSizexy;
			chunks [x, y, z].chunkZ = z * chunkSizez;
      
				
		}
	}
  
	int PerlinNoise (int x, int y, int z, float scale, float height, float power)
	{
        x /= 2;
        return (int)(Mathf.Pow((Mathf.PerlinNoise(x / scale, y / scale) * (height)), (2))); //+z / 40.0f

    }
    byte tmp;

    public bool isInBounds(int x, int y, int z)
    {
#pragma warning disable CS0168 // La variable e est déclarée mais jamais utilisée
        try
        {
            tmp = data[x, y, z];
        }
        catch (Exception e)
        {
            return false;
        }
        return true;
#pragma warning restore CS0168 // La variable est déclarée mais jamais utilisée
    }

  
	public byte Block (int x, int y, int z)
	{
   
		if (x >= worldX || x < 0 || y >= worldY || y < 0 || z >= worldZ || z < 0) {
			return (byte)0;
		}
   
		return data [x, y, z];
	}
}