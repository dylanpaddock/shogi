using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Move : ScriptableObject {
    public int number;
    public Piece piece;
    public Position startPosition {get; set;}
    public Position endPosition {get; set;}
    public enum MoveType {simple, capture, drop}
    public MoveType type;
    public bool isPromotion;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public static Move makeNew(Position start, Position end, MoveType t, bool p){
        Move newMove = ScriptableObject.CreateInstance<Move>();
        newMove.startPosition = start;
        if (end.isSideboard){
            Debug.Log("creating impossible move");
        }
        newMove.endPosition = end;
        newMove.type = t;
        newMove.isPromotion = p;
        return newMove;
    }

    public string toString(){
        string turn = number + ". ";
        string pieceName = piece.toString();
        string connector;
        if (type == MoveType.simple) {
            connector = "-";
        }else if (type == MoveType.capture){
            connector = "x";
        }else{ //(type == MoveType.drop) drop move starts from sideboard
            connector = "*";
            return turn + pieceName + connector + endPosition.toString();
        }

        string promotion = isPromotion? "+" : "";
        return turn + pieceName + startPosition.toString()+ connector + promotion + endPosition.toString();
    }
}
