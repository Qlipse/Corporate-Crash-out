using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// NPCController implements Interactable interface and defines the function.
public class NPCController : MonoBehaviour, Interactable
{

    [SerializeField] Dialog dialog;

    Healer healer;

    private void Awake()
    {
        healer = GetComponent<Healer>();
    }

    public IEnumerator Interact()
    {
        if (healer != null)
        {
            yield return healer.Heal(dialog);
        } else 
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }

}
