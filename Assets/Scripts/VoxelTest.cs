using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTest : MonoBehaviour {
    GameObject mapObject;
    VoxelMap map;

    private void Start()
    {
        map = new VoxelMap(20, 20, 25, 0.25f);
        map.GenerateFilledTerrain();
        mapObject = map.IntantiateMap();
    }


}
