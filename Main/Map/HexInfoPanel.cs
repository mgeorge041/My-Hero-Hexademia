using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexInfoPanel : MonoBehaviour
{
    public Text gCostLabel;
    public Text hCostLabel;
    public Text coordLabel;
    public Hex hex;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hex != null)
        {
            gCostLabel.text = hex.gCost.ToString();
            hCostLabel.text = hex.hCost.ToString();
            coordLabel.text = hex.hexCoords.ToString();
        }
    }
}
