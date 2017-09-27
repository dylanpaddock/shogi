using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : Player {
    bool isSelecting;
    Piece selectedPiece;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}

	// Update is called once per frame
	protected override void Update () {
        if (!isSelecting && Input.GetMouseButtonDown(0)){//selecting a piece
            Debug.Log("mouse pressed");
            //see if mouse collides with an object

            int layermask = (1 << 8); //find where mouse click hits board, ignoring pieces
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layermask);

            if (hit){
                Debug.Log("piece hit: " + hitInfo.transform.gameObject.name);
                selectedPiece = hitInfo.transform.gameObject.GetComponent<Piece>();
                if (selectedPiece != null){//if the object is a Piece, select it
                    isSelecting = true;
                    selectedPiece.selectPiece();
                }
            }
        }
        if (isSelecting){
            selectedPiece.targetLocation = ScreenToWorldUtil(Input.mousePosition, 10);
        }

        if (isSelecting && Input.GetMouseButtonUp(0)){//releasing a piece
            Debug.Log("mouse released");
            isSelecting = false;//detach from mouse
            selectedPiece.targetLocation = ScreenToWorldUtil(Input.mousePosition, 0);

            bool legalMove = false;
            int layermask = ~(1 << 8); //find where mouse click hits board, ignoring pieces
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layermask);
            if (hit && hitInfo.transform.gameObject.GetComponent<Board>() == board){//piece released on board.
                Vector2 loc = board.WorldPointToSquare(hitInfo.point);
                //snap to bounds
                Position p = Position.makeNew((int)loc.x, (int)loc.y);
                if (board.isLegalMovePosition(selectedPiece, p)){//check if location is legal for this piece
                    Debug.Log("Found legal move. Taking it. (" + p.x + ", "+p.y+")");
                    legalMove = true;
                    selectedPiece.targetLocation = board.SquareToWorldPoint((int)loc.x, (int)loc.y);
                    //missing: make move
                    selectedPiece.deselectPiece();
                }
            }

            if (!legalMove){//move is illegal, return to starting place
                if (selectedPiece.currentPosition.isSideboard){
                    Debug.Log("return to sideboard");
                    selectedPiece.targetLocation = selectedPiece.currentPosition.getSideboard().PieceToWorldPoint(selectedPiece);
                }else{
                    Debug.Log("return to board");
                    selectedPiece.targetLocation = board.PieceToWorldPoint(selectedPiece);
                }
                selectedPiece.deselectPiece();
            }
        }

	}

    void OnGUI(){
        //missing: highlight selected piece, here? or in Piece?
    }

    Vector3 ScreenToWorldUtil(Vector3 v, float z){
        z += 1 + Camera.main.transform.position.z + board.transform.localScale.z/2;
        Vector3 temp = new Vector3(v.x, v.y, -z);
        return Camera.main.ScreenToWorldPoint(temp);
    }
}
