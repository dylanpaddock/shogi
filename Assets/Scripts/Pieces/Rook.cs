using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece {
    List<Vector2> directions;

    protected override void Awake(){
        isPromoted = false;
        size = 4;

        directions = new List<Vector2>();
        directions.Add(new Vector2(0, 1));
        directions.Add(new Vector2(1, 0));
        directions.Add(new Vector2(0, -1));
        directions.Add(new Vector2(-1, 0));

        normalMoves = new List<Vector2>();
        promotedMoves = new List<Vector2>();
        promotedMoves.Add(new Vector2(1, -1));
        promotedMoves.Add(new Vector2(1, 1));
        promotedMoves.Add(new Vector2(-1, 1));
        promotedMoves.Add(new Vector2(-1, -1));
    }

    public override string toString(){
        if (isPromoted){
            return "R+";
        }
        return "R";
    }

        // look at the board and calculate the possible extended moves this piece can make, useful only for bishop, rook and lance
    protected void CalculateMoveVectors(){
        //can go up to 8 units in any diag direction
        //stop upon reaching a unit
        this.possibleMoves = isPromoted? new List<Vector2>(promotedMoves) : new List<Vector2>(normalMoves);
        foreach (Vector2 direction in directions){
            Vector2 moveVector = direction;
            Position movePosition = currentPosition + moveVector;
            //Debug.Log("position to check: (" + movePosition.x+", " + movePosition.y+")");
            while (board.isValidPosition(movePosition) && board.isEmpty(movePosition)){//stop when hit a piece
//                Debug.Log("move found: (" + movePosition.x+", "+movePosition.y+")");
                this.possibleMoves.Add(moveVector);
                moveVector += direction;
                movePosition = currentPosition + moveVector;
            }
            if(board.isValidPosition(movePosition) && board.getPiece(movePosition).currentPlayer != this.currentPlayer){//check for opponent's piece to capture
                //Debug.Log("move found: (" + movePosition.x+", "+movePosition.y+")");
                this.possibleMoves.Add(moveVector);
            }
        }

    }

    public override List<Vector2> getLegalMoveVectors(){
        List<Vector2> legalMoves = new List<Vector2>();
        if (currentPosition.isSideboard){
            CalculateDropPositions();
            foreach (Vector2 possibleDrop in possibleDrops){
                legalMoves.Add(possibleDrop);
            }
        }else{
            CalculateMoveVectors();
            foreach (Vector2 possibleMove in possibleMoves){
                if (board.isLegalMovePosition(this, currentPosition + possibleMove)){
                    legalMoves.Add(possibleMove);
                }
            }
        }
        return legalMoves;
    }
}
