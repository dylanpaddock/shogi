using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turns : MonoBehaviour {
    public List<Player> players; //always two players
    private int current;
    public enum GameMode {standard, handicap, tsume};
    public GameMode gameMode = GameMode.standard;

    // Use this for initialization
	void Start () {
        StartGame();
        //Debug.Log("current turn: " + toString());

	}

	// Update is called once per frame
	void Update () {

	}

    public void passTurn(){
        current = 1 - current;//switch player
        //Debug.Log("current turn: " + toString());

    }

    public Player activePlayer(){
        return players[current];
    }

    public Player inactivePlayer(){
        return players[1 - current];
    }

    public bool isTurn(Player player){
        return activePlayer() == player;
    }

    public string toString(){
        return players[current].playerName == Player.Name.BLACK ? "Black" : "White";
    }

    public void StartGame(){
        current = players[0].playerName == Player.Name.BLACK ? 0 : 1;
    }

    public void ResetPlayers(){
        foreach (Player player in players){
            player.Reset();
        }
    }
}
