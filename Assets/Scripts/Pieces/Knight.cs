using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    protected override void Awake(){
        normalMoves = new List<Vector2>();
        normalMoves.Add(new Vector2(1, -2));
        normalMoves.Add(new Vector2(-1, -2));
        isPromoted = false;
        size = 2;

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
            return "N+";
        }
        return "N";
    }
}
