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
	void Awake () {
        pieceLayout = new Piece[1 + numCols, 1 + numRows];
    }

    void Start(){

    }
	// Update is called once per frame
	void Update () {

	}

    // tells if a given position is within the confines of the board
    // not if a given piece can move to that position
    // deal with forced promotion separately
    public bool isLegalMovePosition(Piece piece, Position pos){
        //illegal if position out of bounds or position occupied by allied piece
        if (piece == null || !isValidPosition(pos)){
            return false;
        }
        if(pieceLayout[pos.x, pos.y] != null){
            return pieceLayout[pos.x, pos.y].currentPlayer != piece.currentPlayer;
        }

        return true;
    }

    public bool isValidPosition(Position pos){
        return !(pos == null || pos.isSideboard || pos.x < 1 || pos.x > 9 || pos.y < 1 || pos.y > 9);
    }

    public bool isLegalDropPosition(Piece piece, Position pos){
        if (forcePromotion(piece, pos) || pieceLayout[pos.x, pos.y] != null){ //illegal to drop some pieces to end rows, or on another piece
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
    public void Reset(){
        foreach(Piece p in pieceList){
            p.Reset();
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
        //if (!piece.currentPlayer.movingUpward){
        //    position.y = numRows - position.y + 1;
        //}
        if (!isLegalMovePosition(piece, position)){
            Debug.Log("Tried moving a piece to an illegal position");
        }
        pieceLayout[position.x, position.y] = piece;

    }

    public bool isEmpty(Position pos){//checks if a square is both valid and empty
        if (!isValidPosition(pos)){
            Debug.Log("position (" +pos.x+", "+pos.y+") is invalid. Can't get a piece there.");
            return false;
        }
        return pieceLayout[pos.x, pos.y] == null;
    }

    public Piece getPiece(Position pos){
        if(!isValidPosition(pos)){
            Debug.LogError("Error: can't find you piece in an imaginary place");
        }
        return pieceLayout[pos.x, pos.y];

    }

    public void removePiece(Piece piece){
        //take a piece off the board. need to put it somewhere or the reference will be lost
        pieceLayout[piece.currentPosition.x, piece.currentPosition.y] = null;
    }

    public Vector3 PieceToWorldPoint(Piece piece){
        //converts the Position of a piece on the board to worldspace coordinates
        return SquareToWorldPoint(piece.currentPosition.x, piece.currentPosition.y);
    }


    public Vector3 SquareToWorldPoint(int x, int y){
        //converts a square on the board to worldspace coordinates
        //hard coded for current board scaling. 100x110
        Vector3 scale = this.transform.localScale;
        float newx = scale.x/2f - x*scale.x/10f;
        float newy = scale.y/2f - y*scale.y/10f;
        float newz = scale.z/2f;

        return new Vector3(-newx, -newy, -newz);
    }

    //converts a worldspace coordinate to a square on the board, ignoring z-coordinate
    public Vector2 WorldPointToSquare(Vector3 point){
        int x = Mathf.RoundToInt(5 - point.x/10f);
        int y = Mathf.RoundToInt(5 - point.y/11f);
        if (x < 1 || x > 9 || y < 1 || y > 9){
            Debug.Log("this position is not a legal square on the board");
        }
        return new Vector2(x, y);
    }

    public string toString(){
        string s = "";
        for (int i = 1; i<pieceLayout.GetLength(0); i++){
            string n = "";
            for (int j = 1; j<pieceLayout.GetLength(1); j++){
                if (pieceLayout[j,i] == null){
                    n = "_" + n;
                }else{
                    n = pieceLayout[j,i].toString() + n;
                }
            }
            s += n + "\n";
        }
        return s;
    }

}
