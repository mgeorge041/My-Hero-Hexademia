using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsPanel : MonoBehaviour
{
    // Character UI
    public Image characterPortrait;
    public Image characterHealthbar;
    public Text characterHealthbarText;
    public Image attackIcon;
    public Image speedIcon;
    public Text attackText;
    public Text defenseText;
    public Text rangeText;
    public Text speedText;
    public Image ability1Image;
    public Image ability2Image;


    // Create character stats panel
    public static CharacterStatsPanel CreateCharacterStatsPanel()
    {
        CharacterStatsPanel newCharacterStatsPanel = Instantiate(Resources.Load<GameObject>(ENV.CHARACTER_STATS_PANEL_PREFAB_RESOURCE_PATH).GetComponent<CharacterStatsPanel>());
        return newCharacterStatsPanel;
    }


    // Set selected character
    public void SetSelectedCharacter(Character character)
    {
        characterPortrait.sprite = character.stats.sprite;
        characterHealthbarText.text = character.currentHealth.ToString();
        characterHealthbar.fillAmount = (float)character.currentHealth / character.health;

        attackText.text = character.strength.ToString();
        defenseText.text = character.defense.ToString();
        rangeText.text = character.range.ToString();
        speedText.text = character.remainingSpeed + "/" + character.speed;
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
