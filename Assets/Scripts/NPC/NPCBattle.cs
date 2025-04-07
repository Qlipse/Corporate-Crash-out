using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBattle : MonoBehaviour
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameObject alert;

    // Displays the alert GameObject and displays dialogs and then on completion of dialog, execute the function.
    public IEnumerator TriggerNPCBattle(PlayerController player)
    {
        alert.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        alert.SetActive(false);

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, () => 
        {
            Debug.Log("TriggerNPCBattle");
        }));
    }
}
