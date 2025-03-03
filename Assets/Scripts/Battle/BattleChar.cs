using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleChar : MonoBehaviour
{
    [SerializeField] PersonBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerChar;

    public Person Person { get ; set; }

    // Fetches data from connected objects through SerializeField.
    public void Setup()
    {
        Person = new Person(_base, level);
        if(isPlayerChar)
            GetComponent<Image>().sprite = Person.Base.BackSprite;
        else
            GetComponent<Image>().sprite = Person.Base.FrontSprite;
    }
}