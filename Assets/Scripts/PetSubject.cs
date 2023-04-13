using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetSubject : MonoBehaviour
{
    // a collection of all the observers of this subject
    public List<PetObserver> observers = new List<PetObserver>();
    // add the observer to the subject's collection
    public void AddObserver(PetObserver observer)
    {
      
        observers.Add(observer);
      
    }
    // remove the observer from the subject's collection
    public void RemoveObserver(PetObserver observer)
    {
        observers.Remove(observer);
    }
    // notify each observer that an event has occurred
    protected void NotifyObservers(string petTag)
    {

      
        observers.ForEach((_observer) => {
           
            _observer.OnNotify(petTag);
        });

        
    }

    protected void NotifyDead()
    {


        observers.ForEach((_observer) => {

            _observer.OnNotifyDead();
        });

       
    }
}
