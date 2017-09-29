using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    protected override void Awake(){
        possibleMoves = new List<Vector2>();
        possibleMoves.Add(new Vector2(1, -2));
        possibleMoves.Add(new Vector2(-1, -2));
        isPromoted = false;
        currentPlayer = owner;
        size = 2;
    }

    public override void Promote(){

    }

    public override void Demote(){

    }

    public override string toString(){
        if (isPromoted){
            return "N+";
        }
        return "N";
    }
}
