using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask interactableLayer;
    private bool isMoving;
    private Vector2 input;

    public event Action OnEncountered;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // HandleUpdate will update the player when called upon by GameController.
    public void HandleUpdate()
    {
        if(!isMoving) 
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            
            if(input.x != 0 && input.y != 0) 
            {
                input.y = 0;
            }

        if(input != Vector2.zero && !isMoving) 
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);
            var targetPos = transform.position;
            targetPos.x += input.x;
            targetPos.y += input.y;

            if(IsWalkable(targetPos))
            {
                StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }
        }

    // Interact function that checks in front of player to see if there is anything in the interactable layer and calling the object's Interact function.
    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact();
        }
        //Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);
    }

    IEnumerator Move(Vector3 targetPos) {
    isMoving = true;
    while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed *Time.deltaTime);
        yield return null;
    }
    transform.position = targetPos;
    isMoving = false;
        }
    }

    // Checks if Player's next movement is allowed by seeing if the next step will overlap with the solidObjectslayer.
    private bool IsWalkable(Vector3 targetPos) 
    {
        if(Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
        {
            return false;
        }

        return true;
    }


}