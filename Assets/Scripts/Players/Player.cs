using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour {
   public enum Name {BLACK, WHITE};//black goes first
    public Name playerName;
    public Board board;
    public Sideboard sideboard;
    public Kifu kifu;
    public Turns turns;
    // Use this for initialization
	protected virtual void Start () {

	}

	// Update is called once per frame
	protected virtual void Update () {

	}

    protected virtual void TakeTurn(Piece piece, Move move){
        kifu.addMove(move);
        piece.makeMove(move);
        //promotion choice
        turns.passTurn();
    }

    public bool isPlayerOne(){//true if player is black, i.e. plays first
        return playerName == Name.BLACK;
    }

    public int layerNumber(){
        if (playerName == Name.BLACK){
            return 8;
        }else{
            return 9;
        }
    }

}
