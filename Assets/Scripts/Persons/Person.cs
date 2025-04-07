using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class Person
{
    [SerializeField] PersonBase _base;
    [SerializeField] int level;

     // getters and setters for variables
    public PersonBase Base {
        get {
            return _base;
        }
    }

    public int Level {
        get {
            return level;
        }
    }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    // Init initializes the persons HP and move lists.
    public void Init()
    {
        HP = MaxHp;

        Moves = new List<Move>();
        foreach (var move in Base.AttainableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }

            if(Moves.Count >= 4)
            {
                break;
            }
        }
    }

    //Returns the floor of (Person.Attack * Level) / 100 + 5
        public int Attack {
            get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
        }

        public int Defense {
            get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
        }

        public int RangeAttack {
            get { return Mathf.FloorToInt((Base.RangeAttack * Level) / 100f) + 5; }
        }

        public int RangeDefense {
            get { return Mathf.FloorToInt((Base.RangeDefense * Level) / 100f) + 5; }
        }

        public int Speed {
            get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; }
        }

        public int MaxHp {
            get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 10; }
        }

        // Calculates damage taken given a move.
        // Returns an DamageDetails object that contains whether or not the person fainted, if the move was a critical, and if it was an effective move.
        // modifiers is for outside factors that affect how much a move does. By default, a move does 85% to 100% full damage.
        public DamageDetails TakeDamage(Move move, Person attacker)
        {
            float critical = 1f;
            if (Random.value * 100f <= 6.25f)
            {
                critical = 2f;
            }
            float type = TypeChart.GetEffectiveness(move.Base.Type, this.Base.Type);

            var damageDetails = new DamageDetails()
            {
                TypeEffectiveness = type,
                Critical = critical,
                Fainted = false
            };

            float modifiers = Random.Range(0.85f, 1f) * type * critical;
            float a = (2 * attacker.Level + 10) / 250f;
            float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
            int damage = Mathf.FloorToInt(d * modifiers);

            HP -= damage;
            if (HP <= 0)
            {
                HP = 0;
                damageDetails.Fainted = true;
            }

            return damageDetails;
        }

        // Picks random move
        public Move GetRandomMove()
        {
            int r = Random.Range(0, Moves.Count);
            return Moves[r];
        }
}

//  Object that stores 3 values that are to be used in the TakeDamage function.
public class DamageDetails
{
    public bool Fainted { get; set; }
    public float Critical { get; set; }
    public float TypeEffectiveness { get; set; }
}