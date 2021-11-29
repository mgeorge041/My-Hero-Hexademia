using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    // Animation
    public bool moving = false;
    public Animator animator;
    public int ySpeed { get; private set; }
    public int xSpeed { get; private set; }
    public SpriteRenderer spriteRenderer;
    public Image healthbar;

    // Stats
    public CharacterStats stats;
    public string characterName;
    public int health;
    public int currentHealth;
    public int defense;
    public int strength;
    public int speed;
    public int remainingSpeed;
    public int range;

    // Map
    public Hex hex;

    // Abilities
    public Ability ability1;
    public Ability ability2;

    // Player
    public Player player;
    public int playerId
    {
        get
        {
            return player.id;
        }
    }

    // Actions
    public bool canMove
    {
        get
        {
            if (remainingSpeed > 0)
                return true;
            return false;
        }
    }
    public bool canAttack;
    public bool hasActions
    {
        get
        {
            if (!canAttack && !canMove)
                return false;
            return true;
        }
    }


    // Initialize
    public void Initialize()
    {

    }


    // Create character
    public static Character CreateCharacter(CharacterCard characterCard)
    {
        return CreateCharacter(characterCard.stats.characterName);
    }


    // Create character
    public static Character CreateCharacter(CharacterStats stats)
    {
        return CreateCharacter(stats.characterName);
    }


    // Create character
    public static Character CreateCharacter(string characterName)
    {
        Character newCharacter = Instantiate(GetCharacterPrefab()).GetComponent<Character>();
        newCharacter.SetStats(CharacterStats.LoadCharacterStats(characterName));
        newCharacter.CreateCharacterAbilities();
        Material characterMaterial = Resources.Load<Material>("Characters/" + characterName + "/Art/" + characterName + " Material");
        newCharacter.spriteRenderer.material = characterMaterial;
        return newCharacter;
    }


    // Create character abilities
    public void CreateCharacterAbilities()
    {
        switch (characterName)
        {
            case "Shoto":
                ability1 = new Fireball();
                ability1.character = this;
                break;
            default:
                ability1 = new Fireball();
                ability1.character = this;
                break;
        }
    }

    // Get character prefab
    public static GameObject GetCharacterPrefab()
    {
        GameObject characterPrefab = Resources.Load<GameObject>(ENV.CHARACTER_PREFAB_RESOURCE_PATH);
        return characterPrefab;
    }


    // Set stats
    public void SetStats(CharacterStats stats)
    {
        this.stats = stats;
        characterName = stats.characterName;
        health = stats.health;
        currentHealth = health;
        strength = stats.strength;
        speed = stats.speed;
        remainingSpeed = speed;
        range = stats.range;
        spriteRenderer.sprite = stats.sprite;
        animator.runtimeAnimatorController = stats.characterAnimatorController;
    }


    // Get character material
    public Material GetCharacterMaterial(string characterName)
    {
        return Resources.Load<Material>("Characters/" + characterName + "/Art/" + characterName + " Material");
    }


    // Set team color
    public void SetTeamColor(string teamCaptain)
    {
        switch (teamCaptain)
        {
            case "Shoto":
                spriteRenderer.material.SetColor("ColorOut", GetCharacterMaterial("Shoto").GetColor("ColorIn"));
                break;
            case "Bakugo":
                spriteRenderer.material.SetColor("ColorOut", GetCharacterMaterial("Bakugo").GetColor("ColorIn"));
                break;
            case "Iida":
                spriteRenderer.material.SetColor("ColorOut", GetCharacterMaterial("Iida").GetColor("ColorIn"));
                break;
            default:
                spriteRenderer.material.SetColor("ColorOut", GetCharacterMaterial("Test").GetColor("ColorIn"));
                break;
        }
    }


    // Start turn
    public void StartTurn()
    {
        remainingSpeed = speed;
        canAttack = true;
        ShowCharacterEnabled();
    }


    // End turn
    public void EndTurn()
    {
        remainingSpeed = 0;
        canAttack = false;
        ShowCharacterDisabled();
    }


    // Check has actions
    private void CheckHasActions()
    {
        if (!hasActions)
            ShowCharacterDisabled();
    }


    // Set speeds
    public void SetSpeeds(int xSpeed, int ySpeed)
    {
        this.xSpeed = xSpeed;
        this.ySpeed = ySpeed;
        animator.SetInteger("xSpeed", xSpeed);
        animator.SetInteger("ySpeed", ySpeed);
    }


    // Set speeds
    public void SetTravelSpeeds(Vector3 startPosition, Vector3 targetPosition)
    {
        // Y speeds
        if (startPosition.y < targetPosition.y)
            SetYSpeed(1);
        else if (startPosition.y > targetPosition.y)
            SetYSpeed(-1);
        else
            SetYSpeed(0);

        // X speeds
        if (startPosition.x < targetPosition.x)
            SetXSpeed(1);
        else if (startPosition.x > targetPosition.x)
            SetXSpeed(-1);
        else
            SetXSpeed(0);
    }


    // Set Y speed
    public void SetYSpeed(int ySpeed)
    {
        this.ySpeed = ySpeed;
        animator.SetInteger("ySpeed", ySpeed);
    }


    // Set X speed
    public void SetXSpeed(int xSpeed)
    {
        this.xSpeed = xSpeed;
        animator.SetInteger("xSpeed", xSpeed);
    }


    // Decrement speed
    public void DecrementSpeed(int speed)
    {
        remainingSpeed = Math.Max(remainingSpeed - speed, 0);
    }


    // Get path move cost
    public int GetPathMoveCost(List<Hex> path)
    {
        int pathCost = 0;
        for (int i = 1; i < path.Count; i++)
        {
            pathCost += path[i].moveCost;
        }
        return pathCost;
    }


    // Move character
    public void MoveCharacter(List<Hex> path)
    {
        Debug.Log("Moving character");
        if (path == null)
            return;

        DecrementSpeed(GetPathMoveCost(path));
        MoveCharacterHex(path[0], path[path.Count - 1]);
        moving = true;
        StartCoroutine(AnimateMove(path));
    }


    // Animate move
    public IEnumerator AnimateMove(List<Hex> path)
    {
        Debug.Log("Animating character move");
        if (path == null)
            yield break; 

        Hex startHex = path[0];
        Hex currentHex = startHex;
        Hex targetHex = null;
        path.RemoveAt(0);

        while (path.Count > 0)
        {
            // Get positions
            targetHex = path[0];
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = targetHex.worldPosition;
            int moveSpeedDecrement = 1;

            // Calculate halfway point for move speed
            Vector3 halfPoint = new Vector3(targetPosition.x - currentPosition.x, targetPosition.y - currentPosition.y) / 2 + transform.position;
            SetTravelSpeeds(currentPosition, targetPosition);
            float t = 0f;

            // Move unit
            while (t < 1)
            {
                // Move speed based on move cost of hex
                if (Vector3.Distance(transform.position, targetPosition) <= Vector3.Distance(halfPoint, targetPosition))
                {
                    moveSpeedDecrement = targetHex.moveCost;
                }
                else if (Vector3.Distance(transform.position, targetPosition) > Vector3.Distance(halfPoint, targetPosition))
                {
                    moveSpeedDecrement = currentHex.moveCost;
                }

                // Move transform
                t += Time.deltaTime / moveSpeedDecrement;
                transform.position = Vector3.Lerp(currentPosition, targetPosition, t);
                yield return null;
            }

            // Update hex
            transform.position = targetPosition;
            currentHex = targetHex;
            path.RemoveAt(0);
        }

        // Finish moving
        moving = false;
        targetHex.AddCharacter(this);
        SetSpeeds(0, 0);
        CheckHasActions();
        if (player != null)
            player.CharacterStoppedMoving(this);
    }


    // Move character
    public void MoveCharacterHex(Hex startHex, Hex targetHex)
    {
        targetHex.character = startHex.character;
        targetHex.character.hex = targetHex;
        startHex.character = null;
    }


    // Deal damage
    public void DealDamage(Character character)
    {
        character.TakeDamage(strength);
        EndTurn();
    }


    // Take damage
    public void TakeDamage(int damage)
    {
        currentHealth = Math.Max(currentHealth - damage, 0);
        healthbar.fillAmount = (float)currentHealth / health;
        StartCoroutine(AnimateTakeDamage());
    }


    // Animate take damage
    public IEnumerator AnimateTakeDamage()
    {
        Debug.Log("Animating take damage");
        animator.SetBool("damage", true);
        spriteRenderer.color = new Color(1, 0, 0);
        Vector3 originalPostion = transform.position;
        transform.position = new Vector3(transform.position.x + 0.04f, transform.position.y + 0.04f);
        float t = 0f;
        while (t < 0.5)
        {
            t += Time.deltaTime;
            if (t > 0.125 && t < 0.25)
                spriteRenderer.color = new Color(1, 1, 1);
            else if (t > 0.375 && t < 0.5)
                spriteRenderer.color = new Color(1, 0, 0);
            yield return null;
        }
        spriteRenderer.color = new Color(1, 1, 1);
        animator.SetBool("damage", false);
        transform.position = originalPostion;
    }


    // Show character enabled
    public void ShowCharacterEnabled()
    {
        spriteRenderer.color = new Color(1, 1, 1);
    }


    // Show character disabled
    public void ShowCharacterDisabled()
    {
        spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
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
