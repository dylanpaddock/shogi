using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Piece : MonoBehaviour {
    //internal states and data
    public Board board;
    protected List<Vector2> possibleMoves;

    protected bool canPromote = false;
    public  bool isPromoted {get; protected set;}
    public Player currentPlayer {get; set;} //player controlling this piece, changes on capture/reset
    public Position currentPosition {get; protected set;} //where this piece is now or is moving to
    public Player owner; //initialization and reset
    public Position startPosition;//initialization and reset
    //movement controls
    public static int moveDuration = 2;//time for movement to complete
    protected bool isMoving = false;
    protected bool isPromoting = false;
    protected bool isChangingSides = false;
    protected float movingSpeed = .5f; //fixed
    protected int rotationSpeed; //fixed
    protected Vector3 _targetLocation;
    public Vector3 targetLocation {//where the piece moves toward in world
        get{
            return _targetLocation;
        }
        set{
            isMoving = true;
            _targetLocation = value;
        }
    }
    public Quaternion targetRotation {get; set;}


    //visuals
    protected bool isSelected = false;



	// Use this for initialization
	protected virtual void Start () {
        //initialization depends on individual piece. override.
        currentPosition = startPosition;
        //set layer according to player
	}

	// Update is called once per frame
	protected void Update () {//same for all pieces
        //control movement: if flags set, move
        if (isMoving){
            Vector3 error = targetLocation - transform.position;
            if (error.magnitude < movingSpeed){
                transform.position = targetLocation;
                isMoving = false;
            }else{
                transform.position += movingSpeed*(error.normalized);
            }
        }
        if (isPromoting){
            //missing: rotate for promotion
        }
        if (isSelected){
            //missing: show selection effect
        }
        if (isChangingSides){//orientation by player, align piece and player directions
            //missing
        }
        //check selection w/ raycast

	}

    //finds all legal positions this piece can move to
    public List<Vector2> getLegalMoveVectors(){
        List<Vector2> legalMoves = new List<Vector2>();
        foreach (Vector2 possibleMove in possibleMoves){
            if (board.isLegalMovePosition(this, currentPosition + possibleMove)){
                legalMoves.Add(possibleMove);
            }
        }
        return legalMoves;
    }

    //tells if this piece can move according to this vector.
    public bool isLegalVector(Vector2 move){
        foreach (Vector2 legalMove in getLegalMoveVectors()){
            if (move == legalMove){
                return true;
            }
        }
        return false;
    }

    //move this piece to the specified position
    //does not check if move is legal on the current board
    //does not change external representation
    protected void moveToPosition(Position pos){
        //set moving flags, target location
        isMoving = true;
        currentPosition = pos;
        targetLocation = pos.getWorldPosition();
        board.placePiece(this, pos);
    }

    //move this piece according to the specified move
    public void makeMove(Move move){
        //missing: check for bad inputs, validate information in Move object
        if (move.startPosition != currentPosition || move.piece != this || board.isLegalMovePosition(this, move.endPosition)){
            Debug.Log("There's something wrong with the move you made");
        }
        moveToPosition(move.endPosition);
    }

    //flip the piece over
    public void Promote(){
        if (canPromote){
            isPromoted = true;
            isPromoting = true;
            targetRotation = Quaternion.FromToRotation(Vector3.up, Vector3.down);
            //update possible moves in subclass (depends on piece type)
        }
    }

    public void Demote(){
        if (isPromoted){
            //set flags for flip
        }
        isPromoted = false;
        //reset possible moves
    }


    public void Reset(){
        //return piece to starting position: unpromoted, in starting position, original owner
        Demote();
        //restore control to owner
        currentPlayer = owner;
        isChangingSides = true;
        //remove from old position in board/sideboard
        if (currentPosition.isSideboard){
            currentPosition.getSideboard().removePiece(this);
        }else{
            board.removePiece(this);

        }
        board.placePiece(this, startPosition);
        currentPosition = startPosition; //return to start position on board+internally
        isMoving = true;
        targetLocation = startPosition.getWorldPosition();

    }

    public void selectPiece(){
        isSelected = true;
        //missing: enable highlight?
    }

    public void deselectPiece(){
        isSelected = false;
        //missing: disable highlight?
    }

    protected Mesh MakeMesh(int pieceSize){
        //size: K = 5; R,B = 4; G,S = 3; Kn = 2, L = 1; P = 0;

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[30];
        //front
        vertices[0] = new Vector3(0-5.1875f, 0-5.5625f, 0-1.75f);
        vertices[1] = new Vector3(1f-5.1875f, 9.375f-5.5625f, .8125f-1.75f);
        vertices[2] = new Vector3(5.1875f-5.1875f, 11.125f-5.5625f, 1f-1.75f);
        vertices[3] = new Vector3(9.375f-5.1875f, 9.375f-5.5625f, .8125f-1.75f);
        vertices[4] = new Vector3(10.375f-5.1875f, 0-5.5625f, 0-1.75f);
        //back
        vertices[5] = new Vector3(-5.1875f, 0-5.5625f, 3.5f-1.75f);
        vertices[6] = new Vector3(1f-5.1875f, 9.375f-5.5625f, 2.6875f-1.75f);
        vertices[7] = new Vector3(5.1875f-5.1875f, 11.125f-5.5625f, 2.5f-1.75f);
        vertices[8] = new Vector3(9.375f-5.1875f, 9.375f-5.5625f, 2.6875f-1.75f);
        vertices[9] = new Vector3(10.375f-5.1875f, 0-5.5625f, 3.5f-1.75f);

        float vertScale = (27f + pieceSize)/32f;
        float horizScale = 22.5f/28.7f + .2f*pieceSize*(1 - 22.5f/28.7f);
        float depthScale = 7.75f/9.7f + .2f*pieceSize*(1 - 7.75f/9.7f);
        Debug.Log("scales x: "+horizScale+", y: "+vertScale+", z: "+depthScale);
        for (int i = 0; i < 10; i++){
            vertices[i] = new Vector3(vertices[i].x*.75f*horizScale,
                                      vertices[i].y*.75f*vertScale,
                                      vertices[i].z*.75f*depthScale);
        }

        //bottom left
        vertices[10] = vertices[5];
        vertices[11] = vertices[6];
        vertices[12] = vertices[1];
        vertices[13] = vertices[0];
        //top left
        vertices[14] = vertices[6];
        vertices[15] = vertices[7];
        vertices[16] = vertices[2];
        vertices[17] = vertices[1];
        //top right
        vertices[18] = vertices[3];
        vertices[19] = vertices[2];
        vertices[20] = vertices[7];
        vertices[21] = vertices[8];
        //bottom right
        vertices[22] = vertices[4];
        vertices[23] = vertices[3];
        vertices[24] = vertices[8];
        vertices[25] = vertices[9];
        //base
        vertices[26] = vertices[5];
        vertices[27] = vertices[0];
        vertices[28] = vertices[4];
        vertices[29] = vertices[9];
        mesh.vertices = vertices;

        int[] triangles = new int[16*3];
        //front
        triangles[0] = 1; triangles[1] = 2; triangles[2] = 3;
        triangles[3] = 0; triangles[4] = 1; triangles[5] = 3;
        triangles[6] = 0; triangles[7] = 3; triangles[8] = 4;
        //left
        triangles[9] = 10; triangles[10] = 12; triangles[11] = 13;
        triangles[12] = 10; triangles[13] = 11; triangles[14] = 12;
        triangles[15] = 14; triangles[16] = 16; triangles[17] = 17;
        triangles[18] = 14; triangles[19] = 15; triangles[20] = 16;
        //right
        triangles[21] = 18; triangles[22] = 19; triangles[23] = 20;
        triangles[24] = 18; triangles[25] = 20; triangles[26] = 21;
        triangles[27] = 22; triangles[28] = 23; triangles[29] = 24;
        triangles[30] = 22; triangles[31] = 24; triangles[32] = 25;
        //base
        triangles[33] = 26; triangles[34] = 28; triangles[35] = 29;
        triangles[36] = 26; triangles[37] = 27; triangles[38] = 28;
        //back
        triangles[39] = 8; triangles[40] = 7; triangles[41] = 6;
        triangles[42] = 9; triangles[43] = 8; triangles[44] = 6;
        triangles[45] = 9; triangles[46] = 6; triangles[47] = 5;

        mesh.triangles = triangles;

        Vector2[] uv = new Vector2[30];
        mesh.uv = uv;

        mesh.RecalculateNormals();

        return mesh;
    }


    public abstract string toString();

}
