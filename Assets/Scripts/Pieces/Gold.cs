using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : Piece {

    protected override void Awake(){
        possibleMoves = new List<Vector2>();
        possibleMoves.Add(new Vector2(0, -1));
        possibleMoves.Add(new Vector2(1, -1));
        possibleMoves.Add(new Vector2(-1, -1));
        possibleMoves.Add(new Vector2(0, 1));
        possibleMoves.Add(new Vector2(1, 0));
        possibleMoves.Add(new Vector2(-1, 0));

        isPromoted = false;
        size = 3;
    }

    public override string toString(){
        return "G";
    }

    public override void Promote(){
        //do nothing
    }

    public override void Demote(){
        //do nothing
    }

    public override bool canPromote(Position movePos){//override for King, Gold,
        return false;
    }

}
