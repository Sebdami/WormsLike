using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour {
    [SerializeField]
    float spawnHeight = 100.0f;

    [Header("Weapon Crate")]

    public GameObject WeaponCratePrefab;

    [SerializeField]
    float weaponCrateSpawnInterval = 60f;
    
    [SerializeField]
    float weaponCrateSpawnOffset = 0.0f;

    [Header("Healing Crate")]

    public GameObject HealingCratePrefab;

    [SerializeField]
    float healingCrateSpawnInterval = 60f;

    [SerializeField]
    float healingCrateSpawnOffset = 30.0f;

    float minX;
    float maxX;

    void Start () {
        minX = 4.0f;
        maxX = GameManager.instance.world.worldX * GameManager.instance.world.transform.localScale.x - 4.0f;
        StartCoroutine(SpawnWeaponCrateEvery(weaponCrateSpawnInterval, weaponCrateSpawnOffset));
        StartCoroutine(SpawnHealingCrateEvery(weaponCrateSpawnInterval, healingCrateSpawnOffset));
    }

    void SpawnHealingCrate()
    {
        Vector3 spawnPos = new Vector3(Random.Range(minX, maxX), spawnHeight, 1.0f);
        Instantiate(HealingCratePrefab, spawnPos, Quaternion.identity);
    }

    void SpawnWeaponCrate()
    {
        Vector3 spawnPos = new Vector3(Random.Range(minX, maxX), spawnHeight, 1.0f);
        WeaponCrate crate = Instantiate(WeaponCratePrefab, spawnPos, Quaternion.identity).GetComponent<WeaponCrate>();
        crate.Init(GameManager.instance.WeaponDb.GetRandomPickableWeaponIndex(), 1);
    }

    IEnumerator SpawnWeaponCrateEvery(float seconds, float offset)
    {
        yield return new WaitForSeconds(offset);
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            SpawnWeaponCrate();
        }
    }
    IEnumerator SpawnHealingCrateEvery(float seconds, float offset)
    {
        yield return new WaitForSeconds(offset);
        while (true)
        {
            yield return new WaitForSeconds(seconds);
            SpawnHealingCrate();
        }
    }
}
