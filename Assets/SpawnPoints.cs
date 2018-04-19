using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour {
    List<Vector3> spawnPoints = null;
    void Init()
    {
        spawnPoints = new List<Vector3>();
        for (int i = 0; i < transform.childCount; i++)
            spawnPoints.Add(transform.GetChild(i).position);
    }

    public Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints == null)
            Init();
        int index = Random.Range(0, spawnPoints.Count);
        Vector3 toReturn = spawnPoints[index];
        spawnPoints.RemoveAt(index);
        return toReturn;
    }
}
