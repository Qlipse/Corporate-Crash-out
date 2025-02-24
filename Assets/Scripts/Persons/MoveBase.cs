using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Move", menuName = "Person/Create new move")]

public class MoveBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] PersonType type;
    [SerializeField] int power;
    [SerializeField] int chance;
    [SerializeField] int stamina;

    public string Name {
        get { return name; }
    }

    public string Description {
        get { return description; }
    }

    public PersonType Type {
        get { return type; }
    }

    public int Power {
        get { return power; }
    }
    
    public int Chance {
        get { return chance; }
    }

    public int Stamina {
        get { return stamina; }
    }
}
