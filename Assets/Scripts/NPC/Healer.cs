using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] GameObject player;
    public IEnumerator Heal( Dialog dialog )
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        var playerPerson = player.GetComponent<PlayerPerson>();
        playerPerson.healUp();
    }
}
