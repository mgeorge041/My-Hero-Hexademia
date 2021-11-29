using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Map.UTests;
using Characters.UTests;
using Players.UTests;

namespace Characters.ITests
{
    public class CharacterITests
    {
        private Character character;
        private Hex hex1;
        private Hex hex2;

        // Create test character with player
        public static Character CreateTestCharacterWithPlayer()
        {
            Character newCharacter = CharacterUTests.CreateTestCharacter();
            Player player = PlayerUTests.CreateTestPlayer();
            player.AddCharacter(newCharacter);
            return newCharacter;
        }

        // Create test character with player
        public static Character CreateTestCharacterWithPlayer(int playerId)
        {
            Character newCharacter = CharacterUTests.CreateTestCharacter();
            Player player = PlayerUTests.CreateTestPlayer(playerId);
            player.AddCharacter(newCharacter);
            return newCharacter;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            character = CharacterUTests.CreateTestCharacter();
            hex1 = HexUTests.CreateTestHex();
            hex2 = HexUTests.CreateTestHex();
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(character);
        }

        // Test get path cost
        [Test]
        public void GetsPathCost()
        {
            List<Hex> path = new List<Hex>();
            path.Add(hex1);
            path.Add(hex2);
            int pathCost = character.GetPathMoveCost(path);
            Assert.AreEqual(1, pathCost);
        }

        // Test decrements speed
        [Test]
        public void DecrementsSpeed()
        {
            List<Hex> path = new List<Hex>();
            path.Add(hex1);
            path.Add(hex2);
            int initialSpeed = character.remainingSpeed;
            character.MoveCharacter(path);
            Assert.AreEqual(initialSpeed - 1, character.remainingSpeed);
        }
    }
}