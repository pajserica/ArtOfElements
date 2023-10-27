using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyPad : MonoBehaviour
{
    [SerializeField] int size;
    [SerializeField] float force;
    [SerializeField] Move moveScript;


    // Start is called before the first frame update
    void Start()
    {
        // transform.localScale =new Vector3(transform.localScale.x, size, transform.localScale.z);
        if(GameObject.FindGameObjectsWithTag("Player") != null){
            moveScript = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Move>();
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    void OnTriggerStay(Collider other)
    {
        
        
        if (other.attachedRigidbody)
        {
            other.attachedRigidbody.AddForce(Vector3.up * force);
        }
    }
}
