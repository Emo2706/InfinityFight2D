using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inputs
{
    Dictionary<KeyCode, CommandInput> _commandDictionary;

    public Player_Inputs()
    {
        _commandDictionary = new Dictionary<KeyCode, CommandInput>();
    }

    public void BlindKeys(KeyCode key , CommandInput input)
    {
        _commandDictionary[key] = input;
    }

    public CommandInput Inputs()
    {
        foreach (var pair in _commandDictionary)
        {
            if (Input.GetKeyDown(pair.Key))
                return pair.Value;

            
        }

        return null;
    }

    
}
