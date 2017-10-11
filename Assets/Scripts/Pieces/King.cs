﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {

	protected override void Awake () {
        possibleMoves = new List<Vector2>();
        possibleMoves.Add(new Vector2(1,0));
        possibleMoves.Add(new Vector2(1,1));
        possibleMoves.Add(new Vector2(0,1));
        possibleMoves.Add(new Vector2(1,-1));
        possibleMoves.Add(new Vector2(-1,1));
        possibleMoves.Add(new Vector2(-1,0));
        possibleMoves.Add(new Vector2(-1,-1));
        possibleMoves.Add(new Vector2(0,-1));

        isPromoted = false;
        size = 5;
    }

    // Update is called once per frame
    public override string toString(){
        return "K";
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
