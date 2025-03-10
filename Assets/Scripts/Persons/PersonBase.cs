using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Person", menuName = "Person/Create New Person")]
public class PersonBase : ScriptableObject
{
    [SerializeField] string name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    [SerializeField] PersonType type;

    //Base Stats
    [SerializeField] int maxHp;
    [SerializeField] int attack;
    [SerializeField] int defense;
    [SerializeField] int rangeAttack;
    [SerializeField] int rangeDefense;
    [SerializeField] int speed;
    [SerializeField] List<AttainableMove> attainableMoves;

    //Getters
    public string Name
    {
        get { return name; }
    }

    public string Description
    {
        get { return description; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }

    public Sprite BackSprite
    {
        get { return backSprite; }
    }

    public PersonType Type
    {
        get { return type; }
    }

    public int MaxHp
    {
        get { return maxHp; }
    }

    public int Attack
    {
        get { return attack; }
    }

    public int Defense
    {
        get { return defense; }
    }

    public int RangeAttack
    {
        get { return rangeAttack; }
    }

    public int RangeDefense
    {
        get { return rangeDefense; }
    }

    public int Speed{
        get { return speed; }
    }

    public List<AttainableMove> AttainableMoves {
        get { return attainableMoves; }
    }
}

[System.Serializable]
public class AttainableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base {
        get { return moveBase; }
    }

    public int Level {
        get { return level; }
    }
}

/*Personality Types
Ambitious > Creative > Charismatic > Analytical > Ambitious
Normal = All Types
*/

public enum PersonType
{
    None,
    Normal,
    Ambitious,
    Creative,
    Charismatic,
    Analytical
}

// 2D Float array that creates a chart and values inside of them represents a type's effectiveness towards each other.
public class TypeChart
{
    static float[][] chart =
    {
        //            NOR AMB CRE CHA ANA
        new float[] { 1f, 1f, 1f, 1f, 1f}, // NORMAL
        new float[] { 1f, 1f, 2f, 1f, 0.5f}, // AMBITIOUS
        new float[] { 1f, 0.5f, 1f, 2f, 1f}, // CREATIVE
        new float[] { 1f, 1f, 0.5f, 1f, 2f}, // CHARISMATIC
        new float[] { 1f, 2f, 1f, 0.5f, 1f}  // ANALYTICAL
    };

    // Function that looks at the 2D Float chart to see what the effectiveness between the attack and the defend type.
    // PersonType is a enum class which means it still has a int value assigned which is used for chart lookup.
    public static float GetEffectiveness(PersonType attackType, PersonType defenseType)
    {
        if (attackType == PersonType.None || defenseType == PersonType.None)
        {
            return 1;
        }

        int row = (int) attackType - 1;
        int col = (int) defenseType - 1;

        return chart[row][col];
    }
}