using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Movement")]
    float moveSpeed;
    public float walkSpeed;
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
    [SerializeField] Transform combatLookAt;
    [SerializeField] GameObject basicCam;
    [SerializeField] GameObject topDownCam;
    [SerializeField] GameObject combatCam;


    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    [SerializeField] LayerMask Ground;
    [SerializeField] float extendedGrCheck = 0.2f;
    bool grounded;

    [Header("KeyBinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] KeyCode ChangeCamKey = KeyCode.V;
    private int camNum;


    [Header("References")]
    [SerializeField] SO_CharacterData defaultChar;
    public GameObject ability;
    [SerializeField] GameObject mainCamObj;
    [SerializeField] Transform playerObj;
    [SerializeField] Transform orientation;

    float horizontalInput;
    float verticalInput;
    public int whatElement;
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

    public float ExtendedGrCheck => extendedGrCheck;

    // Start is called before the first frame update
    void Start()
    {
        //invisible cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
        // cam
        camNum = 1;

        startYScale = transform.localScale.y;

        readyToJump = true;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        // OnStart disable this movement
        UpdateStats(defaultChar);
        SwitchCameraView(1);
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
        if(Input.GetKeyDown(ChangeCamKey)) SwitchCameraView(++camNum);
        // if(Input.GetKeyDown(TopDownCamKey)) SwitchCameraView(CameraState.Topdown);
        // if(Input.GetKeyDown(CombatCamKey)) SwitchCameraView(CameraState.Combat);

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
            Vector3 combatViewDir = combatLookAt.position - new Vector3(combatCam.transform.position.x, combatLookAt.position.y, combatCam.transform.position.z);
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


    private void SwitchCameraView(int num){
        basicCam.SetActive(false);
        topDownCam.SetActive(false);
        combatCam.SetActive(false);
        if(num > 2){
            camNum = 1;
            num = 1;
        }

        if(num == 1){
            basicCam.SetActive(true);
            mainCamObj = basicCam;
            camState = CameraState.Basic;
        }else if(num == 2){
            topDownCam.SetActive(true);
            mainCamObj = topDownCam;
            camState = CameraState.Topdown;
        }else if(num == 3){
            combatCam.SetActive(true);
            mainCamObj = combatCam;
            camState = CameraState.Combat;
        }


        
         
    }

    private void ResetJump(){
        readyToJump = true;
    }

    //Update all stats
    public void UpdateStats(SO_CharacterData character){ // updating char stats
        walkSpeed = character.walkSpeed;
        sprintSpeed = character.sprintSpeed;
        rotationSpeed = character.rotationSpeed;
        groundDrag = character.groundDrag;
        jumpForce = character.jumpForce;
        jumpCooldown = character.jumpCooldown;
        airSpeedMultiplier = character.airSpeedMultiplier;
        crouchSpeed = character.crouchSpeed;
        whatElement = character.id;
        ability = character.ability;
        // rb.mass = character.mass;
        if(playerObj.transform.childCount != 0)
            Destroy(playerObj.transform.GetChild(0).gameObject);
        Instantiate(character.charObject, playerObj.transform.position, playerObj.transform.rotation,  playerObj.transform);
    }

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
