using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerPerson : MonoBehaviour
{
    [SerializeField] List<Person> personChar;

    private void Start()
    {
        foreach (var person in personChar)
        {
            person.Init();
        }
    }

    // Makes sure the player is healthy.
    public Person GetHealthyPerson()
    {
        return personChar.Where(x => x.HP > 0).FirstOrDefault();
    }
}