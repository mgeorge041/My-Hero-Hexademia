using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Map.UTests;
using Characters.UTests;
using Characters.ITests;

namespace Map.ITests
{
    public class HexMapPathingTests
    {
        private HexMap hexMap;
        private Character character1;
        private Character character2;
        private Hex centerHex;

        // Setup
        [SetUp]
        public void Setup()
        {
            hexMap = HexMapUTests.CreateTestHexMap(2);
            character1 = CharacterITests.CreateTestCharacterWithPlayer(1);
            character2 = CharacterITests.CreateTestCharacterWithPlayer(2);
            centerHex = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            centerHex.AddCharacter(character1);
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(hexMap);
            C.Destroy(character1);
            C.Destroy(character2);
        }

        // Test basic path - move 1 up
        [Test]
        public void Pathfinds_1_Hex_Up()
        {
            Hex targetHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            Assert.AreEqual(2, path.Count);
            Assert.Contains(targetHex, path);
            Assert.Contains(centerHex, path);
        }

        // Test basic path - move 2 up
        [Test]
        public void Pathfinds_2_Hex_Up()
        {
            Hex middleHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            Hex targetHex = hexMap.GetHexAtHexCoords(middleHex.GetNeighborCoords(0));
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            Assert.AreEqual(3, path.Count);
            Assert.Contains(targetHex, path);
            Assert.Contains(middleHex, path);
            Assert.Contains(centerHex, path);
        }

        // Test blocked path - move 1 up
        [Test]
        public void Pathfinds_1_Hex_Up_Blocked()
        {
            Hex targetHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            targetHex.character = character2;
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            Assert.IsNull(path);
        }

        // Test blocked path - move 2 up
        [Test]
        public void Pathfinds_2_Hex_Up_Blocked()
        {
            Hex middleHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            middleHex.character = character2;
            Hex targetHex = hexMap.GetHexAtHexCoords(middleHex.GetNeighborCoords(0));
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            Assert.AreEqual(4, path.Count);
            Assert.Contains(targetHex, path);
            Assert.Contains(centerHex, path);
        }

        // Test can't move past speed
        [Test]
        public void Pathfinds_2_Hex_Up_TooFar()
        {
            character1.remainingSpeed = 1;
            Hex middleHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            middleHex.AddCharacter(character2);
            Hex targetHex = hexMap.GetHexAtHexCoords(middleHex.GetNeighborCoords(0));
            List<Hex> path = hexMap.GetMaxPath(centerHex);
            foreach (Hex v3 in path)
                Debug.Log("v3: " + v3.hexCoords);
            Assert.AreEqual(6, path.Count);
        }

        // Test decrements speed count
        [Test]
        public void DecrementsSpeed_DefaultTile_1()
        {
            Hex targetHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            int startSpeed = character1.remainingSpeed;
            character1.MoveCharacter(path);
            Assert.AreEqual(startSpeed - 1, character1.remainingSpeed);
        }

        // Test decrements speed count
        [Test]
        public void DecrementsSpeed_SlowTile_3()
        {
            Hex targetHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            targetHex.hexStats = HexStats.LoadHexStats("mud");
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            int startSpeed = character1.remainingSpeed;
            character1.MoveCharacter(path);
            Assert.AreEqual(startSpeed - 3, character1.remainingSpeed);
        }

        // Test decrements speed count
        [Test]
        public void DecrementsSpeed_DefaultAndSlowTile_4()
        {
            Hex targetHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            targetHex.hexStats = HexStats.LoadHexStats("mud");
            targetHex = hexMap.GetHexAtHexCoords(targetHex.GetNeighborCoords(0));
            List<Hex> path = hexMap.GetPath(centerHex, targetHex);
            int startSpeed = character1.remainingSpeed;
            character1.MoveCharacter(path);
            Assert.AreEqual(startSpeed - 4, character1.remainingSpeed);
        }

        // Test blocked movement - can't move
        [Test]
        public void PathfindsRange_CantMove()
        {
            character1.remainingSpeed = 0;
            List<Hex> maxPath = hexMap.GetMaxPath(centerHex);
            Assert.AreEqual(0, maxPath.Count);
        }

        // Test set movement tiles
        [Test]
        public void SetsMovementTiles_RemainingSpeed1()
        {
            character1.remainingSpeed = 1;
            List<Hex> maxPath = hexMap.GetMaxPath(centerHex);
            hexMap.PaintPath(maxPath);
            Assert.AreEqual(6, hexMap.actionTileCoords.Count);
        }

        // Test set movement tiles
        [Test]
        public void SetsMovementTiles_RemainingSpeed1_1SlowTile()
        {
            character1.remainingSpeed = 1;
            Hex topHex = hexMap.GetHexAtHexCoords(centerHex.GetNeighborCoords(0));
            topHex.hexStats = HexStats.LoadHexStats("mud");
            List<Hex> maxPath = hexMap.GetMaxPath(centerHex);
            hexMap.PaintPath(maxPath);
            Assert.AreEqual(5, hexMap.actionTileCoords.Count);
        }
    }
}