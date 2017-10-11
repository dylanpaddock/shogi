using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HumanPlayer : Player {
    private bool isSelecting;
    private Piece selectedPiece;
    private Move chosenMove; //only stored between updates for promotions

    private bool promotionChoice;
    private bool waitingForPlayer;
    private bool hasPromotionChoice;
    public Button PromotionButton;
    public Button DemotionButton;

	// Use this for initialization
	protected override void Start () {
        base.Start();
        Reset();
	}

	// Update is called once per frame
	protected override void Update () { //this function is a mess.
        if (turns.isTurn(this)){
            if (hasPromotionChoice){//piece is selected, but waiting for promotion choice
                if (!waitingForPlayer){
                    chosenMove.isPromotion = promotionChoice;
                    TakeTurn(selectedPiece, chosenMove);
                    selectedPiece.deselectPiece();
                    selectedPiece = null;
                    hasPromotionChoice = false;
                    PromotionButton.interactable = false;
                    DemotionButton.interactable = false;

                }
                //still waiting
            }else if (!isSelecting && Input.GetMouseButtonDown(0)){//selecting a piece
                //Debug.Log("mouse pressed");
                //see if mouse collides with an object
                int layermask = (1 << layerNumber()); //find where mouse click hits board, ignoring other team's pieces
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layermask);
                if (hit){
                    //Debug.Log("piece hit: " + hitInfo.transform.gameObject.name);
                    selectedPiece = hitInfo.transform.gameObject.GetComponent<Piece>();
                    if (selectedPiece != null){//if the object is a Piece, select it
                        isSelecting = true;
                        selectedPiece.selectPiece();
                    }
                }
            }else if (isSelecting && Input.GetMouseButtonUp(0)){//releasing a piece
                //Debug.Log("mouse released");
                isSelecting = false;//detach from mouse
                bool legalMove = false;
                int layermask = ~(1 << layerNumber()); //find where mouse click hits board, ignoring all pieces
                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity, layermask);
                if (hit && (hitInfo.transform.gameObject.GetComponent<Board>() == board || hitInfo.transform.gameObject.GetComponent<Piece>())){//piece released over board.
                    Vector2 loc = board.WorldPointToSquare(hitInfo.point);//snap to bounds
                    Position movePosition = Position.makeNew((int)loc.x, (int)loc.y);
                    if (board.isLegalMovePosition(selectedPiece, movePosition) && selectedPiece.isLegalMovePosition(movePosition)){//check if location is legal for this piece
                        legalMove = true;
                        bool canPromote = selectedPiece.canPromote(movePosition);
                        if (canPromote){Debug.Log("this piece can promote!! woopee");}
                        chosenMove = Move.makeNew(selectedPiece, movePosition, canPromote);
                        if (!board.removesCheck(chosenMove)){
                            legalMove = false;//jump down
                        }else if (canPromote && !board.forcePromotion(selectedPiece, movePosition)){
                            hasPromotionChoice = true;
                            waitingForPlayer = true;
                            PromotionButton.interactable = true;
                            DemotionButton.interactable = true;
                            //keep piece selected
                        }else{
                            Debug.Log("Found legal move. Taking it. " + chosenMove.toString());
                            TakeTurn(selectedPiece, chosenMove);
                            selectedPiece.deselectPiece();
                            selectedPiece = null;
                        }
                    }
                }
                if (!legalMove){//move is illegal, return to starting place
                    if (selectedPiece.currentPosition.isSideboard){
                        //Debug.Log("return to sideboard");
                        selectedPiece.targetLocation = selectedPiece.currentPosition.getSideboard().PieceToWorldPoint(selectedPiece);
                    }else{
                        //Debug.Log("return to board");
                        selectedPiece.targetLocation = board.PieceToWorldPoint(selectedPiece);
                    }
                    selectedPiece.deselectPiece();
                    selectedPiece = null;
                }
            }else if (isSelecting){//dragging a piece
                //
                selectedPiece.targetLocation = ScreenToWorldUtil(Input.mousePosition, 10);
            }
        }
	}

    private Vector3 ScreenToWorldUtil(Vector3 v, float z){
        z += 1 + Camera.main.transform.position.z + board.transform.localScale.z/2;
        Vector3 temp = new Vector3(v.x, v.y, -z);
        return Camera.main.ScreenToWorldPoint(temp);
    }

    public void choosePromotion(bool choice){
        waitingForPlayer = false;
        promotionChoice = choice;
    }

    public override void Reset(){
        isSelecting = false;
        selectedPiece = null;
        chosenMove = null;
        promotionChoice = false;
        waitingForPlayer = false;
        hasPromotionChoice = false;
        PromotionButton.interactable = false;
        DemotionButton.interactable = false;
    }
}
