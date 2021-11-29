using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Characters.UTests
{
    public class CharacterCardUTests
    {
        private CharacterCard characterCard;

        // Create test character card
        public static CharacterCard CreateTestCharacterCard()
        {
            CharacterCard newCharacterCard = TestFunctions.CreateClassObject<CharacterCard>("Assets/Resources/Prefabs/Characters/Character Card.prefab");
            return newCharacterCard;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            characterCard = CreateTestCharacterCard();
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(characterCard);
        }
    }
}