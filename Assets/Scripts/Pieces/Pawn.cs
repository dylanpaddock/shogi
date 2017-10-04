using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {

	protected override void Awake(){
        possibleMoves = new List<Vector2>();
        possibleMoves.Add(new Vector2(0, -1));
        isPromoted = false;
        size = 0;
    }

    public override string toString(){
        if (isPromoted){
            return "+P";
        }
        return "P";
    }
}
