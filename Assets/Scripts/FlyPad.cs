using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPad : MonoBehaviour
{
    [SerializeField] int size;
    [SerializeField] float force;


    // Start is called before the first frame update
    void Start()
    {
        // transform.localScale =new Vector3(transform.localScale.x, size, transform.localScale.z);
        
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
            other.attachedRigidbody.AddForce(Vector3.up * force);
        }
    }
}
