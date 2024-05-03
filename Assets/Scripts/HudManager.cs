using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HudManager : MonoBehaviour
{
    
    public static HudManager instance;
    public Player playerRef;
    public TMP_Text EnemyLifeText;
    public TMP_Text MyBaseLifeText;
    public TMP_Text IDPLAYER;
    public TMP_Text grenadesCount;
    public Image grenadeImg;
    public Slider MyBaseLifeSlider;
    public Slider EnemyBaseLifeSlider;
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
   /* public void ChangeText(int number)
    {
        EnemyLifeText.text = number.ToString();
    }*/

    public void BaseReceivesDMG(int vida, int IDBase)
    {
        if (IDBase == playerRef.playerID)
        {
            MyBaseLifeSlider.value = vida;
        }
        else EnemyBaseLifeSlider.value = vida;

       
    }

    public void ChangeGrenadesCount(int number)
    {
        grenadesCount.text = number.ToString();
    }
    
}
