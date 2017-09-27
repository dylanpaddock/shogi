using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {
    private Piece[,] pieceLayout; // internal board representation
    public List<Sideboard> sideboards;
    public List<Piece> pieceList;
    public Kifu kifu;
    private int numRows = 9;
    private int numCols = 9;


	// Use this for initialization
	void Start () {
        pieceLayout = new Piece[1 + numCols, 1 + numRows];
        Reset();
	}

	// Update is called once per frame
	void Update () {

	}

    // tells if a given position is within the confines of the board
    // not if a given piece can move to that position
    // deal with forced promotion separately
    public bool isLegalMovePosition(Piece piece, Position pos){
        //illegal if position out of bounds or position occupied by allied piece
        if (pos.x < 1 || pos.x > 9 || pos.y < 1 || pos.y > 9
        || pieceLayout[pos.x, pos.y].currentPlayer == piece.currentPlayer){
            return false;
        }
        return true;
    }

    public bool forcePromotion(Piece piece, Position pos){
        //can't drop certain pieces where they can't move
        if(!isLegalMovePosition(piece, pos)){
            return false;
        }
        //missing: deal with piece types: pawn, lance, knight
        if (piece is Pawn || piece is Lance){
            return (piece.currentPlayer.movingUpward && pos.y == 1) ||
            (!piece.currentPlayer.movingUpward && pos.y == numRows);//can't be in last row
        }else if (piece is Knight){
            return (piece.currentPlayer.movingUpward && pos.y < 3) ||
            (!piece.currentPlayer.movingUpward && pos.y > numRows - 2);
        }
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
        //
        placePiece(piece, piece.currentPosition + move);
    }

    public void placePiece(Piece piece, Position position){
        //update the board layout only. doesn't affect external objects
        if (!isLegalMovePosition(piece, position)){
            Debug.Log("Tried moving a piece to an illegal position");
        }else{
            pieceLayout[position.x, position.y] = piece;
        }
    }

    public void removePiece(Piece piece){
        //take a piece off the board. need to put it somewhere or the reference will be lost
        pieceLayout[piece.currentPosition.x, piece.currentPosition.y] = null;
    }

    //converts the Position of a piece on the board to worldspace coordinates
    public Vector3 PieceToWorldPoint(Piece piece){
        Vector3 worldPos = new Vector3(0f,0f,(this.transform.localScale.z+piece.transform.localScale.z)/2f);
        worldPos.x = piece.currentPosition.x;
        worldPos.y = piece.currentPosition.y;
        return Vector3.zero;
    }
    //converts a square on the board to world
    public Vector3 SquareToWorldPoint(int x, int y){
        return Vector3.zero;
    }

    //converts a worldspace coordinate to a square on the board
    public Vector2 WorldPointToSquare(Vector3 point){
        return Vector2.zero;
    }

}
