using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : MonoBehaviour {
    public List<Player> players; //always two players
    int current;
	// Use this for initialization
	void Start () {
        if (players[0].playerName == Player.Name.BLACK){
            current = 0;
        }else{
            current = 1;
        }
	}

	// Update is called once per frame
	void Update () {

	}

    public void passTurn(){
        current = 1 - current;//switch player
    }

    public Player currentPlayer(){
        return players[current];
    }
}
