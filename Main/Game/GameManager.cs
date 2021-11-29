using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Player> players;
    public GameObject playerPrefab;
    public HexMap hexMap;
    public int playerTurn = 0;
    public int numPlayers;


    // Start scene
    public void StartGame()
    {
        // Create map
        hexMap.Initialize();

        // Create players
        for (int i = 0; i < numPlayers; i++)
        {
            players[i] = Instantiate(playerPrefab).GetComponent<Player>();
            players[i].Initialize(hexMap);
        }

        // Start turns
        players[0].StartTurn();
    }


    // Go to next turn
    public void NextTurn()
    {
        playerTurn++;
        playerTurn = (playerTurn + 1) % numPlayers;
        players[playerTurn].StartTurn();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
