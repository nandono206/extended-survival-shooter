using System;
using UnityEngine;

public class PetFactory : MonoBehaviour, IFactory
{

    [SerializeField]
    public GameObject[] petPrefab;

    public GameObject FactoryMethod(int tag)
    {
        GameObject pet = Instantiate(petPrefab[tag]);
        return pet;
    }
}