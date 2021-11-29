using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class MapBuildingTestScene : MonoBehaviour, ISceneManager
{
    // Hex map
    public HexMap hexMap;
    public Camera mainCamera;

    // Paint hex
    public PaintHexButton paintHex;
    public List<PaintHexButton> paintHexes = new List<PaintHexButton>();
    public string[] paintHexNames = new string[]
    {
        "grass",
        "mud",
        "wall",
        "white"
    };
    private bool isPainting = false;

    // Hex panel
    public Transform paintHexPanel;


    // Start scene
    public void StartScene()
    {
        hexMap.Initialize();
        CreatePaintHexes();
    }


    // Reset scene
    public void ResetScene()
    {
        foreach (PaintHexButton paintHex in paintHexes)
        {
            C.Destroy(paintHex);
        }
        hexMap.Reset();
        StartScene();
    }


    // Create paint hexes
    public void CreatePaintHexes()
    {
        for (int i = 0; i < paintHexNames.Length; i++)
        {
            PaintHexButton newPaintHex = PaintHexButton.CreatePaintHexButton(paintHexNames[i]);
            newPaintHex.transform.SetParent(paintHexPanel);
            newPaintHex.mbts = this;
            paintHexes.Add(newPaintHex);
        }
    }


    // Set selected paint hex
    public void SetSelectedPaintHex(PaintHexButton paintHex)
    {
        this.paintHex = paintHex;
    }


    // Save tilemap
    public void SaveTilemap()
    {
        string filepath = "Assets/Resources/Map/map.txt";
        StreamWriter writer = new StreamWriter(filepath, false);
        foreach (Hex hex in hexMap.hexCoordsDict.Values)
        {
            writer.WriteLine(hex.tileCoords + ":" + hex.hexStats.tile.name);
        }
        writer.Close();
    }


    // Load tilemap
    public void LoadTilemap()
    {
        string filepath = "Assets/Resources/Map/map.txt";
        StreamReader reader = new StreamReader(filepath);
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            line = line.Replace(")", string.Empty);
            line = line.Replace("(", string.Empty);
            string[] lineSplit = line.Split(':');
            string coordsString = lineSplit[0];
            string tileName = lineSplit[1];
            string[] coordsSplit = coordsString.Split(',');
            Vector3Int tileCoords = new Vector3Int(int.Parse(coordsSplit[0]), int.Parse(coordsSplit[1]), int.Parse(coordsSplit[2]));
            Debug.Log("Setting coords: " + tileCoords + " to: " + tileName);
            HexStats hexStats = HexStats.LoadHexStats(tileName);
            hexMap.terrainMap.SetTile(tileCoords, hexStats.tile);
            hexMap.GetHexAtTileCoords(tileCoords).hexStats = hexStats;
        }
    }


    // Paint tile
    public void PaintTile()
    {
        Hex hex = hexMap.GetHexAtMousePosition(mainCamera);
        Debug.Log("hex tile coords: " + hex.tileCoords);
        Debug.Log("paint hex tile: " + paintHex.tile);
        hexMap.terrainMap.SetTile(hex.tileCoords, paintHex.tile);
        hex.hexStats = paintHex.hexStats;
        Tile target = (Tile)hexMap.terrainMap.GetTile(hex.tileCoords);
    }


    // Start is called before the first frame update
    void Start()
    {
        StartScene();
    }

    // Update is called once per frame
    void Update()
    {
        // Left click
        if (Input.GetMouseButtonDown(0))
        {
            ;
        }

        // Right click
        if (Input.GetMouseButtonDown(1))
        {
            isPainting = true;
        }

        if (isPainting)
        {
            PaintTile();
        }

        if (Input.GetMouseButtonUp(1))
        {
            isPainting = false;
        }
    }

    // Mouse drag
    public void OnMouseDrag()
    {
        PaintTile();
    }
}
