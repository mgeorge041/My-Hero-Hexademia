using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Map.UTests
{
    public class HexMapUTests
    {
        private HexMap hexMap;

        // Create test hex map
        public static HexMap CreateTestHexMap()
        {
            HexMap newHexMap = TestFunctions.CreateClassObject<HexMap>("Assets/Resources/Prefabs/Map/Hexmap.prefab");
            newHexMap.Initialize();
            return newHexMap;
        }

        // Create test hex map
        public static HexMap CreateTestHexMap(int mapRadius)
        {
            HexMap newHexMap = TestFunctions.CreateClassObject<HexMap>("Assets/Resources/Prefabs/Map/Hexmap.prefab");
            newHexMap.Initialize(mapRadius);
            return newHexMap;
        }

        // Setup
        [SetUp]
        public void Setup()
        {
            hexMap = CreateTestHexMap();
        }

        // Teardown
        [TearDown]
        public void Teardown()
        {
            C.Destroy(hexMap);
        }

        // Test hex map created
        [Test]
        public void HexMapCreated()
        {
            Assert.IsNotNull(hexMap);
        }

        // Test hex coords dict correct number of hexes
        [Test]
        public void HexCoordsDictHasHexes_Default()
        {
            Assert.AreEqual(91, hexMap.mapSize);
        }

        // Test hex coords dict correct number of hexes
        [Test]
        public void HexCoordsDictHasHexes_Radius_3()
        {
            HexMap newHexMap = TestFunctions.CreateClassObject<HexMap>("Assets/Resources/Prefabs/Map/Hexmap.prefab");
            newHexMap.Initialize(3);
            Assert.AreEqual(37, newHexMap.mapSize);
        }

        // Test gets hex at hex coords
        [Test]
        public void GetsHexAtHexCoords_Exists()
        {
            Hex hex = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Assert.IsNotNull(hex);
        }

        // Test gets hex at hex coords
        [Test]
        public void GetsHexAtHexCoords_NotExists()
        {
            Hex hex = hexMap.GetHexAtHexCoords(new Vector3Int(1, 1, 1));
            Assert.IsNull(hex);
        }

        // Test gets hex at tile coords
        [Test]
        public void GetsHexAtTileCoords_Exists()
        {
            Hex hex = hexMap.GetHexAtTileCoords(Vector3Int.zero);
            Assert.IsNotNull(hex);
        }

        // Test gets hex at tile coords
        [Test]
        public void GetsHexAtTileCoords_NotExists()
        {
            Hex hex = hexMap.GetHexAtTileCoords(new Vector3Int(1, 1, 1));
            Assert.IsNull(hex);
        }

        // Test gets hex at world position
        [Test]
        public void GetsHexAtWorldPosition_Exists()
        {
            Hex hex = hexMap.GetHexAtWorldPosition(Vector3.zero);
            Assert.IsNotNull(hex);
        }

        // Test gets hex at world position
        [Test]
        public void GetsHexAtWorldPosition_NotExists()
        {
            Hex hex = hexMap.GetHexAtWorldPosition(new Vector3(1000, 1000, 1000));
            Assert.IsNull(hex);
        }

        // Test gets hex neighbors
        [Test]
        public void GetsHexNeighbors_Center()
        {
            Hex hex = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Vector3Int[] neighborCoords = hex.GetAllNeighborCoords();
            List<Hex> neighborHexes = hexMap.GetHexNeighbors(neighborCoords);
            Assert.AreEqual(6, neighborHexes.Count);
        }

        // Test gets hex neighbors
        [Test]
        public void GetsHexNeighbors_Edge()
        {
            Hex hex = hexMap.GetHexAtHexCoords(new Vector3Int(5, -5, 0));
            Vector3Int[] neighborCoords = hex.GetAllNeighborCoords();
            List<Hex> neighborHexes = hexMap.GetHexNeighbors(neighborCoords);
            Assert.AreEqual(3, neighborHexes.Count);
        }

        // Test get hexes within range
        [Test]
        public void GetsHexesWithinRange_Center()
        {
            Hex centerHex = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            List<Hex> hexes = hexMap.GetHexesWithinRange(centerHex, 2);
            Assert.AreEqual(19, hexes.Count);
        }

        // Test get hexes within range
        [Test]
        public void GetsHexesWithinRange_Edge()
        {
            Hex edgeHex = hexMap.GetHexAtHexCoords(new Vector3Int(hexMap.mapRadius, -hexMap.mapRadius, 0));
            List<Hex> hexes = hexMap.GetHexesWithinRange(edgeHex, 2);
            Assert.AreEqual(9, hexes.Count);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_XEqual_YLess()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(0, -2, 2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2);
            Assert.AreEqual(3, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_XEqual_YMore()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(0, 2, -2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2);
            Assert.AreEqual(3, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_XEqual_YLess_NoCenter()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(0, -2, 2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2, false);
            Assert.AreEqual(2, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_XEqual_YMore_NoCenter()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(0, 2, -2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2, false);
            Assert.AreEqual(2, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_YEqual_XLess()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(-2, 0, 2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2);
            Assert.AreEqual(3, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_YEqual_XMore()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(2, 0, -2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2);
            Assert.AreEqual(3, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_YEqual_XLess_NoCenter()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(-2, 0, 2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2, false);
            Assert.AreEqual(2, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_YEqual_XMore_NoCenter()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(2, 0, -2));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2, false);
            Assert.AreEqual(2, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_ZEqual_XLess()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(-2, 2, 0));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2);
            Assert.AreEqual(3, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_ZEqual_XMore()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(2, -2, 0));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2);
            Assert.AreEqual(3, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_ZEqual_XLess_NoCenter()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(-2, 2, 0));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2, false);
            Assert.AreEqual(2, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }

        // Test get hexes in line
        [Test]
        public void GetsHexesInLine_ZEqual_XMore_NoCenter()
        {
            Hex hex1 = hexMap.GetHexAtHexCoords(Vector3Int.zero);
            Hex hex2 = hexMap.GetHexAtHexCoords(new Vector3Int(2, -2, 0));
            List<Hex> hexesInLine = hexMap.GetHexesInLine(hex1, hex2, false);
            Assert.AreEqual(2, hexesInLine.Count);
            Assert.Contains(hex2, hexesInLine);
        }
    }
}