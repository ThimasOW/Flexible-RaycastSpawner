using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSpreader : MonoBehaviour
{
    [Tooltip("These objects will spawn randomly around the map in interval")]
    public List<GameObject> objectsToSpawn;
    [Tooltip("How many distinct prefabs should be included")] 
    public int starterTypes = 4;
    [Tooltip("How many of the distinct prefabs should be spawned")]
    public int spawnCount = 10;
    
    List<GameObject> startSpawns = new List<GameObject>();
    [SerializeField]
    LayerMask spawnedObjectLayer; //Layers not to spawn on
    [SerializeField]
    LayerMask terrainLayer; //Layers to spawn on

    [Header("Parameters")]
    // The max distance of a raycast
    float raycastDistance = 100f;
    // Size of the box that is checked at the RaycastHit: x*x*x
    float overlapTestBoxSize = 1f;
    // The X, Y, Z of the spawn radius (in the form of a cube or plane)
    float itemXSpread = 100;
    float itemYSpread = 0;
    float itemZSpread = 150;
    float spawnInterval = 1f;
    // Should it spawn prefabs from objectsToSpawn list every x second?
    bool intervalSpawning = true;

    IEnumerator SpawnEntitiesRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnInterval);
        while (true)
        {
            yield return wait;

            // Spawn entity every spawnInterval seconds
            if (intervalSpawning)
                Spawn();
        }
    }

    void Start()
    {
        ChooseRandomStarterSpawns();

        Spawn(starterTypes * spawnCount);

        if (spawnInterval > 0)
            StartCoroutine(SpawnEntitiesRoutine());
    }

    void Spawn(int amount)
    {
        if (amount == 0) return;

        for (int i = 0; i < amount; i++)
            Spawn();
    }

    void Spawn()
    {
        RaycastHit hit;
        Vector3 randPosition = new Vector3(Random.Range(-itemXSpread, itemXSpread), Random.Range(-itemYSpread, itemYSpread), Random.Range(-itemZSpread, itemZSpread)) + transform.position;

        if (Physics.Raycast(randPosition, Vector3.down, out hit, raycastDistance, terrainLayer))
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            // Constraints rotation of possible spawns
            if (spawnRotation.eulerAngles.z > 25 || spawnRotation.eulerAngles.x > 25)
            {
                Spawn();
                return;
            }

            Vector3 overlapTestBoxScale = new Vector3(overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] arr = Physics.OverlapBox(hit.point, overlapTestBoxScale, spawnRotation, spawnedObjectLayer);

            if (arr.Length == 0)
            {
                Pick(hit.point, spawnRotation);
                //Debug.DrawLine(hit.point, randPosition, Color.red, 2f);
                return;
            }
        }
        Spawn();
    }

    void Pick(Vector3 positionToSpawn, Quaternion rotationToSpawn)
    {
        if (startSpawns.Count < 1)
            Instantiate(objectsToSpawn[Random.Range(0, objectsToSpawn.Count)], positionToSpawn, rotationToSpawn);

        else
        {
            GameObject random = startSpawns[Random.Range(0, startSpawns.Count)];
            Instantiate(random, positionToSpawn, rotationToSpawn);
            startSpawns.Remove(random);
        }
    }

    void ChooseRandomStarterSpawns()
    {
        if (starterTypes == 0 || spawnCount == 0) return;
        if (startSpawns.Count > 0) return;

        List<GameObject> prefabs = "a list of all your prefabs that can be spawned"; // Example: use a singleton to get them

        for (int i = 0; i < starterTypes; i++)
        {
            GameObject spawn = prefabs[Random.Range(0, prefabs.Count)];

            // Finds distinct type that has not been included yet.
            while (startSpawns.Contains(spawn))
            {
                spawn = prefabs[Random.Range(0, prefabs.Count)];
            }

            for (int j = 0; j < spawnCount; j++)
                startSpawns.Add(spawn);
        }
    }
}
