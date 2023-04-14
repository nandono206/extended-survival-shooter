using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;


    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }


    void Spawn()
    {
        if (playerHealth.currentHealth <= 0f)
        {
            return;
        }

        // int spawnPointIndex = Random.Range (0, spawnPoints.Length);
        // Instantiate(enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);

    }
}


public class SpawnedObject : MonoBehaviour
{
    public int uniqueId; // unique identifier for each spawned object
    // ... other properties and methods for the spawned object ...
}

public class ObjectManager : MonoBehaviour
{
    Dictionary<int, SpawnedObject> objectDictionary = new Dictionary<int, SpawnedObject>();

    // Method to spawn an object and assign a unique identifier
    public void SpawnObject(GameObject prefab)
    {
        GameObject spawnedObject = Instantiate(prefab);
        SpawnedObject spawnedObjectComponent = spawnedObject.GetComponent<SpawnedObject>();
        spawnedObjectComponent.uniqueId = GetUniqueId(); // assign a unique identifier
        objectDictionary.Add(spawnedObjectComponent.uniqueId, spawnedObjectComponent); // add to dictionary using unique identifier as key
    }

    // Method to retrieve an object from the dictionary using its unique identifier
    public SpawnedObject GetObjectById(int uniqueId)
    {
        SpawnedObject spawnedObject;
        if (objectDictionary.TryGetValue(uniqueId, out spawnedObject))
        {
            return spawnedObject;
        }
        else
        {
            return null; // object not found in dictionary
        }
    }

    // Method to generate a unique identifier
    private int GetUniqueId()
    {
        // Implement your own logic to generate unique identifiers, such as using an incremental counter, a GUID, etc.
        // Example: return an incremental counter
        return objectDictionary.Count + 1;
    }
}
