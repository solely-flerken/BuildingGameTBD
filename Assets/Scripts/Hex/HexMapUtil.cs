﻿using System.Collections.Generic;
using System.Linq;
using Input;
using Terrain;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Hex
{
    public static class HexMapUtil
    {
        private static readonly (int q, int r)[] NeighborOffsets =
        {
            (1, 0), (0, 1), (-1, 1),
            (-1, 0), (0, -1), (1, -1)
        };

        public static List<HexCoordinate> GetNeighbors(this Dictionary<HexCoordinate, HexCellData> hexMap,
            HexCoordinate coordinate)
        {
            var neighbors = new List<HexCoordinate>();

            foreach (var (dq, dr) in NeighborOffsets)
            {
                var neighborCoordinate = new HexCoordinate(coordinate.Q + dq, coordinate.R + dr);
                if (hexMap.ContainsKey(neighborCoordinate))
                {
                    neighbors.Add(neighborCoordinate);
                }
            }

            return neighbors;
        }

        // TODO: Refactor: Convert worldPosition into Hex Pos and then look for that in HexMap
        public static HexCoordinate? GetNearestHexCoordinate(this Dictionary<HexCoordinate, HexCellData> hexMap,
            Vector3 worldPosition)
        {
            var closestDistance = float.PositiveInfinity;
            HexCoordinate? closestCoordinate = null;

            foreach (var (coordinate, cellData) in hexMap)
            {
                var distance = math.distance(worldPosition, cellData.WorldPosition);

                if (distance >= closestDistance) continue;

                closestDistance = distance;
                closestCoordinate = coordinate;
            }

            return closestCoordinate;
        }

        public static HexCoordinate? GetNearestHexCoordinateToMousePosition(
            this Dictionary<HexCoordinate, HexCellData> hexMap)
        {
            var mouseWorldPosition = MouseUtils.MouseToWorldPosition(Vector3.up, CameraController.Camera);
            mouseWorldPosition.y = 0f;

            return GetNearestHexCoordinate(hexMap, mouseWorldPosition);
        }

        public static List<HexCoordinate> GetTissue(HexCoordinate origin, HexCoordinate[] offsets)
        {
            var tissueCoordinates = new List<HexCoordinate>(offsets.Length);
            tissueCoordinates.AddRange(offsets
                .Select(offset => new HexCoordinate(origin.Q + offset.Q, origin.R + offset.R)));
            // .Where(coordinate => HexMap.ContainsKey(coordinate)));

            return tissueCoordinates;
        }

        public static HexCoordinate[] RotateHexesClockwise(HexCoordinate origin, HexCoordinate[] hexes)
        {
            var rotatedHexes = new HexCoordinate[hexes.Length];

            for (var i = 0; i < hexes.Length; i++)
            {
                var q = hexes[i].Q - origin.Q;
                var r = hexes[i].R - origin.R;
                var s = -q - r;

                // Apply clockwise hex rotation (60 degrees)
                var newQ = -s;
                var newR = -q;

                rotatedHexes[i] = new HexCoordinate(newQ + origin.Q, newR + origin.R);
            }

            return rotatedHexes;
        }
    }
}