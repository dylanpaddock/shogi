using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : Piece {

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

        // look at the board and calculate the possible moves this piece can make
    private void CalculateMoveVectors(){
        this.possibleMoves = new List<Vector2>();
        Vector2 direction = new Vector2(0, -1);
        Vector2 moveVector = direction;
        Position movePosition = currentPosition + moveVector;
        Debug.Log("position to check: (" + movePosition.x+", " + movePosition.y+")");
        int count = 0;
        while (board.isEmpty(movePosition)){
            Debug.Log("move found: (" + movePosition.x+", "+movePosition.y+")");
            this.possibleMoves.Add(moveVector);
            moveVector += direction;
            movePosition = currentPosition + moveVector;
            if (count > 10){
                Debug.Log("found infinite loop??");
                break;
            }
            count++;
        }

    }

    public override List<Vector2> getLegalMoveVectors(){
        CalculateMoveVectors();
        return base.getLegalMoveVectors();
    }
}
