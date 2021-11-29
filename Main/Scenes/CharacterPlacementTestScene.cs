using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPlacementTestScene : MonoBehaviour, ISceneManager
{
    // Hex map
    public HexMap hexMap;

    // Player
    private Player player;
    public GameObject characterPlacementPrefab;
    public GameObject playerPrefab;

    // UI
    public Camera sceneCamera;


    // Start scene
    public void StartScene()
    {
        hexMap.Initialize();
        player = Instantiate(playerPrefab).GetComponent<Player>();
        player.playerCamera = sceneCamera;
        player.Initialize(hexMap);
        player.StartTurn();
        CreateHeroCardShoto();
        CreateVillainCard();
        Give100HeroPoints();
        Give100VillainPoints();
    }


    // Reset scene
    public void ResetScene()
    {
        player.Reset();
        hexMap.Reset();
    }


    // Create new hero card
    public void CreateHeroCard(string characterName)
    {
        CharacterCard characterCard = CharacterCard.CreateCharacterCard(characterName);
        player.AddCharacterCard(characterCard);
    }

    // Create new hero card
    public void CreateHeroCardShoto()
    {
        CreateHeroCard("Shoto");
    }


    // Create new hero card
    public void CreateHeroCardIida()
    {
        CreateHeroCard("Iida");
    }


    // Create new villain card
    public void CreateVillainCard()
    {
        CharacterCard characterCard = CharacterCard.CreateCharacterCard("Bakugo");
        player.AddCharacterCard(characterCard);
    }


    // Give hero points
    public void Give100HeroPoints()
    {
        player.AddHeroPoints(100);
    }


    // Give villain points
    public void Give100VillainPoints()
    {
        player.AddVillainPoints(100);
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
