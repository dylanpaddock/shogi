using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {
    public bool isKingGeneral {get; protected set;}
	// Use this for initialization
	protected override void Start () {
        possibleMoves = new List<Vector2>();
        possibleMoves.Add(new Vector2(1,0));
        possibleMoves.Add(new Vector2(1,1));
        possibleMoves.Add(new Vector2(0,1));
        possibleMoves.Add(new Vector2(0,-1));
        possibleMoves.Add(new Vector2(-1,1));
        possibleMoves.Add(new Vector2(-1,0));
        possibleMoves.Add(new Vector2(-1,-1));
        possibleMoves.Add(new Vector2(0,-1));

        canPromote = false;
        isPromoted = false;

        if (owner.playerName == Player.Name.BLACK){
            isKingGeneral = true;
            startPosition = Position.makeNew(5,1);
        }else{
            isKingGeneral = false;
            startPosition = Position.makeNew(9,1);
        }
        Mesh kingMesh = MakeMesh(5);
        GetComponent<MeshFilter>().mesh = kingMesh;
        GetComponent<MeshCollider>().sharedMesh = kingMesh;


        base.Start();
    }

    // Update is called once per frame
    public override string toString(){
        return "K";
    }
}
