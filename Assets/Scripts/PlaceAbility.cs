using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAbility : MonoBehaviour
{

    [SerializeField] SO_CharacterData character;
    [SerializeField] KeyCode placeAbKey = KeyCode.Mouse0;
    [SerializeField] KeyCode aimAbilityKey = KeyCode.Mouse1;
    [SerializeField] Camera cam;
    [SerializeField] Material aimMaterial;
    [SerializeField] Material defaultMaterial;
    [SerializeField] SO_CharacterData air;
    [SerializeField] SO_CharacterData water;
    [SerializeField] SO_CharacterData fire;
    private GameObject placeholder;
    private GameObject airAbilityTemp;
    private GameObject waterAbilityTemp;
    private GameObject fireAbilityTemp;

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
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(aimAbilityKey)){
            
            Vector3 rayOrigin = cam.ViewportToWorldPoint (new Vector3(0.5f, 0.5f, 0.0f));
            if (Physics.Raycast (rayOrigin, cam.transform.forward, out RaycastHit hit, Mathf.Infinity, layerMaskCombined))
            {
                
                bool letsBuild = true;
                
                switch(hit.collider.gameObject.layer){
                    case 2:{
                        letsBuild = false;
                        airAbilityTemp.SetActive(false);
                        waterAbilityTemp.SetActive(false);
                        fireAbilityTemp.SetActive(false);
                        break;
                    }
                    case 10: {
                        placeholder = air.ability;
                        airAbilityTemp.SetActive(true);
                        break;
                    }
                    case 11: {
                        placeholder = fire.ability;
                        fireAbilityTemp.SetActive(true);
                        break;
                    }
                    case 12: {
                        placeholder = water.ability;
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
                    if(Input.GetKeyDown(placeAbKey))
                        Instantiate(placeholder, hit.point, transform.rotation);                  
                }
                    
                
                
                  
               
            }
            // if(Input.GetKeyUp(aimAbilityKey)){

            //     placeholder.SetActive(false);
            // }
        }
        
    }
}
