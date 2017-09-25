using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour {
    public int playerNumber;//player one or player 2. player one gets the King General
    public enum Name {BLACK, WHITE};//black goes first
    public Name playerName;
    public Board board;
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
        turns.passTurn();
    }
}
