﻿using System.Collections.Generic;
using GaneshaDx.Common;
using GaneshaDx.Resources;
using GaneshaDx.UserInterface.GuiDefinitions;
using ImGuiNET;

namespace GaneshaDx.UserInterface.GuiForms {
	public static class GuiPanelMesh {
		public static MeshType SelectedMesh;
		private static int _selectedComboId = 0;

		public static void Render() {
			if (!CurrentMapState.StateData.HasAnimatedMesh1 &&
			    !CurrentMapState.StateData.HasAnimatedMesh2 &&
			    !CurrentMapState.StateData.HasAnimatedMesh3 &&
			    !CurrentMapState.StateData.HasAnimatedMesh4 &&
			    !CurrentMapState.StateData.HasAnimatedMesh5 &&
			    !CurrentMapState.StateData.HasAnimatedMesh6 &&
			    !CurrentMapState.StateData.HasAnimatedMesh7 &&
			    !CurrentMapState.StateData.HasAnimatedMesh8
			) {
				SelectedMesh = MeshType.PrimaryMesh;
				return;
			}

			GuiStyle.SetNewUiToDefaultStyle();
			ImGui.Indent();
			ImGui.Columns(2, "MeshSelectionColumns", false);
			ImGui.SetColumnWidth(0, GuiStyle.LabelWidth - 20);
			ImGui.SetColumnWidth(1, GuiStyle.WidgetWidth + 30);

			ImGui.Text("Mesh");
			ImGui.NextColumn();

			List<MeshType> meshTypes = new List<MeshType> {MeshType.PrimaryMesh};
			List<string> labels = new List<string> {"Primary Mesh"};

			if (CurrentMapState.StateData.HasAnimatedMesh1) {
				meshTypes.Add(MeshType.AnimatedMesh1);
				labels.Add("Animated Mesh 1");
			}

			if (CurrentMapState.StateData.HasAnimatedMesh2) {
				meshTypes.Add(MeshType.AnimatedMesh2);
				labels.Add("Animated Mesh 2");
			}

			if (CurrentMapState.StateData.HasAnimatedMesh3) {
				meshTypes.Add(MeshType.AnimatedMesh3);
				labels.Add("Animated Mesh 3");
			}

			if (CurrentMapState.StateData.HasAnimatedMesh4) {
				meshTypes.Add(MeshType.AnimatedMesh4);
				labels.Add("Animated Mesh 4");
			}

			if (CurrentMapState.StateData.HasAnimatedMesh5) {
				meshTypes.Add(MeshType.AnimatedMesh5);
				labels.Add("Animated Mesh 5");
			}

			if (CurrentMapState.StateData.HasAnimatedMesh6) {
				meshTypes.Add(MeshType.AnimatedMesh6);
				labels.Add("Animated Mesh 6");
			}

			if (CurrentMapState.StateData.HasAnimatedMesh7) {
				meshTypes.Add(MeshType.AnimatedMesh7);
				labels.Add("Animated Mesh 7");
			}

			if (CurrentMapState.StateData.HasAnimatedMesh8) {
				meshTypes.Add(MeshType.AnimatedMesh8);
				labels.Add("Animated Mesh 8");
			}

			ImGui.SetNextItemWidth(GuiStyle.WidgetWidth + 20);
			ImGui.Combo("##SelectedMeshType", ref _selectedComboId, labels.ToArray(), meshTypes.Count);
			SelectedMesh = meshTypes[_selectedComboId];

			ImGui.NextColumn();

			bool shouldAnimate = true;
			ImGui.Text("Animate Meshes");
			ImGui.NextColumn();
			ImGui.Checkbox("##ShouldAnimateMeshes", ref shouldAnimate);
			ImGui.NextColumn();
			
			bool isolateMeshes = true;
			ImGui.Text("Isolate Mesh");
			ImGui.NextColumn();
			ImGui.Checkbox("##ShouldIsolateMesh", ref isolateMeshes);

			ImGui.Columns(1);
			GuiStyle.AddSpace();
			ImGui.Unindent();
		}
	}
}