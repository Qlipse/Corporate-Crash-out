using System;
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

    public event Action<bool> OnBattleOver;

    BattleState state;
    int currentAction;
    int currentMove;
    int escapeAttempts;

    PlayerPerson playerPerson;
    Person randomPerson;


    // Set up player/enemy information.
    // Coroutine allows for execution of code to pause and resume at various points.
    public void StartBattle(PlayerPerson playerPerson, Person randomPerson)
    {
        this.playerPerson = playerPerson;
        this.randomPerson = randomPerson;
        StartCoroutine(SetupBattle());
    }


    // Sets up player sprites and data as well as enemy sprite and data.
    // As well as dialogue before fight.
    public IEnumerator SetupBattle()
    {
        var healthyPlayer = playerPerson?.GetHealthyPerson();
        if (healthyPlayer != null)
        {
            playerChar.Setup(healthyPlayer);
            playerHud.SetData(playerChar.Person);
            dialogBox.SetMoveNames(playerChar.Person.Moves);
        }

        enemyChar.Setup(randomPerson);
        enemyHud.SetData(enemyChar.Person);


        yield return dialogBox.TypeDialog($"Co-worker {enemyChar.Person.Base.Name} appeared.");

        escapeAttempts = 0;
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

        // Plays attack animation and waits 1 second
        playerChar.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        enemyChar.PlayHitAnimation();
        var damageDetails = enemyChar.Person.TakeDamage(move, playerChar.Person);
        yield return enemyHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{enemyChar.Person.Base.Name} Fainted");
            enemyChar.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
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

        enemyChar.PlayAttackAnimation();
        yield return new WaitForSeconds(1f);

        playerChar.PlayHitAnimation();
        var damageDetails = playerChar.Person.TakeDamage(move, playerChar.Person);
        yield return playerHud.UpdateHP();
        yield return ShowDamageDetails(damageDetails);
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerChar.Person.Base.Name} Fainted");
            playerChar.PlayFaintAnimation();

            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
        }
        else
        {
            PlayerAction();
        }
    }

    // Checks if the attack was a critical/type effective move and display dialog to let the player know.
    IEnumerator ShowDamageDetails(DamageDetails damageDetails)
    {
        if (damageDetails.Critical > 1f)
        {
            yield return dialogBox.TypeDialog("A critical hit!");
        }
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog("It's super effective!");
        } else if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog("It's not very effective!");
        }
    }

    // Function called every frame to check for state changes and calls handler corresponding to state.
    public void HandleUpdate()
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
            else if(currentAction == 2)
            {

            }
            else if(currentAction == 3)
            {
                StartCoroutine(TryToEscape());
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

    // Function for the Run action during battle
    // the formula is taken from Pokémon Generation III and IV game.
    IEnumerator TryToEscape()
    {
        state = BattleState.Busy;

        ++escapeAttempts;
        int playerSpeed = playerChar.Person.Speed;
        int enemySpeed = enemyChar.Person.Speed;

        if(enemySpeed < playerSpeed)
        {
            yield return dialogBox.TypeDialog($"Ran away safely!");
            OnBattleOver(true);
        }
        else
        {
            float f = (playerSpeed * 128) / enemySpeed + 30 * escapeAttempts;
            f = f % 256;

            if(UnityEngine.Random.Range(0, 256) < f)
            {
                yield return dialogBox.TypeDialog($"Ran away safely!");
                OnBattleOver(true);
            }
            else
            {
                yield return dialogBox.TypeDialog($"Failed to escape.");
                StartCoroutine(EnemyMove());
            }
        }
    }
}