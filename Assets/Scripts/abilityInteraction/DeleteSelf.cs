using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
    
        if(other.gameObject.TryGetComponent(out PlaceAbility ability)){
            
        }

        if (other.attachedRigidbody)
        {
            Destroy(this.gameObject);
            Debug.Log("lalalal");
        }
    }

}
