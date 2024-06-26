using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class HudManager : MonoBehaviour
{
    
    public static HudManager instance;
    [HideInInspector] public Player playerRef;

    [Header("Texts")]
    public TMP_Text EnemyLifeText;
    public TMP_Text MyBaseLifeText;
    public TMP_Text IDPLAYER;
    public TMP_Text grenadesCount;
    public TMP_Text VidaTest;
    [SerializeField] TMP_Text _respawningText;
    public TMP_Text BufoText;

    [Header("Images")]
    public Image grenadeImg;
    public Image wallImg;
    
    [Header("Sliders")]
    public Slider MyBaseLifeSlider;
    public Slider EnemyBaseLifeSlider;

    [SerializeField] GameObject WinScreen;
    [SerializeField] GameObject LoseScreen;
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
    private void Start()
    {
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_GameOver, ShowGameOverScene);
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerDies, PlayerDied);
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_PlayerSpawns, PlayerRespawned);
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
        else EnemyBaseLifeSlider.value = EnemyBaseLifeSlider.maxValue - vida;

       
    }

    public void ChangeGrenadesCount(int number)
    {
        grenadesCount.text = number.ToString();
    }

    void ShowGameOverScene(params object[] parameters)
    {
        //Si me pasan mi ID, quiere decir que destruyeron mi base
        if ((int)parameters[0] == LocalPlayerDataManager.instance.LocalPlayer.playerID)
        {
            LoseScreen.gameObject.SetActive(true);
        }
        else WinScreen.gameObject.SetActive(true);
        


    }
    
    void PlayerDied(params object[] parameters)
    {
        if ((int)parameters[0] == LocalPlayerDataManager.instance.LocalPlayer.playerID)
        {
            _respawningText.gameObject.SetActive(true);
        }
    }
    void PlayerRespawned(params object[] parameters)
    {
        if ((int)parameters[0] == LocalPlayerDataManager.instance.LocalPlayer.playerID)
        {
            _respawningText.gameObject.SetActive(false);
            StartCoroutine(BuffPopupText());
        }
    }

    IEnumerator BuffPopupText()
    {
        BufoText.gameObject.SetActive(true);
        float tiempoParaDesaparecer = 3;
        yield return new WaitForSeconds(tiempoParaDesaparecer);
        BufoText.gameObject.SetActive(false);


    }
}
