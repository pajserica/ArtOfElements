using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NewCharacter")]
public class SO_CharacterData : ScriptableObject
{

    public GameObject charObject;
    public GameObject ability;
    // Obj When aiming
    public GameObject abilityAim;

    public KeyCode changeChar = KeyCode.Alpha1;

    [Header("Movement")]
    float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float rotationSpeed;
    public float groundDrag;
    public float canInclude_mass;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airSpeedMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;

    public int id;

    public void PlaceAbility(GameObject gameObj){
        
        // Instantiate(gameObj, transform.position, transform.rotation);
    }
 
}
