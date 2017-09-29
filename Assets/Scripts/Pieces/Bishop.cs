using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {

    protected override void Awake(){
        isPromoted = false;
        currentPlayer = owner;
        size = 4;
    }

    public override void Promote(){

    }

    public override void Demote(){

    }

    public override string toString(){
        if (isPromoted){
            return "B+";
        }
        return "B";
    }

    // look at the board and calculate the possible moves this piece can make
    private void CalculateMoveVectors(){
        this.possibleMoves = new List<Vector2>();
        //can go up to 8 units in any diag direction
        //stop upon reaching a unit
        List<Vector2> directions = new List<Vector2>();
        directions.Add(new Vector2(1, -1));
        directions.Add(new Vector2(1, 1));
        directions.Add(new Vector2(-1, 1));
        directions.Add(new Vector2(-1, -1));
        foreach (Vector2 direction in directions){
            Vector2 moveVector = direction;
            Position movePosition = currentPosition + moveVector;
            Debug.Log("position to check: (" + movePosition.x+", " + movePosition.y+")");
            while (board.isEmpty(movePosition)){
                Debug.Log("move found: (" + movePosition.x+", "+movePosition.y+")");
                this.possibleMoves.Add(moveVector);
                moveVector += direction;
                movePosition = currentPosition + moveVector;
            }//missing: check for possibility of capture, move into occupied square
        }

    }

    public override List<Vector2> getLegalMoveVectors(){
        CalculateMoveVectors();
        return base.getLegalMoveVectors();
    }
}
