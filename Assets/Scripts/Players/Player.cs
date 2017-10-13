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
    public Vector3 targetRotation {get; protected set;}
    // Use this for initialization

    protected void Awake () {
        targetRotation = (isPlayerOne()) ? new Vector3(5, 0, 0) : new Vector3(-5, 0, 180);
    }

	protected virtual void Start () {
	}

	// Update is called once per frame
	protected virtual void Update () {

	}

    protected virtual void TakeTurn(Piece piece, Move move){
        kifu.addMove(move);
        piece.makeMove(move, true);
        //promotion choice
        //check for check + game end
        if (board.isCheckmate(turns.inactivePlayer())){
            Debug.Log("~~~IT'S CHECKMATE!!!! YOU ARE THE WEINER!!!!");
        }

//        Debug.Log("Player " + (isPlayerOne() ? "one" : "two") + " is in check: " + board.isCheck(this));
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

    public string toString(){
        return playerName == Name.BLACK? "Black" : "White";
    }

    public abstract void Reset();

    public Player getOpponent(){//super gross. fix encapsulation so this doesn't have to exist.
        return turns.players[0] == this? turns.players[1] : turns.players[0];
    }

}
