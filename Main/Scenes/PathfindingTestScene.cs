using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PathfindingTestScene : MonoBehaviour, ISceneManager
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

    // Pathfinding
    private Hex previousTargetHex;
    private Hex centerHex;

    // Start scene
    public void StartScene()
    {
        hexMap.Initialize();
        centerHex = hexMap.GetHexAtHexCoords(Vector3Int.zero);
        CreateMapTiles();
        CreateHexInfoPanels();
    }


    // Reset scene
    public void ResetScene()
    {
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


    // Start is called before the first frame update
    void Start()
    {
        StartScene();
    }


    // Update is called once per frame
    void Update()
    {
        Hex targetHex = hexMap.GetHexAtMousePosition(mainCamera);
        if (targetHex != previousTargetHex)
        {
            previousTargetHex = targetHex;
            hexMap.ClearMovementTilemap();
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            hexMap.PaintPath(path);
        }
    }
}
