using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sideboard : MonoBehaviour {
    private List<Piece> pieceList;
    public Player owner;
	// Use this for initialization
	void Start () {
        Reset();
	}

	// Update is called once per frame
	void Update () {

	}

    public void Reset() {
        pieceList = new List<Piece>();
    }

    public void addPiece(Piece piece){
        pieceList.Add(piece);
        piece.Demote();
    }

    public void removePiece(Piece piece){//dropping a piece somewhere or resetting
        pieceList.Remove(piece);
    }

    //converts a Position on the board to world space coordinates
    public Vector3 PieceToWorldPoint(Piece piece){
        //figure out how to lay out the pieces on the sideboard.
        //by type/size, stack like pieces?
        //theoretically needs to fit all pieces from both sides: 40 total
        return Vector3.zero;
    }
}
