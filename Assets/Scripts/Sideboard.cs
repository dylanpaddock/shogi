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

    }
}
