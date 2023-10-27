using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{
    public SO_CharacterData waterCharacter;
    public SO_CharacterData fireCharacter;
    public SO_CharacterData airCharacter;
    public SO_CharacterData earthCharacter;

    public Move moveScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(waterCharacter.changeChar)) moveScript.UpdateStats(waterCharacter);
        if(Input.GetKeyDown(fireCharacter.changeChar)) moveScript.UpdateStats(fireCharacter);
        if(Input.GetKeyDown(airCharacter.changeChar)) moveScript.UpdateStats(airCharacter);
        if(Input.GetKeyDown(earthCharacter.changeChar)) moveScript.UpdateStats(earthCharacter);
        
    }

    
}
