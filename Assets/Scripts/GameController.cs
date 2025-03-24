using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeMove, Battle, Dialog }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCamera;

    GameState state;

    // When playerController triggers OnEncountered event, StartBattle is called.
    // When battleSystem triggers OnBattleOver event, EndBattle is called.
    // When DialogManager triggers OnShowDialog/OnCloseDialog event, their functions are executed.
    private void Start()
    {
        playerController.OnEncountered += StartBattle;
        battleSystem.OnBattleOver += EndBattle;

        // Lambda functions
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
        battleSystem.gameObject.SetActive(true);
        worldCamera.gameObject.SetActive(false);

        battleSystem.StartBattle();
    }

    // Changes state back to FreeMove.
    // BattleSystem set to hidden.
    // Camera back to main camera.
    void EndBattle(bool won)
    {
        state = GameState.FreeMove;
        battleSystem.gameObject.SetActive(false);
        worldCamera.gameObject.SetActive(true);
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
