using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // Game
    public GameManager gameManager;
    public bool isTurn = true;
    public PlayerState playerState;

    // Stats
    public int id = 1;
    public int villainPoints = 0;
    public int heroPoints = 0;
    public int maxPoints = 100;

    // Hex map
    public HexMapController hexMapController;

    // Characters
    public CharacterCard characterCard;
    public Character character;
    public List<Character> characters = new List<Character>();
    public CharacterController characterController;
    
    // UI
    public Camera playerCamera;
    public PlayerUI playerUi;


    // Create player
    public static Player CreatePlayer()
    {
        Player newPlayer = Instantiate(GetPlayerPrefab()).GetComponent<Player>();
        return newPlayer;
    }


    // Create player
    public static Player CreatePlayer(int playerId)
    {
        Player newPlayer = Instantiate(GetPlayerPrefab()).GetComponent<Player>();
        newPlayer.id = playerId;
        return newPlayer;
    }


    // Get player prefab
    public static GameObject GetPlayerPrefab()
    {
        GameObject playerPrefab = Resources.Load<GameObject>(ENV.PLAYER_PREFAB_RESOURCE_PATH);
        return playerPrefab;
    }


    // Initialize
    public void Initialize(HexMap hexMap) 
    {
        hexMapController = new HexMapController(hexMap, this);
        characterController = new CharacterController(hexMap, this);

        playerUi.UpdateBarsAndPointLabels();
    }


    // Reset
    public void Reset()
    {
        heroPoints = 0;
        villainPoints = 0;
        characterCard = null;
        foreach (Character character in characters)
        {
            C.Destroy(character);
        }
        playerUi.Reset();
    }


    // Start turn
    public void StartTurn()
    {
        isTurn = true;
        foreach (Character character in characters)
        {
            Debug.Log("Starting character turn: " + character.name);
            character.StartTurn();
        }
    }


    // End turn
    public void EndTurn()
    {
        isTurn = false;
        gameManager.NextTurn();
    }


    // Add hero points
    public void AddHeroPoints(int points)
    {
        heroPoints += points;
        playerUi.UpdateHeroBarAndPointsLabel();
    }


    // Add villain points
    public void AddVillainPoints(int points)
    {
        villainPoints += points;
        playerUi.UpdateVillainBarAndPointsLabel();
    }


    // Add character card
    public void AddCharacterCard(CharacterCard characterCard)
    {
        characterCard.player = this;
        playerUi.AddCharacterCard(characterCard);
    }


    // Set selected character placement
    public bool SetSelectedCharacterCard(CharacterCard characterCard)
    {
        // Clear same selected panel
        if (characterCard == this.characterCard)
        {
            this.characterCard = null;
            hexMapController.ClearPlacementTiles();
            characterCard.StopWalkAnimation();
            return false;
        }

        // Playable characters
        if (CanPlayCharacterCard(characterCard))
        {
            // Stop animation for prior selected card
            if (this.characterCard != null)
            {
                this.characterCard.StopWalkAnimation();
            }

            this.characterCard = characterCard;
            hexMapController.SetPlacementHexes();
            return true;
        }
        return false;
    }


    // Get whether can play character
    public bool CanPlayCharacterCard(CharacterCard characterCard)
    {
        // Heroes
        if (characterCard.team == Team.Hero && characterCard.cost < heroPoints)
        {
            return true;
        }

        // Villains
        if (characterCard.team == Team.Villain && characterCard.cost < villainPoints)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    // Play character card
    public void PlayCharacterCard()
    {
        if (characterCard.team == Team.Hero)
        {
            heroPoints -= characterCard.cost;
        }
        else
        {
            villainPoints -= characterCard.cost;
        }
        playerUi.PlayCharacterCard(characterCard);
        characterCard = null;
    }


    // Set selected character
    public void SetSelectedCharacter(Character character)
    {
        this.character = character;
        playerUi.SetSelectedCharacter(character);
    }


    // Clear selected character
    public void ClearSelectedCharacter()
    {
        character = null;
        playerState = PlayerState.None;
    }


    // Add character to list
    public void AddCharacter(Character character)
    {
        characters.Add(character);
        character.player = this;
    }


    // Character stopped moving
    public void CharacterStoppedMoving(Character character)
    {
        hexMapController.SetSelectedHex(character.hex);
    }


    // Get mouse world position
    public Vector3 GetMouseWorldPosition()
    {
        return playerCamera.ScreenToWorldPoint(Input.mousePosition);
    }


    // Set player state
    public void SetPlayerState(PlayerState playerState)
    {
        this.playerState = playerState;

        if (playerState == PlayerState.None)
        {
            character = null;
            characterCard = null;
            hexMapController.Clear();
            characterController.Clear();
        }
        else if (playerState == PlayerState.Action)
        {
            hexMapController.ShowActionState();
        }
        else if (playerState == PlayerState.Ability)
        {
            characterController.ShowAbilityState();
        }
        else if (playerState == PlayerState.Placement)
        {
            hexMapController.ShowPlacementState();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Left click
        if (isTurn && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Player left clicked");
            hexMapController.LeftClick(GetMouseWorldPosition());
        }

        // Right click
        if (isTurn && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Player right clicked");
            if (playerState == PlayerState.Action || playerState == PlayerState.Placement)
            {
                hexMapController.RightClick(GetMouseWorldPosition());
            }
            if (playerState == PlayerState.Ability)
            {
                characterController.RightClick(GetMouseWorldPosition());
            }
        }

        // Hit A button
        if (isTurn && Input.GetKeyDown(KeyCode.A)) 
        {
            Debug.Log("Player hit A button");
            if (playerState == PlayerState.Action || playerState == PlayerState.Ability)
            {
                characterController.HitAButton();
            }
        }
    }
}
