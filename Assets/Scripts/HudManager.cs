using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class HudManager : MonoBehaviour
{
    
    public static HudManager instance;
    public Player playerRef;
    public TMP_Text EnemyLifeText;
    public TMP_Text MyBaseLifeText;
    public TMP_Text IDPLAYER;
    int ID;

    
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
        }
    }

    private void Update()
    {
        if (!playerRef) return;
        IDPLAYER.text = playerRef.playerID.ToString();
    }

    public void PlayerIDAssign(int IDPlayer)
    {
        ID = IDPlayer;
    }
    public void ChangeText(int number)
    {
        EnemyLifeText.text = number.ToString();
    }

    public void BaseReceivesDMG(int vida, int IDBase)
    {
        if (IDBase == playerRef.playerID)
        {
            MyBaseLifeText.text = vida.ToString();
        }
        else EnemyLifeText.text = vida.ToString();



       
    }
    
}
