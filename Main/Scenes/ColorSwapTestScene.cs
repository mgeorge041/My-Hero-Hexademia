using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwapTestScene : MonoBehaviour, ISceneManager
{
    private Character character;
    public Color swapColor;


    // Update character color
    public void UpdateCharacterColor()
    {
        character.spriteRenderer.material.SetColor("ColorOut", swapColor);
    }


    // Start scene
    public void StartScene()
    {
        character = Character.CreateCharacter("Shoto");
    }


    // Reset scene
    public void ResetScene()
    {

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
