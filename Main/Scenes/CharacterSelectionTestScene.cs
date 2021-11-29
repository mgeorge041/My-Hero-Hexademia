using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterSelectionTestScene : MonoBehaviour, ISceneManager
{
    // Camera
    public Camera mainCamera;

    // Teams
    public int teamNumber = 0;
    public int pickNumber = 0;

    // Characters
    private Dictionary<Character, CharacterStatsPanel> characters = new Dictionary<Character, CharacterStatsPanel>();
    private string[] characterNames = new string[]
    {
        "Shoto",
        "Bakugo",
        "Iida",
        "Test",
        "Test",
        "Test",
        "Test",
        "Test",
        "Test",
        "Test",
        "Test",
        "Test"
    };
    private Vector3Int[] characterHexes = new Vector3Int[]
    {
        new Vector3Int(1, -2, 1),
        new Vector3Int(2, -3, 1),
        new Vector3Int(3, -3, 0),
        new Vector3Int(1, 0, -1),
        new Vector3Int(2, -1, -1),
        new Vector3Int(3, -1, -2),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-2, 1, 1),
        new Vector3Int(-3, 2, 1),
        new Vector3Int(-1, -1, 2),
        new Vector3Int(-2, -1, 3),
        new Vector3Int(-3, 0, 3)
    };
    private Vector3Int[] team1Hexes = new Vector3Int[]
    {
        new Vector3Int(-4, -1, 5),
        new Vector3Int(-5, 0, 5),
        new Vector3Int(-5, 1, 4),
        new Vector3Int(-5, 2, 3),
        new Vector3Int(-5, 3, 2),
        new Vector3Int(-5, 4, 1)
    };
    private Vector3Int[] team2Hexes = new Vector3Int[]
    {
        new Vector3Int(4, -5, 1),
        new Vector3Int(5, -5, 0),
        new Vector3Int(5, -4, -1),
        new Vector3Int(5, -3, -2),
        new Vector3Int(5, -2, -3),
        new Vector3Int(5, -1, -4)
    };

    // Hex map
    public HexMap hexMap;

    // Character panels
    public GameObject dummyStatsPanelPrefab;
    public Transform statsPanelTransform;
    public Transform team1Panel;
    public Transform team2Panel;
    public Transform team1ScrollView;
    public Transform team2ScrollView;
    public Transform team1CaptainPanel;
    public Transform team2CaptainPanel;
    private Character team1Captain;
    private Character team2Captain;
    public Text vsText;
    private Character selectedCharacter = null;


    // Start scene
    public void StartScene()
    {
        hexMap.Initialize(8);
        for (int i = 0; i < characterHexes.Length; i++)
        {
            hexMap.GetHexAtHexCoords(characterHexes[i]).hexStats = HexStats.LoadHexStats("grass");
        }
        hexMap.Reset();
        
        CreateCharacters();
    }


    // Reset scene
    public void ResetScene()
    {
        foreach (KeyValuePair<Character, CharacterStatsPanel> pair in characters)
        {
            C.Destroy(pair.Key);
            C.Destroy(pair.Value);
        }
        characters.Clear();
        hexMap.Reset();
        pickNumber = 0;
        teamNumber = 0;
        vsText.gameObject.SetActive(false);
        StartScene();
    }


    // Create characters
    public void CreateCharacters()
    {
        for (int i = 0; i < characterNames.Length; i++)
        { 
            Character newCharacter = Character.CreateCharacter(characterNames[i]);
            characters.Add(newCharacter, null);
            hexMap.AddCharacterToHex(newCharacter, hexMap.GetHexAtHexCoords(characterHexes[i]));
        }
    }


    // Select character panel
    public void SelectCharacter(Character character)
    {
        
        // Clear stats panel
        if (character == null)
        {
            return;
        }
        // 1st character select
        if (selectedCharacter != character)
        {
            // Create new panel for 1st time character select
            if (characters[character] == null)
            {
                Debug.Log("Creating character panel");
                CreateCharacterPanel(character);
            }

            // Turn off previous stats panel
            if (selectedCharacter != null)
                characters[selectedCharacter].gameObject.SetActive(false);

            characters[character].gameObject.SetActive(true);
            selectedCharacter = character;
        }

        // Select same character twice
        else
        {
            if (pickNumber < 1 && teamNumber == 0)
            {
                Debug.Log("current panel position: " + characters[character].transform.GetComponent<RectTransform>().anchoredPosition);
                characters[character].transform.SetParent(team1CaptainPanel);
                Debug.Log("after setting parent panel position: " + characters[character].transform.GetComponent<RectTransform>().anchoredPosition);
                SetCaptainPanelPosition(characters[character], team1CaptainPanel);
                vsText.gameObject.SetActive(true);
            }
            else if (pickNumber < 1 && teamNumber == 1)
            {
                SetCaptainPanelPosition(characters[character], team2CaptainPanel);
            }
            else
            {
                SetCharacterCardPosition(characters[character]);
            }
            selectedCharacter = null;
            MoveCharacterToPosition(character);
        }

        Debug.Log("Team panel height: " + team1ScrollView.transform.GetComponent<RectTransform>().rect.height);
    }


    // Add character panel
    public void CreateCharacterPanel(Character character)
    {
        CharacterStatsPanel newCharacterPanel = CharacterStatsPanel.CreateCharacterStatsPanel();
        newCharacterPanel.SetSelectedCharacter(character);
        newCharacterPanel.transform.SetParent(statsPanelTransform);
        characters[character] = newCharacterPanel;

        Debug.Log("Team panel height: " + team1ScrollView.transform.GetComponent<RectTransform>().rect.height);
    }


    // Get list character stats panel position
    public Vector3 GetCharacterListPosition(CharacterStatsPanel statsPanel, Transform scrollView, int xOffset)
    {
        int numChildren = scrollView.transform.childCount;
        VerticalLayoutGroup group = scrollView.GetComponent<VerticalLayoutGroup>();
        RectTransform rectTransform = statsPanel.transform.GetComponent<RectTransform>();
        float yPos = -(group.padding.top + numChildren * group.spacing + (numChildren + 1) * rectTransform.rect.height);
        return new Vector3(xOffset, yPos);
    }


    // Get captain panel position
    public Vector3 GetCaptainPanelPosition(Transform captainPanel)
    {
        return new Vector3(captainPanel.transform.GetComponent<RectTransform>().anchoredPosition.x, -150);
    }


    // Get transform position
    public void SetCharacterCardPosition(CharacterStatsPanel statsPanel)
    {
        Vector3 endPosition;
        GameObject dummyStatsPanel = Instantiate(dummyStatsPanelPrefab);

        if (teamNumber == 0)
        {
            endPosition = GetCharacterListPosition(statsPanel, team1ScrollView, 27);
            dummyStatsPanel.transform.SetParent(team1ScrollView);
            statsPanel.transform.SetParent(team1Panel);
            StartCoroutine(SlideStatsPanel(statsPanel, endPosition, team1ScrollView));
        }
        else
        {
            endPosition = GetCharacterListPosition(statsPanel, team2ScrollView, 10);
            dummyStatsPanel.transform.SetParent(team2ScrollView);
            statsPanel.transform.SetParent(team2Panel);
            StartCoroutine(SlideStatsPanel(statsPanel, endPosition, team2ScrollView));
        }
    }


    // Set captain panel position
    public void SetCaptainPanelPosition(CharacterStatsPanel statsPanel, Transform captainPanel)
    {
        Vector3 endPosition = GetCaptainPanelPosition(captainPanel);
        statsPanel.transform.SetParent(captainPanel);
        StartCoroutine(SlideStatsPanel(statsPanel, endPosition, captainPanel));
    }


    // Slide stats panel
    public IEnumerator SlideStatsPanel(CharacterStatsPanel statsPanel, Vector3 endPos, Transform endScrollView)
    {
        float t = 0;
        Vector3 currentPos;
        int moveSpeed = 4;
        while (t < 1)
        {
            currentPos = statsPanel.transform.GetComponent<RectTransform>().anchoredPosition;
            t += Time.deltaTime / moveSpeed;
            statsPanel.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(currentPos, endPos, t);
            yield return null;
        }
        statsPanel.transform.GetComponent<RectTransform>().anchoredPosition = endPos;
        if (endScrollView.GetComponent<VerticalLayoutGroup>() != null)
            C.DestroyGameObject(endScrollView.GetChild(endScrollView.childCount - 1).gameObject);
        statsPanel.transform.SetParent(endScrollView);
    }


    // Move character to position
    public void MoveCharacterToPosition(Character character)
    {
        Hex targetHex;
        if (teamNumber == 0)
        {
            targetHex = hexMap.GetHexAtHexCoords(team1Hexes[pickNumber]);
            if (team1Captain == null)
                team1Captain = character;
            character.SetTeamColor(team1Captain.characterName);
        }
        else
        {
            targetHex = hexMap.GetHexAtHexCoords(team2Hexes[pickNumber]);
            if (team2Captain == null)
                team2Captain = character;
            character.SetTeamColor(team2Captain.characterName);
        }

        List<Hex> path = hexMap.GetPath(character.hex, targetHex);
        character.MoveCharacter(path);
        teamNumber = (teamNumber + 1) % 2;
        if (teamNumber == 0)
        {
            pickNumber++;
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0;
            Hex hex = hexMap.GetHexAtWorldPosition(worldPosition);
            Debug.Log("world position: " + worldPosition);
            if (hex == null)
                return;
            Debug.Log("hex coords: " + hex.hexCoords);
            if (hex.hasCharacter)
            {
                SelectCharacter(hex.character);
            }
        }
    }
}
