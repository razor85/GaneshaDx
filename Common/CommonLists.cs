﻿using System.Collections.Generic;

namespace GaneshaDx.Common {
	public static class CommonLists {
		public static readonly List<MeshType> MeshTypes = new List<MeshType> {
			MeshType.PrimaryMesh,
			MeshType.AnimatedMesh1,
			MeshType.AnimatedMesh2,
			MeshType.AnimatedMesh3,
			MeshType.AnimatedMesh4,
			MeshType.AnimatedMesh5,
			MeshType.AnimatedMesh6,
			MeshType.AnimatedMesh7,
			MeshType.AnimatedMesh8,
		};

		public static readonly List<PolygonType> PolygonTypes = new List<PolygonType> {
			PolygonType.TexturedTriangle,
			PolygonType.TexturedQuad,
			PolygonType.UntexturedTriangle,
			PolygonType.UntexturedQuad
		};

		public static readonly Dictionary<int, TerrainSurfaceType> TerrainSurfaceTypes =
			new Dictionary<int, TerrainSurfaceType> {
				{0, TerrainSurfaceType.NaturalSurface},
				{1, TerrainSurfaceType.SandArea},
				{2, TerrainSurfaceType.Stalactite},
				{3, TerrainSurfaceType.Grassland},
				{4, TerrainSurfaceType.Thicket},
				{5, TerrainSurfaceType.Snow},
				{6, TerrainSurfaceType.RockyCliff},
				{7, TerrainSurfaceType.Gravel},
				{8, TerrainSurfaceType.Wasteland},
				{9, TerrainSurfaceType.Swamp},
				{10, TerrainSurfaceType.Marsh},
				{11, TerrainSurfaceType.PoisonedMarsh},
				{12, TerrainSurfaceType.LavaRocks},
				{13, TerrainSurfaceType.Ice},
				{14, TerrainSurfaceType.Waterway},
				{15, TerrainSurfaceType.River},
				{16, TerrainSurfaceType.Lake},
				{17, TerrainSurfaceType.Sea},
				{18, TerrainSurfaceType.Lava},
				{19, TerrainSurfaceType.Road},
				{20, TerrainSurfaceType.WoodenFloor},
				{21, TerrainSurfaceType.StoneFloor},
				{22, TerrainSurfaceType.Roof},
				{23, TerrainSurfaceType.StoneWall},
				{24, TerrainSurfaceType.Sky},
				{25, TerrainSurfaceType.Darkness},
				{26, TerrainSurfaceType.Salt},
				{27, TerrainSurfaceType.Book},
				{28, TerrainSurfaceType.Obstacle},
				{29, TerrainSurfaceType.Rug},
				{30, TerrainSurfaceType.Tree},
				{31, TerrainSurfaceType.Box},
				{32, TerrainSurfaceType.Brick},
				{33, TerrainSurfaceType.Chimney},
				{34, TerrainSurfaceType.MudWall},
				{35, TerrainSurfaceType.Bridge},
				{36, TerrainSurfaceType.WaterPlant},
				{37, TerrainSurfaceType.Stairs},
				{38, TerrainSurfaceType.Furniture},
				{39, TerrainSurfaceType.Ivy},
				{40, TerrainSurfaceType.Deck},
				{41, TerrainSurfaceType.Machine},
				{42, TerrainSurfaceType.IronPlate},
				{43, TerrainSurfaceType.Moss},
				{44, TerrainSurfaceType.Tombstone},
				{45, TerrainSurfaceType.Waterfall},
				{46, TerrainSurfaceType.Coffin},
				{47, TerrainSurfaceType.UnknownA},
				{48, TerrainSurfaceType.UnknownB},
				{63, TerrainSurfaceType.CrossSection},
			};

		public static readonly Dictionary<int, TerrainSlopeType> TerrainSlopeTypes =
			new Dictionary<int, TerrainSlopeType> {
				{0, TerrainSlopeType.Flat},
				{133, TerrainSlopeType.InclineNorth},
				{82, TerrainSlopeType.InclineEast},
				{37, TerrainSlopeType.InclineSouth},
				{88, TerrainSlopeType.InclineWest},
				{65, TerrainSlopeType.ConvexNortheast},
				{17, TerrainSlopeType.ConvexSoutheast},
				{20, TerrainSlopeType.ConvexSouthwest},
				{68, TerrainSlopeType.ConvexNorthwest},
				{150, TerrainSlopeType.ConcaveNortheast},
				{102, TerrainSlopeType.ConcaveSoutheast},
				{105, TerrainSlopeType.ConcaveSouthwest},
				{153, TerrainSlopeType.ConcaveNorthwest},
			};
	}
}