using System;
using System.Linq;
using LCHFramework.Extensions;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor
{
	public class HierarchyGadget
	{
		private const string EnableMenuItemPath = LCHFramework.MenuItemRootPath + "/" + "HierarchyGadget" + "/" + "Enabled";
		private const string ShowActiveToggleMenuItemPath = LCHFramework.MenuItemRootPath + "/" + "HierarchyGadget" + "/" + "ShowActiveToggle";
		private const string SqueezeWhenOverflowMenuItemPath = LCHFramework.MenuItemRootPath + "/" + "HierarchyGadget" + "/" + "SqueezeWhenOverflow";
		
		private static readonly string EnabledKey = $"{nameof(HierarchyGadget)}{nameof(Enabled)}";
		private static readonly string ShowActiveToggleKey = $"{nameof(HierarchyGadget)}{nameof(ShowActiveToggle)}";
		private static readonly string SqueezeWhenOverflowKey = $"{nameof(HierarchyGadget)}{nameof(SqueezeWhenOverflow)}";
		
		
		private static bool Enabled { get => EditorPrefs.GetBool(EnabledKey, true); set => EditorPrefs.SetBool(EnabledKey, value); }
		private static bool ShowActiveToggle { get => EditorPrefs.GetBool(ShowActiveToggleKey, true); set => EditorPrefs.SetBool(ShowActiveToggleKey, value); }
		private static bool SqueezeWhenOverflow { get => EditorPrefs.GetBool(SqueezeWhenOverflowKey); set => EditorPrefs.SetBool(SqueezeWhenOverflowKey, value); }
		
		
		
		[MenuItem(EnableMenuItemPath)] private static void EnableMenuItem() => Menu.SetChecked(EnableMenuItemPath, Enabled = !Enabled);
		
		[MenuItem(ShowActiveToggleMenuItemPath)] private static void ShowActiveToggleMenuItem() => Menu.SetChecked(ShowActiveToggleMenuItemPath, ShowActiveToggle = !ShowActiveToggle);
		
		[MenuItem(SqueezeWhenOverflowMenuItemPath)] private static void SqueezeWhenOverflowMenuItem() => Menu.SetChecked(SqueezeWhenOverflowMenuItemPath, SqueezeWhenOverflow = !SqueezeWhenOverflow);
		
		[InitializeOnLoadMethod] public static void InitializeOnLoad() { if (!EditorApplication.hierarchyWindowItemOnGUI.Contains((EditorApplication.HierarchyWindowItemCallback)HierarchyWindowItemOnGUI)) EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI; }

		private static Transform _offsetTransform;
		private static int _offset;
		private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
		{
			if (!Enabled) return;
			
			// Check.
			var gameObjectOrNull = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
			if (gameObjectOrNull == null) return;

			// Fix Rectangle.
			selectionRect.width += selectionRect.x;
			selectionRect.x = 0;

			// Get Components.
			var hiddenComponentTypes = new Type[] {};
			var components = gameObjectOrNull.GetComponents<Component>().Where(component => !hiddenComponentTypes.Any(hiddenComponentType =>
			{
				var componentType = component.GetType();
				return componentType == hiddenComponentType || hiddenComponentType.IsSubclassOf(componentType);
			})).ToArray();

			const int defaultMaxIconNumber = 7;
			var maxIconNumber = defaultMaxIconNumber;
			const int iconSize = 16;
			var offset = gameObjectOrNull.transform == _offsetTransform ? _offset : 0;
			var globalOffsetX = /*PrefabUtility.GetPrefabInstanceStatus(gameObjectOrNull) == PrefabInstanceStatus.NotAPrefab ? 0 : -16*/ 0;

			// Active Toggle.
			if (ShowActiveToggle)
			{
				globalOffsetX -= iconSize + 1;
				maxIconNumber--;
			}

			// Has "..." Button
			var hasMoreButton = !SqueezeWhenOverflow && maxIconNumber < components.Length;
			if (hasMoreButton) maxIconNumber--;
				
			// Main
			var prevColor = GUI.color;
			var deltaX = /*SqueezeWhenOverflow ? Mathf.Min(iconSize * ((float)maxIconNumber / Mathf.Max(components.Length, 1)), iconSize) : iconSize*/ iconSize;
			const int y = (18 - iconSize) / 2;
			for (var i = 0; i + offset < components.Length && (SqueezeWhenOverflow || i < maxIconNumber); i++)
			{
				var component = components[i + offset];
				var rect = new Rect(selectionRect.width - deltaX * i + globalOffsetX, selectionRect.y + y, iconSize, iconSize);
				var texture2d = AssetPreview.GetMiniThumbnail(component);
				if (texture2d == null) continue;
				
				GUI.color = gameObjectOrNull.activeInHierarchy && (component is not Behaviour behaviour || behaviour.enabled) ? Color.white : new Color(1, 1, 1, 0.4f);
				//GUI.Box(guiRect, GUIContent.none);
				GUI.DrawTexture(rect, texture2d);
			}
			GUI.color = prevColor;

			// "..." Button.
			if (hasMoreButton)
			{
				var moreButtonRect = new Rect(selectionRect.width - iconSize * (offset + maxIconNumber <= components.Length ? maxIconNumber : components.Length % maxIconNumber) + globalOffsetX - 1, selectionRect.y + y, iconSize, iconSize);
				var moreButtonStyle = new GUIStyle(GUI.skin.label) { fontSize = 9, alignment = TextAnchor.MiddleCenter };
				if (GUI.Button(moreButtonRect, "•••", moreButtonStyle))
				{
					_offset = _offsetTransform != gameObjectOrNull.transform || components.Length - maxIconNumber <= _offset ? 0 : _offset + maxIconNumber;
					_offsetTransform = gameObjectOrNull.transform;	
				}
			}

			// Active Toggle.
			if (ShowActiveToggle)
			{
				selectionRect.x = selectionRect.width;
				selectionRect.width = selectionRect.height;
				var active = GUI.Toggle(selectionRect, gameObjectOrNull.activeSelf, GUIContent.none);
				if (active != gameObjectOrNull.activeSelf)
				{
					gameObjectOrNull.SetActive(active);
					EditorUtility.SetDirty(gameObjectOrNull);	
				}
			}
		}
	}
}