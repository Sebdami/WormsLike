using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockData
{
    [SerializeField]
    public int blockId;
    [SerializeField]
    public GameObject blockPrefab;

    public int BlockId
    {
        get
        {
            return blockId;
        }
    }

    public GameObject BlockPrefab
    {
        get
        {
            return blockPrefab;
        }
    }
}

public class BlockDatabase : MonoBehaviour{
    [SerializeField]
    GameObject[] prefabs;

    [SerializeField]
    static BlockData[] Blocks;

    private void Awake()
    {
        Blocks = new BlockData[2];
        Blocks[1] = new BlockData();
        Blocks[1].blockId = 1;
        Blocks[1].blockPrefab = prefabs[1];
    }

    public static GameObject GetBlockPrefabByIndex(int index)
    {
        return Blocks[index].BlockPrefab;
    }
}
