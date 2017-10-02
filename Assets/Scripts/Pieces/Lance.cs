using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : Piece {
    Vector2 direction = new Vector2(0, -1);

    protected override void Awake(){
        isPromoted = false;
        currentPlayer = owner;
        size = 1;

    }

    public override void Promote(){

    }

    public override void Demote(){

    }

    public override string toString(){
        if (isPromoted){
            return "L+";
        }
        return "L";
    }

    // look at the board and calculate the possible extended moves this piece can make, useful only for bishop, rook and lance
    protected void CalculateMoveVectors(){
        //can go up to 8 units in any diag direction
        //stop upon reaching a unit
        this.possibleMoves = new List<Vector2>();
        int directionFactor = currentPlayer.isPlayerOne() ? 1 : -1;
        Vector2 moveVector = directionFactor * direction;
        Position movePosition = currentPosition + moveVector;
        //Debug.Log("position to check: (" + movePosition.x+", " + movePosition.y+")");
        while (board.isValidPosition(movePosition) && board.isEmpty(movePosition)){//stop when hit a piece
            Debug.Log("move found: (" + movePosition.x+", "+movePosition.y+")");
            this.possibleMoves.Add(moveVector);
            moveVector += directionFactor*direction;
            movePosition = currentPosition + moveVector;
        }
        if(board.isValidPosition(movePosition) && board.getPiece(movePosition).currentPlayer != this.currentPlayer){//check for opponent's piece to capture
            //Debug.Log("move found: (" + movePosition.x+", "+movePosition.y+")");
            this.possibleMoves.Add(moveVector);
        }
    }

    public override List<Vector2> getLegalMoveVectors(){
        CalculateMoveVectors();
        List<Vector2> legalMoves = new List<Vector2>();
        foreach (Vector2 possibleMove in possibleMoves){
            if (board.isLegalMovePosition(this, currentPosition + possibleMove)){
                legalMoves.Add(possibleMove);
            }
        }
        return legalMoves;
    }
}
