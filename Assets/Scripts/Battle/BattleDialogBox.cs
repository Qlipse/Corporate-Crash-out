using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Attached to the DialogBox that shows changes based on states and visually show player's current choice.
public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] int lettersPerSecond;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject actionBox;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;
    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;
    [SerializeField] Text powerText;
    [SerializeField] Text typeText;
    [SerializeField] Color highlightedColor;

    // Sets dialog text to what is passed.
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    // Creates an animation that slowly shows text one letter at a time.
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }

        yield return new WaitForSeconds(1f);
    }

    // Function that sets visibility of the dialog textbox based on what argument is passed.
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    // Function to unhide Action Box.
    public void EnableActionBox(bool enabled)
    {
        actionBox.SetActive(enabled);
    }

    // Function to unhide move selector and move details textbox.
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    // Highlights current action selection by index of the possible action list.
    public void UpdateActionSelection(int selectedAction)
    {
        for(int i = 0; i < actionTexts.Count; i++)
        {
            if(i == selectedAction)
                actionTexts[i].color = highlightedColor;
                else
                    actionTexts[i].color = Color.black;

        }
    }

    // Highlights current move selection and move details by index of moveTexts and Player's character moves.
    public void UpdateMoveSelection(int selectedMove, Move move)
    {
        for(int i = 0; i < moveTexts.Count; ++i)
        {
            if(i == selectedMove)
                moveTexts[i].color = highlightedColor;
            else
                moveTexts[i].color = Color.black;
        }

        powerText.text = $"Power {move.Power}";
        typeText.text = move.Base.Type.ToString();
    }

    // Changes move texts to reflect player character moves.
    public void SetMoveNames(List<Move> moves)
    {
        for(int i = 0; i < moveTexts.Count; ++i)
        {
            if(i < moves.Count)
                moveTexts[i].text = moves[i].Base.Name;
            else
                moveTexts[i].text = "-";
        }
    }
}
