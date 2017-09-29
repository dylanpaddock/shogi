using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Move : ScriptableObject {
    public int number;
    public Piece piece;
    public Position startPosition {get; set;}
    public Position endPosition {get; set;}
    public enum Type {simple, capture, drop}
    public Type type;
    public bool isPromotion;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public static Move makeNew(Piece piece, Position end, bool p){
        Move newMove = ScriptableObject.CreateInstance<Move>();
        newMove.piece = piece;
        newMove.startPosition = piece.currentPosition;
        if (end.isSideboard){
            Debug.Log("creating impossible move");
        }
        newMove.endPosition = end;
        newMove.isPromotion = p;

        if (piece.currentPosition.isSideboard){
            newMove.type = Type.drop;
        }else if (!piece.board.isEmpty(end)){
            newMove.type = Type.capture;
        }else{
            newMove.type = Type.simple;
        }
        return newMove;
    }

    public string toString(){
        string turn = number + ". ";
        string pieceName = piece.toString();
        string connector;
        if (type == Type.simple) {
            connector = "-";
        }else if (type == Type.capture){
            connector = "x";
        }else{ //(type == Type.drop) drop move starts from sideboard
            connector = "*";
            return turn + pieceName + connector + endPosition.toString();
        }

        string promotion = isPromotion? "+" : "";
        return turn + pieceName + startPosition.toString()+ connector + promotion + endPosition.toString();
    }
}
