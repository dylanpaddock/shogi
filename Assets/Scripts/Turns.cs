using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : MonoBehaviour {
    public List<Player> players; //always two players
    int current;
	// Use this for initialization
	void Start () {
        startGame();
        Debug.Log("current turn: " + toString());

	}

	// Update is called once per frame
	void Update () {

	}

    public void passTurn(){
        current = 1 - current;//switch player
        Debug.Log("current turn: " + toString());
    }

    public Player currentPlayer(){
        return players[current];
    }

    public bool isTurn(Player player){
        return currentPlayer() == player;
    }

    public void startGame(){
        if (players[0].playerName == Player.Name.BLACK){
            current = 0;
        }else{
            current = 1;
        }
    }

    public string toString(){
        return players[current].playerName == Player.Name.BLACK ? "Black" : "White";
    }
}
