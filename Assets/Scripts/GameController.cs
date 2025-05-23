using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeMove, Battle, Dialog, Cutscene }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;
    
    [SerializeField] AudioClip mapMusic;

    GameState state;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    // When playerController triggers OnEncountered event, StartBattle is called.
    // When battleSystem triggers OnBattleOver event, EndBattle is called.
    // When DialogManager triggers OnShowDialog/OnCloseDialog event, their functions are executed.
    // When playerController triggers OnNPCEncounter event, a lambda function is executed to change state and prevent movement from other handlers.
    private void Start()
    {
        AudioManager.i.PlayMusic(mapMusic, true, true);
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        // Lambda functions
        playerController.OnNPCEncounter += (Collider2D NPCCollider) => 
        {
            var NPC = NPCCollider.GetComponent<NPCBattle>();
            if(NPC != null)
            {
                state = GameState.Cutscene;
                StartCoroutine(NPC.TriggerNPCBattle(playerController));
            }

        };

        DialogManager.Instance.OnShowDialog += () => 
        {
            state = GameState.Dialog;
        };

        DialogManager.Instance.OnCloseDialog += () => 
        {
            if (state == GameState.Dialog)
                state = GameState.FreeMove;
        };
    }

    // Changes state and unhides battleSystem while also changing the main camera.
    // Also starting battle.
    void StartBattle()
    {
        state = GameState.Battle;
        worldCamera.GetComponent<AudioListener>().enabled = false;
        battleSystem.GetComponentInChildren<AudioListener>().enabled = true;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerPerson = playerController.GetComponent<PlayerPerson>();
        var randomPerson = FindObjectOfType<MapArea>().GetComponent<MapArea>().GetRandomPerson();
        battleSystem.StartBattle(playerPerson, randomPerson);
    }

    // Initiates Battle with given NPC.
    public void StartNPCBattle(NPCBattle npc)
    {
        state = GameState.Battle;
        worldCamera.GetComponent<AudioListener>().enabled = false;
        battleSystem.GetComponentInChildren<AudioListener>().enabled = true;
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        var playerPerson = playerController.GetComponent<PlayerPerson>();
        var npcPerson = npc.GetComponent<NPCPerson>();
        battleSystem.StartNPCBattle(playerPerson, npcPerson);
    }

    // Changes state back to FreeMove.
    // BattleSystem set to hidden.
    // Camera back to main camera.
    void EndBattle(bool won)
    {
        state = GameState.FreeMove;
        battleSystem.GetComponentInChildren<AudioListener>().enabled = false;
        worldCamera.GetComponent<AudioListener>().enabled = true;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
        AudioManager.i.PlayMusic(mapMusic, true, true);
    }

    // Update is called every frame.
    // Check for GameState changes and grant control to whichever state it is currently on.
    private void Update()
    {
        if (state == GameState.FreeMove)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Battle)
        {
            battleSystem.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
    }
}
