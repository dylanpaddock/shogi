using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : Piece {

    protected override void Awake(){
        normalMoves = new List<Vector2>();
        normalMoves.Add(new Vector2(0, -1));
        normalMoves.Add(new Vector2(1, -1));
        normalMoves.Add(new Vector2(-1, -1));
        normalMoves.Add(new Vector2(-1, 1));
        normalMoves.Add(new Vector2(1, 1));
        isPromoted = false;
        size = 3;

        promotedMoves = new List<Vector2>();
        promotedMoves.Add(new Vector2(0, -1));
        promotedMoves.Add(new Vector2(1, -1));
        promotedMoves.Add(new Vector2(-1, -1));
        promotedMoves.Add(new Vector2(0, 1));
        promotedMoves.Add(new Vector2(1, 0));
        promotedMoves.Add(new Vector2(-1, 0));

        possibleMoves = normalMoves;
    }

    public override string toString(){
        if (isPromoted){
            return "S+";
        }
        return "S";
    }
}
