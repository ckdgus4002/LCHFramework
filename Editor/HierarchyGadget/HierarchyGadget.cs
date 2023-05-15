using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LCHFramework.Editor.HierarchyGadget
{
	public class HierarchyGadget
	{
		private const bool Enabled = true;
		private const int MaxIconNum = 5;
		private const int IconSize = 16;
		private const bool ShowActiveToggle = true;
		private const bool SqueezeWhenOverflow = false;
		
		
		
		private static readonly List<Type> HideTypes = new List<Type>
		{
			typeof(ParticleSystemRenderer),
		};
		private static Transform _offsetTransform;
		private static int _offset;
		
		
		
		[InitializeOnLoadMethod]
		public static void Init()
		{
			EditorApplication.hierarchyWindowItemOnGUI -= HierarchyWindowItemOnGUI;
			if (Enabled) EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
		}

		private static void HierarchyWindowItemOnGUI (int instanceID, Rect rect) 
		{
			// Check
			var @object = EditorUtility.InstanceIDToObject(instanceID);
			if (!@object) return;

			// fix rect
			rect.width += rect.x;
			rect.x = 0;

			// Logic
			var gameObject = @object as GameObject;
			var components = gameObject.GetComponents<Component>().ToList();
			for (var i = 0; i < components.Count; i++)
				if (components[i])
				{
					var isHiddenType = HideTypes.Any(item =>
					{
						var behaviourType = components[i].GetType();
						return item == behaviourType || behaviourType.IsSubclassOf(item);
					});
					if (isHiddenType)
					{
						components.RemoveAt(i);
						i--;
					}	
				}

			var maxIconNum = MaxIconNum;
			const int y = (18 - IconSize) / 2;
			var offset = gameObject.transform == _offsetTransform ? _offset : 0;
			var globalOffsetX = PrefabUtility.GetPrefabInstanceStatus(gameObject) == PrefabInstanceStatus.NotAPrefab ? 0 : /*-16*/0;

			// Active TG
			if (ShowActiveToggle)
			{
				globalOffsetX -= IconSize + 2;
				maxIconNum--;
			}

			// Has ...
			var hasMoreButton = !SqueezeWhenOverflow && maxIconNum < components.Count;
			if (hasMoreButton) maxIconNum--;
				
			// Main
			var oldColor = GUI.color;
			var deltaX = SqueezeWhenOverflow 
				// ? Mathf.Min(IconSize * ((float)maxIconNum / Mathf.Max(components.Count, 1)), IconSize)
				? IconSize
				: IconSize;
			for (var i = 0; i + offset < components.Count && (SqueezeWhenOverflow || i <= maxIconNum); i++)
			{
				var component = components[i + offset];
				var texture2d = AssetPreview.GetMiniThumbnail(component);
				if (texture2d) 
				{
					GUI.color = gameObject.activeInHierarchy && (!(component is Behaviour behaviour) || behaviour.enabled) ? Color.white : new Color(1, 1, 1, 0.4f);
					var guiRect = new Rect
					(
						rect.width - deltaX * i + globalOffsetX,
						rect.y + y,
						IconSize,
						IconSize
					);
					//GUI.Box(guiRect, GUIContent.none);
					GUI.DrawTexture(guiRect, texture2d);
				}
			}
			GUI.color = oldColor;

			// "..." Button
			if (hasMoreButton)
			{
				var moreButtonRect = new Rect
				(
					rect.width - (IconSize + 2) * (maxIconNum + 1) + globalOffsetX,
					rect.y + y,
					22,
					IconSize
				);
				var moreButtonStyle = new GUIStyle(GUI.skin.label) 
				{
					fontSize = 9,
					alignment = TextAnchor.MiddleCenter
				};
				if (GUI.Button(moreButtonRect, "•••", moreButtonStyle))
				{
					if (_offsetTransform != gameObject.transform) 
					{
						_offsetTransform = gameObject.transform;
						_offset = 0;
					}
					_offset += maxIconNum + 1;
					if (components.Count <= _offset) _offset = 0;
				}
			}

			if (ShowActiveToggle) 
			{
				// Active Toggle
				rect.x = rect.width;
				rect.width = rect.height;
				var active = GUI.Toggle(rect, gameObject.activeSelf, GUIContent.none);
				if (active != gameObject.activeSelf) 
				{
					gameObject.SetActive(active);
					EditorUtility.SetDirty(gameObject);
				}
			}
		}
	}
}