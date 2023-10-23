using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Movement")]
    float moveSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float groundDrag;

    [Header("Jumping")]
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airSpeedMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    [SerializeField] float crouchSpeed;
    [SerializeField] float crouchYScale;
     float startYScale;
 
    [Header("CameraSwitch")]
    public Transform combatLookAt;
    [SerializeField] GameObject basicCam;
    [SerializeField] GameObject topDownCam;
    [SerializeField] GameObject combatCam;


    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask Ground;
    [SerializeField] float extendedGrCheck = 0.2f;
    bool grounded;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode BasicCamKey = KeyCode.Alpha1;
    public KeyCode TopDownCamKey = KeyCode.Alpha2;
    public KeyCode CombatCamKey = KeyCode.Alpha3;


    [Header("References")]
    [SerializeField] GameObject mainCamObj;
    public Transform playerObj;
    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;
    
    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState moveState;
    public enum MovementState{
        walking,
        sprinting,
        crouching,
        air
    }

    public CameraState camState;
    public enum CameraState{
        Basic,
        Combat,
        Topdown
    }

    // Start is called before the first frame update
    void Start()
    {
        //invisible cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        //
        startYScale = transform.localScale.y;

        readyToJump = true;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate(){
        MovePlayer();
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && grounded){
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if(Input.GetKeyDown(crouchKey)){
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if(Input.GetKeyUp(crouchKey)){
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
        if(Input.GetKeyDown(BasicCamKey)) SwitchCameraView(CameraState.Basic);
        if(Input.GetKeyDown(TopDownCamKey)) SwitchCameraView(CameraState.Topdown);
        if(Input.GetKeyDown(CombatCamKey)) SwitchCameraView(CameraState.Combat);

    }

    private void MoveStateHandler(){
        if(Input.GetKey(crouchKey)){
            moveState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }else if(grounded && Input.GetKey(sprintKey)){
            moveState = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(grounded){
            moveState = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else{
            moveState = MovementState.air;
        }
    }

    private void RotatePlayer(){
        //easy rotate if these two cams
        if(camState == CameraState.Basic || camState == CameraState.Topdown){
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if(inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, rotationSpeed * Time.deltaTime);
        }
        //hard rotation for combat
        else if(camState == CameraState.Combat){
            Vector3 combatViewDir = combatLookAt.position - new Vector3(mainCamObj.transform.position.x, combatLookAt.position.y, mainCamObj.transform.position.z);
            orientation.forward = combatViewDir.normalized;

            playerObj.forward = combatViewDir.normalized;
        }
    }

    private void MovePlayer(){
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airSpeedMultiplier, ForceMode.Force);
    }

    private void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        
        if(flatVel.magnitude > moveSpeed){
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump(){
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void SwitchCameraView(CameraState newView){
        basicCam.SetActive(false);
        topDownCam.SetActive(false);
        combatCam.SetActive(false);

        if(newView == CameraState.Basic){
            basicCam.SetActive(true);
            mainCamObj = basicCam;
        }if(newView == CameraState.Topdown){
            topDownCam.SetActive(true);
            mainCamObj = topDownCam;
        }if(newView == CameraState.Combat){
            combatCam.SetActive(true);
            mainCamObj = combatCam;
        } 

        camState = newView;
         
    }

    private void ResetJump(){
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + extendedGrCheck, Ground);
        //view direction (orientation)
        Vector3 viewDir = transform.position - new Vector3(mainCamObj.transform.position.x, transform.position.y, mainCamObj.transform.position.z);
        orientation.forward = viewDir.normalized;

        MyInput();
        SpeedControl();
        RotatePlayer();
        MoveStateHandler();
        
        if(grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }
}
