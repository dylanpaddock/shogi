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
    public Quaternion targetRotation;
    public Player opponent;
    // Use this for initialization
    protected void Awake () {
        targetRotation.eulerAngles = (isPlayerOne()) ? new Vector3(5, 0, 0) : new Vector3(-5, 0, 180);
    }

	protected virtual void Start () {
	}

	// Update is called once per frame
	protected virtual void Update () {

	}

    protected virtual void TakeTurn(Piece piece, Move move){
        kifu.addMove(move);
        piece.makeMove(move);
        //promotion choice
        //check for check + game end
        if (board.isCheckmate(opponent)){
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

}
