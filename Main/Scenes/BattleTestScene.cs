using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTestScene : MonoBehaviour, ISceneManager
{
    public HexMap hexMap;
    private Hex[] placementHex = new Hex[2];
    private Player player;
    private Player player2;
    public Camera mainCamera;
    public GameObject playerPrefab;
    public GameObject characterPrefab;

    // Start scene
    public void StartScene()
    {
        hexMap.Initialize();
        placementHex[0] = hexMap.GetHexAtHexCoords(new Vector3Int(1, -1, 0));
        placementHex[1] = hexMap.GetHexAtHexCoords(new Vector3Int(-1, 0, 1));
        
        // Player 1
        player = Player.CreatePlayer(1);
        player.Initialize(hexMap);
        player.playerCamera = mainCamera;
        player.StartTurn();
        
        // Player 2
        player2 = Player.CreatePlayer(2);
        player2.Initialize(hexMap);
        player2.playerCamera = mainCamera;
        player2.isTurn = false;

        AddCharacter();
        AddEnemyCharacter();
    }


    // Reset scene
    public void ResetScene()
    {
        player.Reset();
        player2.Reset();
        StartScene();
    }


    // Add character
    public void AddCharacter()
    {
        if (placementHex[0].hasCharacter)
            return;
        Character newCharacter = Character.CreateCharacter("Shoto");
        player.AddCharacter(newCharacter);
        hexMap.AddCharacterToHex(newCharacter, placementHex[0]);
    }


    // Add enemy character
    public void AddEnemyCharacter()
    {
        if (placementHex[1].hasCharacter)
            return;
        Character newCharacter = Character.CreateCharacter("Shoto");
        player2.AddCharacter(newCharacter);
        hexMap.AddCharacterToHex(newCharacter, placementHex[1]);
    }


    // Reset characters
    public void ResetCharacters()
    {
        player.StartTurn();
    }


    // Start is called before the first frame update
    void Start()
    {
        StartScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
