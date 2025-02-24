using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person
{
    PersonBase _base;
    int level;

    public int HP { get; set; }

    public List<Move> Moves { get; set; }

    public Person(PersonBase pBase, int pLevel)
    {
        _base = pBase;
        level = pLevel;
        HP = _base.MaxHp;

        Moves = new List<Move>();
        foreach (var move in _base.AttainableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }

            if(Moves.Count >= 4)
            {
                break;
            }
        }
    }

    //Returns the floor of (Person.Attack * level) / 100 + 5
        public int Attack {
            get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; }
        }

        public int Defense {
            get { return Mathf.FloorToInt((_base.Defense * level) / 100f) + 5; }
        }

        public int RangeAttack {
            get { return Mathf.FloorToInt((_base.RangeAttack * level) / 100f) + 5; }
        }

        public int RangeDefense {
            get { return Mathf.FloorToInt((_base.RangeDefense * level) / 100f) + 5; }
        }

        public int Speed {
            get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; }
        }

        public int MaxHp {
            get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 10; }
        }
}