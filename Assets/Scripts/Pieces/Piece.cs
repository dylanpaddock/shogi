using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Piece : MonoBehaviour {
    //internal states and data
    public Board board;
    protected List<Vector2> possibleMoves;
    protected bool canPromote = false;
    public  bool isPromoted {get; protected set;}
    protected Player currentPlayer; //player controlling this piece, changes on capture/reset
    public Position currentPosition {get; protected set;} //where this piece is now or is moving to
    public Player owner; //initialization and reset
    public Position startPosition;//initialization and reset
    //movement controls
    public static int moveDuration = 2;//time for movement to complete
    protected bool isMoving = false;
    protected bool isPromoting = false;
    protected bool isChangingSides = false;
    protected int movingSpeed; //fixed
    protected Vector3 worldLocation; //where the piece moves toward
    //visuals
    protected bool isSelected = false;



	// Use this for initialization
	protected virtual void Start () {
        //initialization depends on individual piece. override.
        currentPosition = startPosition;
	}

	// Update is called once per frame
	protected void Update () {//same for all pieces
        //control movement: if flags set, move
        if (isMoving){
            //move to location
        }
        if (isPromoting){
            //rotate
        }
        if (isSelected){
            //show selection effect
        }
        if (isChangingSides){//orientation by player, align piece and player directions

        }
        //check selection w/ raycast

	}

    //finds all legal positions this piece can move to
    public List<Vector2> getLegalMoves(){
        List<Vector2> legalMoves = new List<Vector2>();
        foreach (Vector2 possibleMove in possibleMoves){
            if (Board.isLegalPosition(this, currentPosition + possibleMove)){
                legalMoves.Add(possibleMove);
            }
        }
        return legalMoves;
    }

    //tells if this piece can make this move.
    public bool isLegalMove(Vector2 move){
        foreach (Vector2 legalMove in getLegalMoves()){
            if (move == legalMove){
                return true;
            }
        }
        return false;
    }

    //move this piece to the specified position
    //does not check if move is legal on the current board
    //does not change external representation
    protected void moveToPosition(Position pos){
        //set moving flags, target location
        isMoving = true;
        currentPosition = pos;
        worldLocation = pos.getWorldPosition();
        //update board w/ board.makeMove

    }

    //move this piece according to the specified move
    public void makeMove(Move move){
        //set moving flags, target location
        //update board w/ board.makeMove
        //update internal with moveToPosition
    }

    //flip the piece over
    public void Promote(){
        if (canPromote){
            //set moving flags for flip
            isPromoted = true;
            //update possible moves in subclass (depends on piece type)
        }
    }

    public void Demote(){
        if (isPromoted){
            //set flags for flip
        }
        isPromoted = false;
        //reset possible moves
    }


    //return piece to starting position:
    //unpromoted, in starting position, original owner
    public void Reset(){
        Demote();
        //restore control to owner
        currentPlayer = owner;
        isChangingSides = true;
        if (currentPosition.isSideboard){
            currentPosition.getSideboard().removePiece(this);
        }else{
            board.removePiece(this);
            board.placePiece(this, startPosition);
        }
        currentPosition = startPosition;
        isMoving = true;
        worldLocation = startPosition.getWorldPosition();
        //remove from old position in board/sideboard
        //return to start position on board+internally
    }

    public void select(){
        isSelected = true;
        //enable highlight
    }

    public void deselect(){
        isSelected = false;
        //disable highlight
    }

    public abstract string toString();

}
