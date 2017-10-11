using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour {
    private Piece[,] pieceLayout; // internal board representation
    public List<Sideboard> sideboards;
    public List<Piece> pieceList;
    public Kifu kifu;
    public int numRows {get; protected set;}
    public int numCols {get; protected set;}
    public Turns turns;

    public Text winnerText;


	// Use this for initialization
	void Awake () {
        numRows = 9;
        numCols = 9;
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
        if(!isEmpty(pos)){
            return getPiece(pos).currentPlayer != piece.currentPlayer;
        }

        return true;
    }

    public bool isValidPosition(Position pos){
        return !(pos == null || pos.isSideboard || pos.x < 1 || pos.x > 9 || pos.y < 1 || pos.y > 9);
    }

    public bool isLegalDropPosition(Piece piece, Position pos){
        if (piece == null || !isValidPosition(pos)){//bad inputs
            return false;
        }
        if (forcePromotion(piece, pos) || !isEmpty(pos)){ //occupied spot or no legal moves
            return false;
        }
        return true;
    }

    public bool forcePromotion(Piece piece, Position pos){
        //can't drop/move certain pieces where they can't move after
        if(!isValidPosition(pos)){
            return false;
        }
        //missing: deal with piece types: pawn, lance, knight
        if (piece is Pawn || piece is Lance){
            return (piece.currentPlayer.isPlayerOne() && pos.y == 1) ||
            (!piece.currentPlayer.isPlayerOne() && pos.y == numRows);//can't be in last row
        }else if (piece is Knight){
            return (piece.currentPlayer.isPlayerOne() && pos.y < 3) ||
            (!piece.currentPlayer.isPlayerOne() && pos.y > numRows - 2);
        }
        return false;
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
        turns.ResetPlayers();
        turns.StartGame();
        winnerText.gameObject.SetActive(false);
    }

    void placePiece(Piece piece, Vector2 move){
        //
        placePiece(piece, piece.currentPosition + move);
    }

    public void placePiece(Piece piece, Position position){
        //update the board layout only. doesn't affect external objects
        //if (!piece.currentPlayer.isPlayerOne()){
        //    position.y = numRows - position.y + 1;
        //}
        if (!isLegalMovePosition(piece, position)){
            Debug.Log("Tried moving a piece to an illegal position: (" +position.x+ ", " +position.y+ ")");
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
            Debug.LogError("Error: can't find your piece in an imaginary place" );
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
        float newz = scale.z/2f + 1f;

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

    public bool isCheck(Player p){//player p's king is in check
        //find p's king
        //check row, col, 8 around,
        Position kingPos = Position.makeNew(p.sideboard);//dummy assignment
        for (int x = 1; x < numCols; x++){
            for (int y = 1; y < numRows; y++){
                if (pieceLayout[x, y] is King && pieceLayout[x, y].currentPlayer == p){
                    kingPos = Position.makeNew(x, y);
                }
            }
        }
        foreach (Piece piece in pieceLayout){
            if (piece != null && piece.currentPlayer != p && piece.isLegalMovePosition(kingPos)){// any of op's pieces can move to king
                return true;
            }
        }
        return false;


    }

    //makes a move, looks for check, unmakes move
    public bool removesCheck(Move move){
        if (!move.startPosition.isSideboard){
            removePiece(move.piece);
        }
        Piece temp = getPiece(move.endPosition);
        placePiece(move.piece, move.endPosition);
        bool result = isCheck(move.piece.currentPlayer);
        pieceLayout[move.endPosition.x, move.endPosition.y] = temp;
        if (!move.startPosition.isSideboard){
            placePiece(move.piece, move.startPosition);
        }
        return !result;
    }

    public bool isCheckmate(Player p){
        if (!isCheck(p)){
            return false;
        }
        foreach (Piece piece in pieceList){
            if (piece.currentPlayer == p){
                foreach (Vector2 vec in piece.getLegalMoveVectors()){
                    Move move = Move.makeNew(piece, piece.currentPosition + vec, false);
                    if (removesCheck(move)){
                        return false;
                    }
                }
            }
        }
        winnerText.gameObject.SetActive(true);
        winnerText.text = turns.inactivePlayer().toString() + " is the winner!";
        return true;
    }

    //finds the columns which contain a pawn.
    public List<int> getPawnFiles(Player p){
        List<int> pawnList = new List<int>();
        for (int x = 1; x <= 9; x++){
            for (int y = 1; y <= 9; y++){
                Piece piece = pieceLayout[x, y];
                if (piece is Pawn && piece.currentPlayer == p && !piece.isPromoted){
                    pawnList.Add(x);
                    break;
                }

            }

        }
        return pawnList;
    }
}
