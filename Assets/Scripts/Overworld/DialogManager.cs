using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// Manager for Overworld Dialogues
public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    // Events for Dialog states in GameController
    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    // Allows access to DialogManager's Instance from other scripts.
    public static DialogManager Instance { get; private set; }

    // Before application starts, attach the object to Instance.
    private void Awake()
    {
        Instance = this;
    }

    Dialog dialog;
    int currentLine = 0;
    bool isTyping;

    // Displays Dialog from NPC
    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();

        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    // Changes DialogBox to the next line if it exists. Otherwise, CloseDialog event is called.
    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
            }
            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }

    // Function taken from BattleDialogBox that slowly displays the lines for animation.
    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
        isTyping = false;
    }
}


