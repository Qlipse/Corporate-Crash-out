using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapArea : MonoBehaviour
{
    [SerializeField] List<Person> randomPersons;

    public Person GetRandomPerson()
    {
        var randomPerson = randomPersons[Random.Range(0, randomPersons.Count)];
        randomPerson.Init();
        return randomPerson;
    }
}
