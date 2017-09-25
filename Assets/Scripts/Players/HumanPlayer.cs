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
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit){
                Debug.Log("piece hit: " + hitInfo.transform.gameObject.name);
                selectedPiece = hitInfo.transform.gameObject.GetComponent<Piece>();
                if (selectedPiece != null){//if the object is a Piece, select it
                    isSelecting = true;
                }
            }
        }
        if (isSelecting){
            selectedPiece.transform.position = ScreenToWorldUtil(Input.mousePosition, 10);
        }

        if (isSelecting && Input.GetMouseButtonUp(0)){//releasing a piece
            Debug.Log("mouse released");
            isSelecting = false;//detach from mouse
            selectedPiece.transform.position = ScreenToWorldUtil(Input.mousePosition, 0);
            //if legal position:
                //snap to bounds
                //make move
            //else return to starting place
        }

	}

    void OnGUI(){
        //raycasting and selection
    }

    Vector3 ScreenToWorldUtil(Vector3 v, float z){
        z += Camera.main.transform.position.z + board.transform.localScale.z/2;
        Vector3 temp = new Vector3(v.x, v.y, -z);
        return Camera.main.ScreenToWorldPoint(temp);
    }
}
