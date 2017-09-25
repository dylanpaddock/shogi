using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class Position : ScriptableObject {
    public int _x;
    public int x {
        get {
            if (sideboard){
                Debug.Log("Asked for board position for sideboard piece");
                return 0;
            }
            return _x;
        }
        set{
            _x = value;
        }
    }
    public int _y;
    public int y {
        get {
            if (sideboard){
                Debug.Log("Asked for board position for sideboard piece");
                return 0;
            }
            return _y;
        }
        set{
            _y = value;
        }
    }
    private Sideboard sideboard;
    public bool isSideboard {get; protected set;}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

    public void set(int newx, int newy){
        if (newx == 0 || newy == 0){
            Debug.Log("Trying to create illegal move from value");
            return;
        }
        this.x = newx;
        this.y = newy;
        isSideboard = false;
    }

    public void set(Vector2 v){
        set((int)v.x, (int)v.y);
    }

    public void set(Sideboard sideboard){//should only be used when capturing a piece
        isSideboard = true;
        this.sideboard = sideboard;
        x = 0;
        y = 0;
    }

//    public Vector2 getPosition(){
//        if (isSideboard){
//            Debug.Log("Illegal request for board position on while on sideboard");
//            return Vector2.zero;
//        }
//        return pos;
//    }

    public Sideboard getSideboard(){
        if (isSideboard){
            return sideboard;
        }
        Debug.Log("Illegal request for sideboard position while on board");
        return null;
    }

    //get the world coordinates for this position: board position or sideboard position
    public Vector3 getWorldPosition(){
        return Vector3.zero;
    }

    public string toString(){
        return ""+x+""+y;
    }

    //creates a new move which is a combination of the two given moves
    public static Position operator +(Vector2 m, Position p){
        Position newPosition;
        if (p.isSideboard){
            newPosition = Position.makeNew((int)m.x, (int)m.y);
            return newPosition;
        }else{
            newPosition = Position.makeNew((int)m.x + p.x, (int)m.y + p.y);
            return newPosition;
        }
    }
    //commutivity
    public static Position operator +(Position p, Vector2 m){
        Position newPosition;
        if (p.isSideboard){
            newPosition = Position.makeNew((int)m.x, (int)m.y);
            return newPosition;
        }else{
            newPosition = Position.makeNew((int)m.x + p.x, (int)m.y + p.y);
            return newPosition;
        }
    }

    public static Position makeNew(int x, int y){
        Position newPosition = ScriptableObject.CreateInstance<Position>();
        newPosition.set(x,y);
        return newPosition;
    }

    public static Position makeNew(Sideboard sb){
        Position newPosition = ScriptableObject.CreateInstance<Position>();
        newPosition.set(sb);
        return newPosition;
    }


}
