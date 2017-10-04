using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : Piece {

    protected override void Awake(){
        possibleMoves = new List<Vector2>();
        possibleMoves.Add(new Vector2(0, -1));
        possibleMoves.Add(new Vector2(1, -1));
        possibleMoves.Add(new Vector2(-1, -1));
        possibleMoves.Add(new Vector2(-1, 1));
        possibleMoves.Add(new Vector2(1, 1));
        isPromoted = false;
        size = 3;
    }

    public override void Promote(){

    }

    public override void Demote(){

    }

    public override string toString(){
        if (isPromoted){
            return "S+";
        }
        return "S";
    }
}
