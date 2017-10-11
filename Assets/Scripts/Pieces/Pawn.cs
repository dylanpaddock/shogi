using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pawn : Piece {

	protected override void Awake(){
        normalMoves = new List<Vector2>();
        normalMoves.Add(new Vector2(0, -1));
        isPromoted = false;
        size = 0;

        promotedMoves = new List<Vector2>();
        promotedMoves.Add(new Vector2(0, -1));
        promotedMoves.Add(new Vector2(1, -1));
        promotedMoves.Add(new Vector2(-1, -1));
        promotedMoves.Add(new Vector2(0, 1));
        promotedMoves.Add(new Vector2(1, 0));
        promotedMoves.Add(new Vector2(-1, 0));

        possibleMoves = normalMoves;
    }

    public override string toString(){
        if (isPromoted){
            return "+P";
        }
        return "P";
    }

    protected override void CalculateDropPositions(){
        //account for pawn drop restrictions: no checkmate, only 1 in file
        this.possibleDrops = new List<Vector2>();
        List<int> files = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
        files = files.Except(board.getPawnFiles(currentPlayer)).ToList();
        foreach (int x in files){
            bool p1 = currentPlayer.isPlayerOne();
            int ymin = p1 ? 2 : 1; //can't drop a pawn on back row
            int ymax = p1 ? 9 : 8;
            for (int y = ymin; y <= ymax; y++){
                Position pos = Position.makeNew(x, y); //created and thrown away... inefficient
                //Debug.Log("Position: "+pos.toString()+ " legal: "+ board.isLegalDropPosition(this, pos));
                if (board.isLegalDropPosition(this, pos)){
                    //if pawn is at king's head, check if player has options
                    Position head = Position.makeNew(x, y + (p1? -1 : 1));
                    if (!board.isEmpty(head) && board.getPiece(head) is King &&
                        board.getPiece(head).currentPlayer != this.currentPlayer){
                        //check for mate
                    }
                    this.possibleDrops.Add(new Vector2(x, y));
                }
            }
        }
    }
}
