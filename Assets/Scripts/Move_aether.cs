using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_aether : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float speedMultiplier;
    [SerializeField] KeyCode sprint = KeyCode.Space;
    [SerializeField] float rotationSpeed;
    [Header("Reference")]
    [SerializeField] GameObject mainCamObj;
    [SerializeField] Transform playerObj;
    [SerializeField] Transform orientation;
    [SerializeField] Transform lookAt;
    float horizontalInput;
    float verticalInput;

    private CharacterManagement CM_Script;

    // Start is called before the first frame update
    void Start()
    {
        CM_Script = this.transform.parent.GetComponent<CharacterManagement>();
        //invisible cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    // Update is called once per frame
    void Update()
    {
        float tempSpeed = 1;
        if(Input.GetKey(sprint)){
            tempSpeed = speedMultiplier;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        transform.position += (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized * moveSpeed * tempSpeed * Time.deltaTime;
        
        RotateChar();
        
    }

    private void RotateChar(){
        Vector3 combatViewDir = lookAt.position - new Vector3(mainCamObj.transform.position.x, mainCamObj.transform.position.y, mainCamObj.transform.position.z);
        // Debug.Log(combatViewDir);
        orientation.forward = combatViewDir.normalized;

        playerObj.forward = combatViewDir.normalized;
    }

}
