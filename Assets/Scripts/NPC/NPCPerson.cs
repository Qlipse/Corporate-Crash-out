using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Copy of PlayerPerson.cs so an NPC can also have their own Person object stored.
public class NPCPerson : MonoBehaviour
{
    [SerializeField] public List<Person> personChar;

    private void Start()
    {
        foreach (var person in personChar)
        {
            person.Init();
        }
    }

    public Person GetPerson()
    {
        return personChar.FirstOrDefault();
    }
}