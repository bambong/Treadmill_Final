using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDist
{
    public double movedDist { get; private set; }

    public void MoveDistUpdate()
    {
        movedDist += Managers.Token.CurrentMoveMeter;
    }
    public string GetString()
    {
        return string.Format("{0:0.00}", movedDist);
    }

    public MoveDist()
    {
        movedDist = 0;
    }
}
