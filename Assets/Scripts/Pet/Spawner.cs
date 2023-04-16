using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : PetSubject
{
    public List<GameObject> objectsToSpawn = new List<GameObject>();

    [SerializeField]
    private Camera Camera;
    [SerializeField]
    private Canvas HealthBarCanvas;

    public int health = -999;



    GameObject spawnedPet;
    string activePet;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SpawnObject(int index)
    {
        Debug.Log(spawnedPet);
        if (spawnedPet != null) {
            
            Destroy(spawnedPet);
        }
        Debug.Log(spawnedPet);
        spawnedPet = Instantiate(objectsToSpawn[index], transform.position, transform.rotation);
        PetController spawnedPetController = spawnedPet.GetComponent<PetController>();
        if (health > 0)
        {
            spawnedPetController.Health = health;
        }
        spawnedPetController.SetupHealthBar(Camera);
        spawnedPetController.observers = this.observers;
        activePet = spawnedPet.tag;
        NotifyObservers(activePet);
      
        
        
    }

    public void FullHpPet()
    {
        if (spawnedPet != null)
        {
            spawnedPet.GetComponent<PetController>().fullHpCheat();
        }
    }

    public void DestroyObject()
    {
        spawnedPet.GetComponent<PetController>().Health = 0;
        Destroy(spawnedPet);
        NotifyDead();
    }

    public void OnNotifyDamaged(int health)
    {
        this.health = health;
    }
}
