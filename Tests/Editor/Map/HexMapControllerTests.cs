using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Map.UTests;
using Players.UTests;
using Characters.UTests;
using Characters.ITests;

namespace Map.ITests
{
    public class HexMapControllerTests
    {
        private HexMap hexMap;
        private HexMapController hexMapController;
        private Player player;
        private Player player2;
        private Character character;
        private CharacterCard characterCard;
        private Hex centerHex;
        private Vector3Int centerHexCoords;

        // Add character to center hex
        private void AddCharacterToCenterHex()
        {
            centerHex.AddCharacter(character);
            player.AddCharacter(character);
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            hexMap = HexMapUTests.CreateTestHexMap();
            player = PlayerUTests.CreateTestPlayer();
            player2 = PlayerUTests.CreateTestPlayer();
            hexMapController = new HexMapController(hexMap, player);
            character = CharacterUTests.CreateTestCharacter();
            characterCard = CharacterCardUTests.CreateTestCharacterCard();
            centerHexCoords = Vector3Int.zero;
            centerHex = hexMap.GetHexAtHexCoords(centerHexCoords);
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(hexMap);
            C.Destroy(player);
        }

        // Test player left click
        [Test]
        public void LeftClick_EmptyHex_NoCharacterSelected_DoNothing()
        {
            hexMapController.LeftClick(centerHexCoords);
            Assert.AreEqual(0, hexMap.actionTileCoords.Count);
            Assert.AreEqual(HexMapState.None, hexMap.hexMapState);
        }

        // Test player left click
        [Test]
        public void LeftClick_NonEmptyHex_NoCharacterSelected_SelectCharacter()
        {
            AddCharacterToCenterHex();

            hexMapController.LeftClick(centerHexCoords);
            Assert.AreEqual(91, hexMap.actionTileCoords.Count);
            Assert.AreEqual(HexMapState.Action, hexMap.hexMapState);
        }

        // Test player left click
        [Test]
        public void LeftClick_EmptyHex_CharacterSelected_DoNothing()
        {
            AddCharacterToCenterHex();
            Vector3Int hexCoords = new Vector3Int(1, -1, 0);
            Vector3 worldPosition = hexMap.HexToWorldCoords(hexCoords);

            hexMapController.LeftClick(centerHexCoords);
            hexMapController.LeftClick(worldPosition);
            Assert.AreEqual(91, hexMap.actionTileCoords.Count);
            Assert.AreEqual(HexMapState.Action, hexMap.hexMapState);
        }

        // Test player left click
        [Test]
        public void LeftClick_NonEmptyHex_CharacterSelected_SelectNewCharacter()
        {
            AddCharacterToCenterHex();

            // Add 2nd character
            Character character2 = CharacterUTests.CreateTestCharacter();
            Vector3Int hexCoords = new Vector3Int(1, -1, 0);
            Vector3 worldPosition = hexMap.HexToWorldCoords(hexCoords);
            Hex hex2 = hexMap.GetHexAtHexCoords(hexCoords);
            hex2.character = character2;
            character2.player = player;
            hexMap.Initialize(7);

            // Click
            hexMapController.LeftClick(centerHexCoords);
            hexMapController.LeftClick(worldPosition);
            Assert.AreEqual(character2, hexMapController.character);
            Assert.AreEqual(89, hexMap.actionTileCoords.Count);
            Assert.AreEqual(HexMapState.Action, hexMap.hexMapState);
        }

        // Test player left click
        [Test]
        public void LeftClick_NonEmptyHex_CharacterSelected_SelectSameCharacter()
        {
            AddCharacterToCenterHex();

            // Click
            hexMapController.LeftClick(centerHexCoords);
            hexMapController.LeftClick(centerHexCoords);
            Assert.IsNull(hexMapController.character);
            Assert.AreEqual(0, hexMap.actionTileCoords.Count);
            Assert.AreEqual(HexMapState.None, hexMap.hexMapState);
        }

        // Test player right click
        [Test]
        public void RightClick_EmptyHex_NoCharacterSelected_DoNothing()
        {
            hexMapController.RightClick(centerHexCoords);
            Assert.AreEqual(0, hexMap.actionTileCoords.Count);
            Assert.AreEqual(HexMapState.None, hexMap.hexMapState);
        }

        // Test player right click
        [Test]
        public void RightClick_NonEmptyHex_NoCharacterSelected_DoNothing()
        {
            hexMapController.RightClick(centerHexCoords);
            Assert.AreEqual(0, hexMap.actionTileCoords.Count);
            Assert.AreEqual(HexMapState.None, hexMap.hexMapState);
        }

        // Test player right click
        [Test]
        public void RightClick_EmptyHex_CharacterSelected_MoveCharacter()
        {
            AddCharacterToCenterHex();
            Vector3Int hexCoords = new Vector3Int(1, -1, 0);
            Hex hex2 = hexMap.GetHexAtHexCoords(hexCoords);
            Vector3 worldPosition = hexMap.HexToWorldCoords(hexCoords);

            // Click
            hexMapController.LeftClick(centerHexCoords);
            hexMapController.RightClick(worldPosition);
            Assert.AreEqual(0, hexMap.actionTileCoords.Count);
            Assert.AreEqual(character, hex2.character);
            Assert.IsNull(centerHex.character);
            Assert.AreEqual(HexMapState.None, hexMap.hexMapState);
        }

        // Test player right click
        [Test]
        public void RightClick_NonEmptyHex_CharacterSelected_AttackCharacter()
        {
            AddCharacterToCenterHex();

            // Create 2nd character
            Character character2 = CharacterITests.CreateTestCharacterWithPlayer(2);
            Vector3Int hexCoords = new Vector3Int(1, -1, 0);
            Vector3 worldPosition = hexMap.HexToWorldCoords(hexCoords);
            Hex hex2 = hexMap.GetHexAtHexCoords(hexCoords);
            hex2.AddCharacter(character2);

            // Click
            int startHealth = character2.currentHealth;
            hexMapController.LeftClick(centerHexCoords);
            hexMapController.RightClick(worldPosition);
            Assert.AreEqual(0, hexMap.actionTileCoords.Count);
            Assert.AreEqual(startHealth - 5, character2.currentHealth);
            Assert.IsFalse(character.hasActions);
            Assert.AreEqual(HexMapState.None, hexMap.hexMapState);
        }
    }
}