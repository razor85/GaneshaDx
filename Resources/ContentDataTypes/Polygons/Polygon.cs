﻿using System.Collections.Generic;
using GaneshaDx.Common;
using GaneshaDx.Environment;
using GaneshaDx.Rendering;
using GaneshaDx.UserInterface;
using GaneshaDx.UserInterface.GuiDefinitions;
using GaneshaDx.UserInterface.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GaneshaDx.Resources.ContentDataTypes.Polygons {
	public class Polygon {
		public PolygonType PolygonType;
		public MeshType MeshType;
		public List<Vertex> Vertices;
		public List<Vector2> UvCoordinates;
		public int PaletteId;
		public int TexturePage;
		public int TerrainX;
		public int TerrainZ;
		public int TerrainLevel;
		public PolygonRenderingProperties RenderingProperties;

		private VertexPositionNormalTexture[] _renderVertices;
		private Texture2D _texture2D;
		private readonly List<VertexIndicator> _vertexIndicators = new List<VertexIndicator>();

		public bool IsTextured => PolygonType == PolygonType.TexturedQuad ||
		                          PolygonType == PolygonType.TexturedTriangle;

		public bool IsQuad => PolygonType == PolygonType.TexturedQuad ||
		                      PolygonType == PolygonType.UntexturedQuad;

		public bool IsSelected => Selection.SelectedPolygons.Contains(this);

		public bool IsHovered => Selection.HoveredPolygons.Count > 0 && Selection.HoveredPolygons[0] == this;

		public Vector3 AveragePoint {
			get {
				List<Vector3> adjustedVerts = new List<Vector3>();
				foreach (Vertex vertex in Vertices) {
					adjustedVerts.Add(vertex.CurrentAnimatedPosition);
				}

				return Utilities.GetAveragePoint(adjustedVerts);
			}
		}

		public double DistanceToCamera => Vector3.Distance(StageCamera.CamPosition, AveragePoint);

		public void Render() {
			SetRenderVertices();
			SetTexture();
			SetPolygonEffect();

			Stage.PolygonVertexBuffer.SetData(_renderVertices);
			Stage.GraphicsDevice.SetVertexBuffer(Stage.PolygonVertexBuffer);
			Stage.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
			Stage.GraphicsDevice.BlendState = BlendState.NonPremultiplied;

			Stage.BasicEffect.Alpha = Gui.SelectedTab == RightPanelTab.Terrain
				? Configuration.Properties.PolygonTransparencyForTerrainEditing / 100f
				: 1f;

			Stage.BasicEffect.Texture = _texture2D;
			Stage.BasicEffect.TextureEnabled = true;
			Stage.BasicEffect.VertexColorEnabled = false;

			foreach (EffectPass pass in IsTextured
				? Stage.FftPolygonEffect.CurrentTechnique.Passes
				: Stage.BasicEffect.CurrentTechnique.Passes
			) {
				pass.Apply();
				Stage.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, IsQuad ? 2 : 1);
			}
		}

		public void RenderVertexIndicators() {
			foreach (VertexIndicator vertexIndicator in _vertexIndicators) {
				vertexIndicator.Render();
			}
		}

		public void SetNewPosition(Vector3 newPosition) {
			Vector3 offset = newPosition - Vertices[0].Position;
			foreach (Vertex vertex in Vertices) {
				vertex.Position += offset;
			}
		}

		public void SetNewVertexPosition(int vertexIndex, Vector3 newPosition) {
			Vector3 offset = newPosition - Vertices[vertexIndex].Position;
			Vertices[vertexIndex].Position += offset;
		}

		public void RotateVerticesClockwise() {
			if (IsQuad) {
				Vector3 temp = Vertices[0].Position;
				Vertices[0].Position = Vertices[1].Position;
				Vertices[1].Position = Vertices[3].Position;
				Vertices[3].Position = Vertices[2].Position;
				Vertices[2].Position = temp;
			} else {
				Vector3 temp = Vertices[0].Position;
				Vertices[0].Position = Vertices[1].Position;
				Vertices[1].Position = Vertices[2].Position;
				Vertices[2].Position = temp;
			}
		}

		public void RotateVerticesCounterClockwise() {
			if (IsQuad) {
				Vector3 temp = Vertices[0].Position;
				Vertices[0].Position = Vertices[2].Position;
				Vertices[2].Position = Vertices[3].Position;
				Vertices[3].Position = Vertices[1].Position;
				Vertices[1].Position = temp;
			} else {
				Vector3 temp = Vertices[2].Position;
				Vertices[2].Position = Vertices[1].Position;
				Vertices[1].Position = Vertices[0].Position;
				Vertices[0].Position = temp;
			}
		}

		public void FlipNormals() {
			Vector3 temp = Vertices[0].Position;
			Vertices[0].Position = Vertices[1].Position;
			Vertices[1].Position = temp;

			if (IsQuad) {
				temp = Vertices[2].Position;
				Vertices[2].Position = Vertices[3].Position;
				Vertices[3].Position = temp;
			}
		}

		public void FlipUvsHorizontally() {
			if (IsTextured) {
				Vector2 tempUv = UvCoordinates[0];
				UvCoordinates[0] = UvCoordinates[1];
				UvCoordinates[1] = tempUv;

				if (IsQuad) {
					tempUv = UvCoordinates[2];
					UvCoordinates[2] = UvCoordinates[3];
					UvCoordinates[3] = tempUv;
				}
			}
		}

		public void FlipUvsVertically() {
			if (IsTextured) {
				Vector2 tempUv = UvCoordinates[0];
				UvCoordinates[0] = UvCoordinates[2];
				UvCoordinates[2] = tempUv;

				if (IsQuad) {
					tempUv = UvCoordinates[3];
					UvCoordinates[3] = UvCoordinates[1];
					UvCoordinates[1] = tempUv;
				}
			}
		}

		public void RotateUvsClockwise() {
			if (IsTextured) {
				if (IsQuad) {
					Vector2 tempUv = UvCoordinates[2];
					UvCoordinates[2] = UvCoordinates[3];
					UvCoordinates[3] = UvCoordinates[1];
					UvCoordinates[1] = UvCoordinates[0];
					UvCoordinates[0] = tempUv;
				} else {
					Vector2 tempUv = UvCoordinates[0];
					UvCoordinates[0] = UvCoordinates[1];
					UvCoordinates[1] = UvCoordinates[2];
					UvCoordinates[2] = tempUv;
				}
			}
		}

		public void RotateUvsCounterClockwise() {
			if (IsTextured) {
				if (IsQuad) {
					Vector2 tempUv = UvCoordinates[0];
					UvCoordinates[0] = UvCoordinates[1];
					UvCoordinates[1] = UvCoordinates[3];
					UvCoordinates[3] = UvCoordinates[2];
					UvCoordinates[2] = tempUv;
				} else {
					Vector2 tempUv = UvCoordinates[0];
					UvCoordinates[0] = UvCoordinates[2];
					UvCoordinates[2] = UvCoordinates[1];
					UvCoordinates[1] = tempUv;
				}
			}
		}

		public void GuessNormals() {
			Vector3 vertexA = Vertices[0].Position;
			Vector3 vertexB = Vertices[1].Position;
			Vector3 vertexC = Vertices[2].Position;

			Vector3 v1 = new Vector3(vertexC.X - vertexA.X, vertexC.Y - vertexA.Y, vertexC.Z - vertexA.Z);
			Vector3 v2 = new Vector3(vertexB.X - vertexA.X, vertexB.Y - vertexA.Y, vertexB.Z - vertexA.Z);

			Vector3 normal = new Vector3(
				v1.Y * v2.Z - v1.Z * v2.Y,
				v1.Z * v2.X - v1.X * v2.Z,
				v1.X * v2.Y - v1.Y * v2.X
			);

			(double elevation, double azimuth) = Utilities.VectorToSphere(normal);

			foreach (Vertex vertex in Vertices) {
				vertex.NormalElevation = (float) elevation;
				vertex.NormalAzimuth = (float) azimuth;
			}
		}

		public Polygon CreateClone() {
			Polygon newPolygon = new Polygon {
				MeshType = MeshType,
				PaletteId = PaletteId,
				PolygonType = PolygonType,
				RenderingProperties = RenderingProperties == null
					? null
					: new PolygonRenderingProperties(RenderingProperties.RawData),
				TerrainLevel = TerrainLevel,
				TerrainX = TerrainX,
				TerrainZ = TerrainZ,
				TexturePage = TexturePage,
				Vertices = new List<Vertex>()
			};

			foreach (Vertex vertex in Vertices) {
				newPolygon.Vertices.Add(new Vertex(
					new Vector3(vertex.Position.X, vertex.Position.Y, vertex.Position.Z),
					vertex.Color,
					vertex.UsesNormal,
					vertex.NormalAzimuth,
					vertex.NormalElevation
				));
			}

			if (IsTextured) {
				newPolygon.UvCoordinates = new List<Vector2>();

				foreach (Vector2 coordinates in UvCoordinates) {
					newPolygon.UvCoordinates.Add(new Vector2(coordinates.X, coordinates.Y));
				}
			}

			return newPolygon;
		}

		public List<Polygon> Break() {
			if (!IsQuad) {
				return new List<Polygon>();
			}

			Polygon newPolygonA = new Polygon {
				MeshType = MeshType,
				PaletteId = PaletteId,
				PolygonType = IsTextured ? PolygonType.TexturedTriangle : PolygonType.UntexturedTriangle,
				RenderingProperties = new PolygonRenderingProperties(RenderingProperties.RawData),
				TerrainLevel = TerrainLevel,
				TerrainX = TerrainX,
				TerrainZ = TerrainZ,
				TexturePage = TexturePage,
				Vertices = new List<Vertex>()
			};

			Polygon newPolygonB = new Polygon {
				MeshType = MeshType,
				PaletteId = PaletteId,
				PolygonType = IsTextured ? PolygonType.TexturedTriangle : PolygonType.UntexturedTriangle,
				RenderingProperties = new PolygonRenderingProperties(RenderingProperties.RawData),
				TerrainLevel = TerrainLevel,
				TerrainX = TerrainX,
				TerrainZ = TerrainZ,
				TexturePage = TexturePage,
				Vertices = new List<Vertex>()
			};

			newPolygonA.Vertices.Add(CloneVertex(0, Color.Red));
			newPolygonA.Vertices.Add(CloneVertex(1, Color.Green));
			newPolygonA.Vertices.Add(CloneVertex(2, Color.Blue));

			newPolygonB.Vertices.Add(CloneVertex(1, Color.Red));
			newPolygonB.Vertices.Add(CloneVertex(3, Color.Green));
			newPolygonB.Vertices.Add(CloneVertex(2, Color.Blue));

			if (IsTextured) {
				newPolygonA.UvCoordinates = new List<Vector2> {
					new Vector2(UvCoordinates[0].X, UvCoordinates[0].Y),
					new Vector2(UvCoordinates[1].X, UvCoordinates[1].Y),
					new Vector2(UvCoordinates[2].X, UvCoordinates[2].Y)
				};

				newPolygonB.UvCoordinates = new List<Vector2> {
					new Vector2(UvCoordinates[1].X, UvCoordinates[1].Y),
					new Vector2(UvCoordinates[3].X, UvCoordinates[3].Y),
					new Vector2(UvCoordinates[2].X, UvCoordinates[2].Y)
				};

				CurrentMapState.StateData.PolygonCollection[MeshType][PolygonType.TexturedQuad].Remove(this);
				CurrentMapState.StateData.PolygonCollection[MeshType][PolygonType.TexturedTriangle].Add(newPolygonA);
				CurrentMapState.StateData.PolygonCollection[MeshType][PolygonType.TexturedTriangle].Add(newPolygonB);
			} else {
				CurrentMapState.StateData.PolygonCollection[MeshType][PolygonType.UntexturedQuad].Remove(this);
				CurrentMapState.StateData.PolygonCollection[MeshType][PolygonType.UntexturedTriangle].Add(newPolygonA);
				CurrentMapState.StateData.PolygonCollection[MeshType][PolygonType.UntexturedTriangle].Add(newPolygonB);
			}

			return new List<Polygon> {newPolygonA, newPolygonB};
		}

		public void SetLastAnimatedPositionsForVertices(Vector3 newRotation) {
			Matrix totalRotation = Matrix.CreateRotationX(MathHelper.ToRadians(newRotation.X)) *
			                       Matrix.CreateRotationY(MathHelper.ToRadians(newRotation.Y)) *
			                       Matrix.CreateRotationZ(MathHelper.ToRadians(newRotation.Z));

			foreach (Vertex vertex in Vertices) {
				vertex.LastAnimatedStartPosition = Vector3.Transform(vertex.LastAnimatedStartPosition, totalRotation);
			}
		}

		private Vertex CloneVertex(int vertexIndex, Color newColor) {
			return new Vertex(
				new Vector3(
					Vertices[vertexIndex].Position.X,
					Vertices[vertexIndex].Position.Y,
					Vertices[vertexIndex].Position.Z
				),
				newColor,
				Vertices[vertexIndex].UsesNormal,
				Vertices[vertexIndex].NormalAzimuth,
				Vertices[vertexIndex].NormalElevation
			);
		}

		private void SetTexture() {
			_texture2D = IsTextured switch {
				true when !Configuration.Properties.RenderPolygonsInLightingMode => CurrentMapState.StateData.Texture,
				true when Configuration.Properties.RenderPolygonsInLightingMode => UniversalTextures.GreyTexture,
				false => IsSelected ? UniversalTextures.SelectedBlackTexture :
					IsHovered ? UniversalTextures.HoveredBlackTexture : UniversalTextures.BlackTexture,
				_ => _texture2D
			};
		}

		private void SetRenderVertices() {
			_renderVertices = new VertexPositionNormalTexture[IsQuad ? 4 : 3];

			_renderVertices[0] = BuildVertex(0);
			_renderVertices[1] = BuildVertex(1);
			_renderVertices[2] = BuildVertex(2);

			if (IsQuad) {
				_renderVertices[3] = BuildVertex(3);
			}

			UpdateVertexIndicator();
		}

		private void UpdateVertexIndicator() {
			if (_vertexIndicators.Count != (IsQuad ? 4 : 3)) {
				_vertexIndicators.Clear();
				_vertexIndicators.Add(new VertexIndicator(Vertices[0]));
				_vertexIndicators.Add(new VertexIndicator(Vertices[1]));
				_vertexIndicators.Add(new VertexIndicator(Vertices[2]));
				if (IsQuad) {
					_vertexIndicators.Add(new VertexIndicator(Vertices[3]));
				}
			}
		}

		private VertexPositionNormalTexture BuildVertex(int vertexIndex) {
			Vector3 vertexPosition = Vertices[vertexIndex].LastAnimatedStartPosition;
			Vector3 normal = Utilities.SphereToVector(
				Vertices[vertexIndex].NormalElevation,
				Vertices[vertexIndex].NormalAzimuth
			);

			if (MeshType != MeshType.PrimaryMesh) {
				vertexPosition = MeshAnimationController.GetAnimatedVertexOffset(MeshType, vertexPosition);
			}

			Vertices[vertexIndex].CurrentAnimatedPosition = vertexPosition;

			if (IsTextured) {
				double adjustedX = UvCoordinates[vertexIndex].X / 256f;
				double adjustedY = (UvCoordinates[vertexIndex].Y + 256 * TexturePage) / 1024f;

				Vector2 adjustedCoordinates = new Vector2((float) adjustedX, (float) adjustedY);
				return new VertexPositionNormalTexture(vertexPosition, normal, adjustedCoordinates);
			}

			return new VertexPositionNormalTexture(vertexPosition, normal, Vector2.Zero);
		}

		private void SetPolygonEffect() {
			Stage.FftPolygonEffect.Parameters["ModelTexture"].SetValue(_texture2D);

			Color ambientColor = CurrentMapState.StateData.AmbientLightColor;
			Stage.FftPolygonEffect.Parameters["AmbientColor"].SetValue(ambientColor.ToVector4());

			for (int lightIndex = 0; lightIndex < 3; lightIndex++) {
				DirectionalLight light = CurrentMapState.StateData.DirectionalLights[lightIndex];
				Vector4 lightColor = light.LightColor.ToVector4();
				Vector3 lightDirection = Utilities.SphereToVector(
					CurrentMapState.StateData.DirectionalLights[lightIndex].DirectionElevation,
					CurrentMapState.StateData.DirectionalLights[lightIndex].DirectionAzimuth
				);

				lightDirection.Normalize();

				Stage.FftPolygonEffect.Parameters["DirectionalLightDirection" + lightIndex].SetValue(lightDirection);
				Stage.FftPolygonEffect.Parameters["DirectionalLightColor" + lightIndex].SetValue(lightColor);
			}

			Vector4[] shaderColors = SceneRenderer.AnimationAdjustedPalettes[PaletteId].ShaderColors;

			if (Configuration.Properties.RenderPolygonsInLightingMode) {
				for (int colorIndex = 0; colorIndex < 16; colorIndex++) {
					shaderColors[colorIndex] = new Vector4(0.5f, 0.5f, 0.5f, 1);
				}
			}

			Stage.FftPolygonEffect.Parameters["PaletteColors"].SetValue(shaderColors);
			Stage.FftPolygonEffect.Parameters["HighlightBright"].SetValue(false);
			Stage.FftPolygonEffect.Parameters["HighlightDim"].SetValue(false);

			if (!Stage.ScreenshotMode) {
				if (IsSelected && this == Selection.SelectedPolygons[0]) {
					bool shouldHighlight = Gui.SelectedTab != RightPanelTab.Texture ||
					                       Configuration.Properties.HighlightSelectionOnTexturePage;

					Stage.FftPolygonEffect.Parameters["HighlightBright"].SetValue(shouldHighlight);
				} else if (IsHovered || IsSelected && this != Selection.SelectedPolygons[0]) {
					Stage.FftPolygonEffect.Parameters["HighlightDim"].SetValue(true);
				}
			}

			float maxAlpha = Configuration.Properties.PolygonTransparencyForTerrainEditing / 100f;

			Stage.FftPolygonEffect.Parameters["MaxAlpha"].SetValue(
				Gui.SelectedTab == RightPanelTab.Terrain
					? maxAlpha
					: 1f
			);
		}
	}
}