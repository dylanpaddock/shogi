using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kifu : MonoBehaviour {
    public List<Move> moveRecord;
	// Use this for initialization
	void Start () {
        Reset();
	}

	// Update is called once per frame
	void Update () {

	}

    public void Reset(){
        moveRecord = new List<Move>();
        moveRecord.Add(null);//0th entry is empty move

    }

    public Move getLastMove(){
        return getMove(moveRecord.Count - 1);
    }

    public Move getMove(int i){
        if (i > moveRecord.Count || i < 1){
            return null;
        }
        return moveRecord[i];
    }

    public void addMove(Move move){
        moveRecord.Add(move);
    }

    public void removeMove(Move move){
        moveRecord.Remove(move);
    }

    public string toString(){
        string moves = "";
        foreach (Move move in moveRecord){
            if (move != null){
                moves += move.toString() + "\n";
            }
        }
        return moves;
    }
}
