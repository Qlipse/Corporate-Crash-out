using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase Base { get; set; }
    public int Stamina { get; set; }
    public int Power { get; set; }

    public Move(MoveBase pBase)
    {
        Base = pBase;
        Stamina = pBase.Stamina;
        Power = pBase.Power;
    }

}
