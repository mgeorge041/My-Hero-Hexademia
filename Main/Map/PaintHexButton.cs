using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PaintHexButton : MonoBehaviour
{
    // Scene manager
    public MapBuildingTestScene mbts;

    // Hex info
    public HexStats hexStats;
    public Tile tile;
    public Image hexImage;


    // Create paint hex button
    public static PaintHexButton CreatePaintHexButton(string hexName)
    {
        PaintHexButton newPaintHexButton = Instantiate(Resources.Load<PaintHexButton>("Prefabs/Map/Paint Hex Button"));
        newPaintHexButton.SetHex(hexName);
        return newPaintHexButton;
    }


    // Set hex info
    public void SetHex(string hexName)
    {
        hexStats = HexStats.LoadHexStats(hexName);
        tile = hexStats.tile;
        hexImage.sprite = tile.sprite;
    }


    // Set selected paint hex
    public void SetSelectedPaintHex()
    {
        mbts.SetSelectedPaintHex(this);
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
