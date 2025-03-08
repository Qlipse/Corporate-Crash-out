using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//States for battle
public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleChar playerChar;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleChar enemyChar;
    [SerializeField] BattleHud enemyHud;

    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;
    int currentMove;

    // Runs once to set up player/enemy information.
    // Coroutine allows for execution of code to pause and resume at various points.
    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    // Sets up player sprites and data as well as enemy sprite and data.
    // As well as dialogue before fight.
    public IEnumerator SetupBattle()
    {
        playerChar.Setup();
        enemyChar.Setup();
        playerHud.SetData(playerChar.Person);
        enemyHud.SetData(enemyChar.Person);

        dialogBox.SetMoveNames(playerChar.Person.Moves);

        yield return dialogBox.TypeDialog($"Co-worker {enemyChar.Person.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }


    // After introduction of enemy(coworker), change state, and unhide action textbox.
    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action"));
        dialogBox.EnableActionBox(true);
    }

    // When Player inputs Z, state changes and text box change to show moves.
    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionBox(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    // Performs the selected move and does damage to enemy.
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        var move = playerChar.Person.Moves[currentMove];
        yield return dialogBox.TypeDialog($"{playerChar.Person.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);
        bool isFainted = enemyChar.Person.TakeDamage(move, playerChar.Person);
        yield return enemyHud.UpdateHP();
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{enemyChar.Person.Base.Name} Fainted");
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    // Function that changes state and selects a random move for the enemy.
    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        var move = enemyChar.Person.GetRandomMove();
        yield return dialogBox.TypeDialog($"{enemyChar.Person.Base.Name} used {move.Base.Name}");
        yield return new WaitForSeconds(1f);
        bool isFainted = playerChar.Person.TakeDamage(move, playerChar.Person);
        yield return playerHud.UpdateHP();
        if (isFainted)
        {
            yield return dialogBox.TypeDialog($"{playerChar.Person.Base.Name} Fainted");
        }
        else
        {
            PlayerAction();
        }
    }

    // Function called every frame to check for state changes and calls handler corresponding to state.
    private void Update()
    {
        if(state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    // Handler for PlayerAction(Player starts in PlayerAction state).
    // Takes arrow key inputs which calls a function from BattleDialogBox.cs to highlight selection while also keeping track of player's input and selection.
    void HandleActionSelection()
    {
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentAction < 1 || currentAction == 2)
                ++currentAction;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentAction > 0 && currentAction != 2)
                --currentAction;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentAction < 2)
                currentAction += 2;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentAction > 1)
                currentAction -= 2;
        }
    }

        dialogBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(currentAction == 0)
            {
                PlayerMove();
            }
            else if(currentAction == 1)
            {
                
            }
        }
    }

    // Handler for PlayerMove state.
    // Calls a function from BattleDialogBox.cs to show player current selection.(Player selection not implemented yet)
    void HandleMoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentMove < playerChar.Person.Moves.Count - 1)
                ++currentMove;
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentMove > 0)
                --currentMove;
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentMove < playerChar.Person.Moves.Count - 2)
                currentMove += 2;
        }
        else if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerChar.Person.Moves[currentMove]);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnableMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove());
        }
    }
}
