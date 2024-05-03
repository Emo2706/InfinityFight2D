using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerDataManager : MonoBehaviour
{
    public static LocalPlayerDataManager instance;
    [HideInInspector] public Player LocalPlayer;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }
    
    public void SetLocalPlayer(Player py)
    {
        LocalPlayer = py;
    }
    
}
