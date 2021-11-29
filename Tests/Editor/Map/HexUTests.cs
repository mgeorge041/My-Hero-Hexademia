using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Map.UTests
{
    public class HexUTests
    {
        private Vector3Int hexCoords;
        private Vector3Int tileCoords;
        private Hex hex;
        private Vector3Int[] neighborCoords;

        // Create test hex
        public static Hex CreateTestHex()
        {
            Hex newHex = new Hex(Vector3Int.zero);
            newHex.hexStats = Resources.Load<HexStats>("Map/Tiles/White Tile Stats");
            return newHex;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            hexCoords = Vector3Int.zero;
            tileCoords = Vector3Int.zero;
            hex = new Hex(Vector3Int.zero);

            neighborCoords = new Vector3Int[]
            {
            new Vector3Int(0, -1, 1),
            new Vector3Int(1, -1, 0),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 1, -1),
            new Vector3Int(-1, 1, 0),
            new Vector3Int(-1, 0, 1)
            };
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {

        }

        // Test resets
        [Test]
        public void Resets()
        {
            hex.Reset();
            Assert.IsNull(hex.character);
        }

        // Test convert hex to tile coords
        [Test]
        public void ConvertsHexToTileCoords_Center()
        {
            Vector3Int newTileCoords = Hex.HexToTileCoords(Vector3Int.zero);
            Vector3Int expectedTileCoords = Vector3Int.zero;
            Assert.AreEqual(expectedTileCoords, newTileCoords);
        }

        // Test convert hex to tile coords
        [Test]
        public void ConvertsHexToTileCoords_NonCenter()
        {
            hexCoords = new Vector3Int(1, -1, 0);
            Vector3Int newTileCoords = Hex.HexToTileCoords(hexCoords);
            Vector3Int expectedTileCoords = new Vector3Int(0, 1, 0);
            Assert.AreEqual(expectedTileCoords, newTileCoords);
        }

        // Test convert tile to hex coords
        [Test]
        public void ConvertsTileToHexCoords_Center()
        {
            Vector3Int newHexCoords = Hex.TileToHexCoords(Vector3Int.zero);
            Vector3Int expectedHexCoords = Vector3Int.zero;
            Assert.AreEqual(expectedHexCoords, newHexCoords);
        }

        // Test convert tile to hex coords
        [Test]
        public void ConvertsTileToHexCoords_NonCenter()
        {
            tileCoords = new Vector3Int(1, -1, 0);
            Vector3Int newHexCoords = Hex.TileToHexCoords(tileCoords);
            Vector3Int expectedHexCoords = new Vector3Int(-1, -1, 2);
            Assert.AreEqual(expectedHexCoords, newHexCoords);
        }

        // Test get distance between hexes
        [Test]
        public void GetsDistanceBetweenHexes_1()
        {
            Vector3Int hexCoords2 = new Vector3Int(1, -1, 0);
            int distance = Hex.GetDistanceHexCoords(hexCoords, hexCoords2);
            int expectedDistance = 1;
            Assert.AreEqual(expectedDistance, distance);
        }

        // Test get distance between hexes
        [Test]
        public void GetsDistanceBetweenHexes_2_Center()
        {
            Vector3Int hexCoords2 = new Vector3Int(2, -1, -1);
            int distance = Hex.GetDistanceHexCoords(hexCoords, hexCoords2);
            int expectedDistance = 2;
            Assert.AreEqual(expectedDistance, distance);
        }

        // Test get distance between hexes
        [Test]
        public void GetsDistanceBetweenHexes_2_NonCenter()
        {
            hexCoords = new Vector3Int(0, -1, 1);
            Vector3Int hexCoords2 = new Vector3Int(2, -1, -1);
            int distance = Hex.GetDistanceHexCoords(hexCoords, hexCoords2);
            int expectedDistance = 2;
            Assert.AreEqual(expectedDistance, distance);
        }

        // Test hex created
        [Test]
        public void HexCreated()
        {
            hex = CreateTestHex();
            Assert.IsNotNull(hex);
        }

        // Test get neighbor coords
        [Test]
        public void GetsNeighborCoords_Center()
        {
            Vector3Int[] newNeighborCoords = hex.GetAllNeighborCoords();
            Assert.AreEqual(neighborCoords, newNeighborCoords);
        }

        // Test get neighbor coords
        [Test]
        public void GetsNeighborCoords_NonCenter()
        {
            hex.hexCoords = new Vector3Int(1, -1, 0);
            Vector3Int[] newNeighborCoords = hex.GetAllNeighborCoords();
            Vector3Int[] expectedCoords = new Vector3Int[]
            {
            new Vector3Int(1, -2, 1),
            new Vector3Int(2, -2, 0),
            new Vector3Int(2, -1, -1),
            new Vector3Int(1, 0, -1),
            new Vector3Int(0, 0, 0),
            new Vector3Int(0, -1, 1)
            };
            Assert.AreEqual(expectedCoords, newNeighborCoords);
        }
    }
}