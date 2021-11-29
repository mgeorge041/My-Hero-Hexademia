using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Map.UTests;
using Characters.ITests;

namespace Characters.ITests
{
    public class CharacterControllerITests
    {
        private HexMap hexMap;
        private Character character1;
        private Character character2;
        private Hex hex1;
        private Hex hex2;
        private Vector3Int centerCoords;
        private Vector3Int targetCoords;

        // Setup
        [SetUp]
        public void Setup()
        {
            hexMap = HexMapUTests.CreateTestHexMap();
            character1 = CharacterITests.CreateTestCharacterWithPlayer(1);
            character2 = CharacterITests.CreateTestCharacterWithPlayer(2);
            centerCoords = Vector3Int.zero;
            targetCoords = new Vector3Int(1, -1, 0);
            
            hex1 = hexMap.GetHexAtHexCoords(centerCoords);
            hex2 = hexMap.GetHexAtHexCoords(targetCoords);
            hex1.AddCharacter(character1);
            hex2.AddCharacter(character2);
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(hexMap);
            C.Destroy(character1);
            C.Destroy(character2);
        }

        // Test targets 1st ability
        [Test]
        public void TargetsAbility()
        {
            List<Hex> targetHexes = character1.ability1.GetTargets(hexMap);
            Assert.AreEqual(1, targetHexes.Count);
            Assert.Contains(hex2, targetHexes);
        }
    }
}