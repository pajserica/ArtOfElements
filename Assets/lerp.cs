using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lerp : MonoBehaviour
{

    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 endPos;
    float lerpSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;
    }

    // Update is called once per frame
    void Update()
    {
        float blend = Time.deltaTime * lerpSpeed;
        Debug.Log(blend);
        transform.position = Vector3.Lerp(transform.position, endPos, blend);
    }
}
