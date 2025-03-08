using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 
public class Person
{
    // getters and setters for variables
    public PersonBase Base { get; set; }
    public int Level { get; set; }

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public Person(PersonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
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

        // Damage formula for moves. 
        // modifiers is for randomness in term alike in pokemon.
        public bool TakeDamage(Move move, Person attacker)
        {
            float modifiers = Random.Range(0.85f, 1f);
            float a = (2 * attacker.Level + 10) / 250f;
            float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
            int damage = Mathf.FloorToInt(d * modifiers);

            HP -= damage;
            if (HP <= 0)
            {
                HP = 0;
                return true;
            }

            return false;
        }

        // Picks random move
        public Move GetRandomMove()
        {
            int r = Random.Range(0, Moves.Count);
            return Moves[r];
        }
}