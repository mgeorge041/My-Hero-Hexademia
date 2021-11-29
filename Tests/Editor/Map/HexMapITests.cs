using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Map.UTests;
using Characters.UTests;
using Players.UTests;

namespace Map.ITests
{
    public class HexMapITests
    {
        private Character character1;
        private Character character2;
        private HexMap hexMap;
        private Hex centerHex;
        private Hex targetHex; 
        private Player player1;
        private Player player2;
        

        // Setup
        [SetUp]
        public void Setup()
        {
            character1 = CharacterUTests.CreateTestCharacter();
            character2 = CharacterUTests.CreateTestCharacter();
            player1 = PlayerUTests.CreateTestPlayer(1);
            player2 = PlayerUTests.CreateTestPlayer(2);
            character1.player = player1;
            character2.player = player2;
            hexMap = HexMapUTests.CreateTestHexMap();
            centerHex = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            targetHex = hexMap.GetHexAtHexCoords(new Vector3Int(1, -1, 0));
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(character1);
            C.Destroy(hexMap);
        }

        // Test setting character
        [Test]
        public void SetsCharacter_NotMoveable()
        {
            hexMap.AddCharacterToHex(character1, centerHex);
            Assert.IsFalse(centerHex.moveable);
        }

        // Test getting actionable hexes
        [Test]
        public void GetPossibleHexes()
        {
            character1.remainingSpeed = 1;
            character1.range = 1;
            hexMap.AddCharacterToHex(character1, centerHex);
            hexMap.AddCharacterToHex(character2, targetHex);
            List<Hex> possibleHexes = hexMap.GetMaxPath(centerHex);
            Assert.AreEqual(6, possibleHexes.Count);
            Assert.Contains(targetHex, possibleHexes);
        }
    }
}