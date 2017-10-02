using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sideboard : MonoBehaviour {
    private List<Piece> pieceList;
    public Player owner;
    //piece layout
    int numRows;
	// Use this for initialization
	void Awake () {
        Reset();
	}

    void Start(){

    }

	// Update is called once per frame
	void Update () {

	}

    public void Reset() {
        pieceList = new List<Piece>();
        numRows = 1;
    }

    public void addPiece(Piece piece){
        int index = 0;
        for (int i=0; i< pieceList.Count; i++){
            if (true/*is smaller*/){
                //figure out where to put the thing in the list
                index = 1;
            }
        }
        //add in sorted order, from big to small: K, R, B, G, S, N, L, P
        pieceList.Insert(index, piece);//missing: sort by size
        piece.Demote();
        numRows =  Mathf.FloorToInt(Mathf.Sqrt(pieceList.Count));
        rearrange();
    }

    public void removePiece(Piece piece){//dropping a piece somewhere or resetting
        pieceList.Remove(piece);
        numRows =  Mathf.FloorToInt(Mathf.Sqrt(pieceList.Count + 1));
        rearrange();

    }

    public Position getPosition(){
        return Position.makeNew(this);
    }


    //converts a Position on the board to world space coordinates
    public Vector3 PieceToWorldPoint(Piece piece){
        //figure out how to lay out the pieces on the sideboard.
        //by type/size, stack like pieces?
        //theoretically needs to fit all pieces from both sides: 40 total
        Vector3 newPos = this.transform.position;
        Vector3 scale = this.transform.localScale;
        int index = pieceList.FindIndex(item => item==piece);
        //Debug.Log("index: "+index);
        int rowSize = (pieceList.Count - 1)/numRows + 1;
        int x = (index % rowSize) + 1;
        int y = (index / rowSize) + 1;
        newPos.x += scale.x*(x/(1f+rowSize) - .5f);
        newPos.y += scale.y*(.5f - y/(1f+numRows));
        newPos.z += -10;
        return newPos;
    }

    private void rearrange(){
        foreach (Piece piece in pieceList){
            piece.targetLocation = PieceToWorldPoint(piece);
        }
    }
}
