using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private Piece[,] pieceLayout; // internal board representation
    public List<Sideboard> sideboards;
    public List<Piece> pieceList;
    public Kifu kifu;


	// Use this for initialization
	void Start () {
        pieceLayout = new Piece[10,10];
        Reset();
	}

	// Update is called once per frame
	void Update () {

	}

    // tells if a given position is within the confines of the board
    // not if a given piece can move to that position
    public static bool isLegalPosition(Piece piece, Position position){
        return true;
    }

    //start a new game, putting all pieces to their correct positions
    //take the game components and reset them
    void Reset(){
        foreach(Piece p in pieceList){
            p.Reset();
            pieceLayout[p.currentPosition.x, p.currentPosition.y] = null;
            pieceLayout[p.startPosition.x, p.startPosition.y] = p;
        }
//        foreach (Sideboard sideboard in sideboards){
//            sideboard.Reset();
//        }
        kifu.Reset();
    }

    void placePiece(Piece piece, Vector2 move){
        placePiece(piece, piece.currentPosition + move);
    }

    //update the board layout only. doesn't affect external objects
    public void placePiece(Piece piece, Position position){
        if (!Board.isLegalPosition(piece, position)){
            Debug.Log("Tried moving a piece to an illegal position");
        }else{
            pieceLayout[position.x, position.y] = piece;
        }
    }

    public void removePiece(Piece piece){
        pieceLayout[piece.currentPosition.x, piece.currentPosition.y] = null;
    }
}
