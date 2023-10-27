using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrigger : Interactable
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collidededed");
        
        Activate();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggeredededed");
        
        Activate();
    }
}
