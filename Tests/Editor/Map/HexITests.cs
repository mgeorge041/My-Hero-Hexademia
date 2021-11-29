using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Characters.UTests;
using Map.UTests;

namespace Map.ITests
{
    public class HexITests
    {
        private Character character;
        private Hex hex;

        // Setup
        [SetUp]
        public void Setup()
        {
            character = CharacterUTests.CreateTestCharacter();
            hex = HexUTests.CreateTestHex();
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(character);
        }

        // Test setting character
        [Test]
        public void SetsCharacter()
        {
            hex.AddCharacter(character);
            Assert.IsFalse(hex.moveable);
            Assert.AreEqual(hex.character, character);
            Assert.AreEqual(hex.worldPosition, character.transform.position);
        }


    }
}