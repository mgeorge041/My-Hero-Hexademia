using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MovementTestScene : MonoBehaviour, ISceneManager
{
    // Hex map
    public HexMapController hexMapController;
    public HexMap hexMap;
    public Tile defaultTile;
    public Tile redTile;
    public Tile wallTile;
    public Tile startTile;
    public Camera mainCamera;

    // Hex info panels
    public GameObject hexInfoPrefab;
    private List<GameObject> hexInfoPanels = new List<GameObject>();
    public Canvas canvas;

    // Character pathfinding
    private Player player;
    private Hex centerHex;
    public GameObject characterPrefab;
    private List<Character> characters = new List<Character>();


    // Start scene
    public void StartScene()
    {
        hexMap.Initialize();
        centerHex = hexMap.GetHexAtHexCoords(Vector3Int.zero);
        CreateMapTiles();
        CreateHexInfoPanels();
        player = Player.CreatePlayer();
        player.Initialize(hexMap);
        player.playerCamera = mainCamera;
        player.StartTurn();
        player.playerUi.gameObject.SetActive(false);
        CreateTestUnit();
    }


    // Reset scene
    public void ResetScene()
    {
        player.Reset();
        hexMap.Reset();
        StartScene();
    }


    // Create map tiles
    private void CreateMapTiles()
    {
        foreach (Hex hex in hexMap.hexCoordsDict.Values)
        {
            if (Hex.GetDistanceHexes(hex, centerHex) == 2 && hex.hexCoords.x != 0)
                hex.hexStats = HexStats.LoadHexStats("mud");
            else if (Hex.GetDistanceHexes(hex, centerHex) == 3)
                hex.hexStats = HexStats.LoadHexStats("grass");
            else if (Hex.GetDistanceHexes(hex, centerHex) == 4 && hex.hexCoords.y != 0)
                hex.hexStats = HexStats.LoadHexStats("wall");
        }
        hexMap.Reset();
    }


    // Toggle hex info panels
    public void ToggleHexInfoPanels()
    {
        foreach (GameObject hexInfoPanel in hexInfoPanels)
        {
            hexInfoPanel.SetActive(!hexInfoPanel.activeSelf);
        }
    }


    // Create hex info panels
    private void CreateHexInfoPanels()
    {
        foreach (Hex hex in hexMap.hexCoordsDict.Values)
        {
            // Create hex info panel
            GameObject newHexInfoObject = Instantiate(hexInfoPrefab);
            newHexInfoObject.transform.SetParent(canvas.transform);

            HexInfoPanel newHexInfo = newHexInfoObject.GetComponent<HexInfoPanel>();
            newHexInfo.transform.position = mainCamera.WorldToScreenPoint(hexMap.grid.CellToWorld(hex.tileCoords));
            newHexInfo.hex = hex;

            hexInfoPanels.Add(newHexInfoObject);
        }
    }


    // Create test unit
    public void CreateTestUnit()
    {
        if (centerHex.hasCharacter)
            return;

        Character newCharacter = Character.CreateCharacter("Test");
        newCharacter.canAttack = false;
        player.AddCharacter(newCharacter);
        centerHex.AddCharacter(newCharacter);
        characters.Add(newCharacter);
    }


    // Reset characters
    public void ResetCharacters()
    {
        foreach (Character character in characters)
        {
            character.StartTurn();
        }
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
