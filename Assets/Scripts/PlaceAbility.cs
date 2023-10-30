using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAbility : MonoBehaviour
{

    [SerializeField] KeyCode placeAbKey = KeyCode.Mouse0;
    [SerializeField] KeyCode aimAbilityKey = KeyCode.Mouse1;
    [SerializeField] Camera cam;
    [SerializeField] SO_CharacterData air;
    [SerializeField] SO_CharacterData water;
    [SerializeField] SO_CharacterData fire;
    private GameObject tempPlaceholder;
    private GameObject trueAbility;
    private GameObject airAbilityTemp;
    private GameObject waterAbilityTemp;
    private GameObject fireAbilityTemp;
    //to test transition from aether to default elements( need better solution)
    private int badTest;


    // layer mask for raycast ignore.. 000000111000...
    int layerMaskCombined = (1 << 10) | (1 << 11) | (1 << 12) | (1 << 2);

    // Start is called before the first frame update
    void Start()
    {
        airAbilityTemp = Instantiate(air.abilityAim, transform.position, transform.rotation,  this.transform);
        fireAbilityTemp = Instantiate(fire.abilityAim, transform.position, transform.rotation,  this.transform);
        waterAbilityTemp = Instantiate(water.abilityAim, transform.position, transform.rotation,  this.transform);
        airAbilityTemp.SetActive(false);
        waterAbilityTemp.SetActive(false);
        fireAbilityTemp.SetActive(false);

        badTest = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(aimAbilityKey)){
            
            Vector3 rayOrigin = cam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast (rayOrigin, cam.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMaskCombined))
            {
                
                bool letsBuild = true;
                airAbilityTemp.SetActive(false);
                waterAbilityTemp.SetActive(false);
                fireAbilityTemp.SetActive(false);
                
                switch(hit.collider.gameObject.layer){
                    case 2:{
                        letsBuild = false;
                        airAbilityTemp.SetActive(false);
                        waterAbilityTemp.SetActive(false);
                        fireAbilityTemp.SetActive(false);
                        break;
                    }
                    case 10: {
                        trueAbility = air.ability;
                        tempPlaceholder = air.abilityAim;
                        airAbilityTemp.SetActive(true);
                        break;
                    }
                    case 11: {
                        trueAbility = fire.ability;
                        tempPlaceholder = fire.abilityAim;
                        fireAbilityTemp.SetActive(true);
                        break;
                    }
                    case 12: {
                        trueAbility = water.ability;
                        tempPlaceholder = water.abilityAim;
                        waterAbilityTemp.SetActive(true);
                        break;
                    }
                    // default:{
                    //     airAbilityTemp.SetActive(false);
                    //     waterAbilityTemp.SetActive(false);
                    //     fireAbilityTemp.SetActive(false);
                    //     break;
                    // }
                }

                if(letsBuild){
                    transform.position = hit.point;
                    if(Input.GetKeyDown(placeAbKey)){
                        GameObject temp = tempPlaceholder;
                        temp.GetComponent<MeshRenderer>().material = air.preActiveMat;
                        Instantiate(temp, hit.point, transform.rotation);
                        Instantiate(trueAbility,hit.point, transform.rotation);
                        if(badTest == 3)
                            transform.parent.gameObject.GetComponent<CharacterManagement>().preRun = false;
                        else
                            badTest++;
                        Debug.Log(badTest);
                    }
                }
                    
                
                
                  
               
            }else{
                airAbilityTemp.SetActive(false);
                waterAbilityTemp.SetActive(false);
                fireAbilityTemp.SetActive(false);
            }
        }
        if(Input.GetKeyUp(aimAbilityKey)){
            airAbilityTemp.SetActive(false);
            waterAbilityTemp.SetActive(false);
            fireAbilityTemp.SetActive(false);
        }
        
    }
}
