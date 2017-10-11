using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Piece : MonoBehaviour {
    //internal states and data
    public Board board;
    protected List<Vector2> possibleMoves;
    protected List<Vector2> possibleDrops = new List<Vector2>();
    protected List<Vector2> normalMoves;
    protected List<Vector2> promotedMoves;

    public  bool isPromoted {get; set;}
    private Player _currentPlayer;
    public Player currentPlayer {
        get{
            return _currentPlayer;
        }
        set{
            _currentPlayer = value;
            setSideRotation();
            gameObject.layer = _currentPlayer.layerNumber();
        }//player controlling this piece, changes on capture/reset
    }
    public Position currentPosition {get; protected set;} //where this piece is now or is moving to
    public Player owner; //initialization and reset
    public Position startPosition;//initialization and reset

    //movement controls
    protected bool isMoving = false;
    protected static float movingSpeed = 2f; //fixed
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
    protected Vector3 targetRotation;
    protected Quaternion oldRotation;
    protected bool isChangingSides = false;
    protected bool isFlipping = false;
    protected float rotationSpeed = 5f; //fixed


    //visuals
    protected bool isSelected = false;
    protected int size;


    protected abstract void Awake();//piece-specific initialization

	protected virtual void Start () {
        currentPosition = startPosition;
        board.placePiece(this, currentPosition);
        board.pieceList.Add(this);
        transform.position = board.PieceToWorldPoint(this);

        currentPlayer = owner;
        targetRotation = currentPlayer.targetRotation;
        transform.rotation = Quaternion.Euler(targetRotation);

        Mesh mesh = MakeMesh(size);
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
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
        if (isChangingSides || isFlipping){//orientation by player, align piece and player directions
            if (Quaternion.Angle(this.transform.rotation, Quaternion.Euler(targetRotation)) < .1f){
                this.transform.rotation = Quaternion.Euler(targetRotation);
                //this.transform.rotation = Quaternion.Euler(targetRotation);
                isFlipping = false;
                isChangingSides = false;
            }else{
                this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, Quaternion.Euler(targetRotation), 7.5f);
            }
        }
        //check selection w/ raycast

	}

    //finds all legal positions this piece can move to
    public virtual List<Vector2> getLegalMoveVectors(){
        List<Vector2> legalMoves = new List<Vector2>();
        if (currentPosition.isSideboard){
            CalculateDropPositions();
            foreach (Vector2 possibleDrop in possibleDrops){
                legalMoves.Add(possibleDrop);
            }
        }else{
            int directionFactor = currentPlayer.isPlayerOne() ? 1 : -1;
            foreach (Vector2 possibleMove in possibleMoves){
                if (board.isLegalMovePosition(this, currentPosition + directionFactor*possibleMove)){
                    legalMoves.Add(directionFactor*possibleMove);
                }
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

    public bool isLegalMovePosition(Position pos){
        foreach (Vector2 legalMove in getLegalMoveVectors()){
            if(pos.isEqual(legalMove + currentPosition)){
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
        if (pos.isSideboard){
            targetLocation = pos.getSideboard().PieceToWorldPoint(this);

        }else{
            targetLocation = board.PieceToWorldPoint(this);
        }
    }

    //move this piece according to the specified move
    public void makeMove(Move move){
        if (!move.startPosition.isEqual(currentPosition) || move.piece != this || !board.isLegalMovePosition(this, move.endPosition)){
            Debug.LogError("There's something wrong with the move you made");
        }
        //move with capture
        if(move.type == Move.Type.capture){//relocating captured piece
            Piece captured = board.getPiece(move.endPosition);
            captured.Demote();
            board.removePiece(captured);
            captured.currentPlayer = this.currentPlayer;
            currentPlayer.sideboard.addPiece(captured);
            captured.moveToPosition(currentPlayer.sideboard.getPosition());
            board.removePiece(this);
            if (move.isPromotion){
                this.Promote();
            }
        }else if (move.type == Move.Type.drop){//drop
            currentPosition.getSideboard().removePiece(this);
        }else{//regular move
            board.removePiece(this);
            if (move.isPromotion){
                this.Promote();
            }
        }
        moveToPosition(move.endPosition);
        board.placePiece(this, move.endPosition);
        board.kifu.addMove(move);
    }

    //flip the piece over
    public virtual void Promote(){
        //expected behavior: flip piece, change movement
        isPromoted = true;
        setFlipRotation();
        possibleMoves = promotedMoves;
    }

    public virtual void Demote(){
        if (isPromoted){
            isPromoted = false;
            setFlipRotation();
            possibleMoves = normalMoves;
        }
    }

    protected void setFlipRotation(){
        isFlipping = true;
        targetRotation.y += 180;
        targetRotation.x *= -1;
    }

    protected void setSideRotation(){
        isChangingSides = true;
        targetRotation.z += 180;
        targetRotation.x *= -1;
    }

    public virtual bool canPromote(Position movePos){//override for King, Gold,
        if (this.currentPosition.isSideboard || this.isPromoted){
            return false;
        }else if (this.currentPlayer.isPlayerOne()){
            return movePos.y < 4 || this.currentPosition.y < 4;
        }else{
            return movePos.y > 6 || this.currentPosition.y > 6;
        }
    }

    public void Reset(){
        //return piece to starting position: unpromoted, in starting position, original owner
        Demote();
        //restore control to owner
        if (owner != currentPlayer){
            currentPlayer = owner;
            targetRotation = owner.targetRotation;
        }
        //remove from old position in board/sideboard
        if (currentPosition.isSideboard){
            currentPosition.getSideboard().removePiece(this);
        }else{
            board.removePiece(this);
        }
        board.placePiece(this, startPosition);
        currentPosition = startPosition; //return to start position on board+internally
        targetLocation = board.PieceToWorldPoint(this);
    }

    public void selectPiece(){
        isSelected = true;
        //missing: enable highlight?
    }

    public void deselectPiece(){
        isSelected = false;
        //missing: disable highlight?
    }

    protected virtual void CalculateDropPositions(){
        //drop is illegal if:
        //  another piece occupies the position**accounted for in board
        //  this piece has no possible moves (L,N,P)**accounted for in board
        //  no two pawns in one file **override
        //  pawn cannot drop for checkmate **override
        this.possibleDrops = new List<Vector2>();
        for (int x = 1; x <= 9; x++){
            for (int y = 1; y <= 9; y++){
                Position pos = Position.makeNew(x, y); //created and thrown away... inefficient
                //Debug.Log("Position: "+pos.toString()+ " legal: "+ board.isLegalDropPosition(this, pos));
                if (board.isLegalDropPosition(this, pos)){
                    this.possibleDrops.Add(new Vector2(x, y));
                }
            }
        }
    }

    protected Mesh MakeMesh(int pieceSize){
        //size: K = 5; R,B = 4; G,S = 3; Kn = 2, L = 1; P = 0;

        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[30];
        //front
        vertices[0] = new Vector3(0-5.1875f, 0-5.5625f, 0-3.15f/2);
        vertices[1] = new Vector3(1f-5.1875f, 9.375f-5.5625f, .8125f-3.15f/2);
        vertices[2] = new Vector3(5.1875f-5.1875f, 11.125f-5.5625f, 1f-3.15f/2);
        vertices[3] = new Vector3(9.375f-5.1875f, 9.375f-5.5625f, .8125f-3.15f/2);
        vertices[4] = new Vector3(10.375f-5.1875f, 0-5.5625f, 0-3.15f/2);
        //back
        vertices[5] = new Vector3(-5.1875f, 0-5.5625f, 3.5f-3.15f/2);
        vertices[6] = new Vector3(1f-5.1875f, 9.375f-5.5625f, 2.6875f-3.15f/2);
        vertices[7] = new Vector3(5.1875f-5.1875f, 11.125f-5.5625f, 2.5f-3.15f/2);
        vertices[8] = new Vector3(9.375f-5.1875f, 9.375f-5.5625f, 2.6875f-3.15f/2);
        vertices[9] = new Vector3(10.375f-5.1875f, 0-5.5625f, 3.5f-3.15f/2);

        float vertScale = (27f + pieceSize)/32f;
        float horizScale = 22.5f/28.7f + .2f*pieceSize*(1 - 22.5f/28.7f);
        float depthScale = 7.75f/9.7f + .2f*pieceSize*(1 - 7.75f/9.7f);
        //Debug.Log("scales x: "+horizScale+", y: "+vertScale+", z: "+depthScale);
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
        //front
        uv[0] = new Vector2(9f/512f, 1f/256f);
        uv[1] = new Vector2(31f/512f, 210f/256f);
        uv[2] = new Vector2(124f/512f, 249f/256f);
        uv[3] = new Vector2(217f/512f, 210f/256f);
        uv[4] = new Vector2(239f/512f, 1f/256f);
        //back
        uv[5] = new Vector2(495f/512f, 1f/256f);
        uv[6] = new Vector2(473f/512f, 210f/256f);
        uv[7] = new Vector2(380f/512f, 249f/256f);
        uv[8] = new Vector2(287f/512f, 210f/256f);
        uv[9] = new Vector2(265f/512f, 1f/256f);
        for (int i = 10; i < 30; i++){
            uv[i] = Vector2.zero;
        }
        mesh.uv = uv;

        mesh.RecalculateNormals();

        return mesh;
    }


    public abstract string toString();

}
