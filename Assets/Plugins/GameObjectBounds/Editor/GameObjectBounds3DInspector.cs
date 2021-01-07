using System;
using UnityEditor;
using UnityEngine;

namespace GameObjectBounds.Editor
{
	[CustomEditor(typeof(GameObjectBounds3D), true)]
	public class GameObjectBounds3DInspector : UnityEditor.Editor
	{
		private readonly int[] _indices = {0, 1, 1, 2, 2, 3, 3, 0};

		public override void OnInspectorGUI()
		{
			var imin = serializedObject.FindProperty("_innerLeftBottomFront");
			var imax = serializedObject.FindProperty("_innerRightTopBack");
			EditorGUILayout.LabelField("Inner bounds", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(imin);
			EditorGUILayout.PropertyField(imax);
			DrawSeparator(Color.gray);
			var hasOuterBounds = serializedObject.FindProperty("_hasOuterBounds");
			EditorGUILayout.PropertyField(hasOuterBounds);
			var omin = serializedObject.FindProperty("_outerLeftBottomFront");
			var omax = serializedObject.FindProperty("_outerRightTopBack");
			EditorGUI.BeginDisabledGroup(!hasOuterBounds.boolValue);
			EditorGUILayout.LabelField("Outer bounds", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(omin);
			EditorGUILayout.PropertyField(omax);
			EditorGUI.EndDisabledGroup();

			var innerMin = imin.vector3Value;
			var innerMax = imax.vector3Value;
			var outerMin = omin.vector3Value;
			var outerMax = omax.vector3Value;

			if (innerMin.x > innerMax.x) innerMin.x = innerMax.x;
			if (innerMin.y > innerMax.y) innerMin.y = innerMax.y;
			if (innerMin.z > innerMax.z) innerMin.z = innerMax.z;

			if (innerMax.x < innerMin.x) innerMax.x = innerMin.x;
			if (innerMax.y < innerMin.y) innerMax.y = innerMin.y;
			if (innerMax.z < innerMin.z) innerMax.z = innerMin.z;

			if (hasOuterBounds.boolValue)
			{
				if (outerMin.x > innerMin.x) outerMin.x = innerMin.x;
				if (outerMin.y > innerMin.y) outerMin.y = innerMin.y;
				if (outerMin.z > innerMin.z) outerMin.z = innerMin.z;

				if (outerMax.x < innerMax.x) outerMax.x = innerMax.x;
				if (outerMax.y < innerMax.y) outerMax.y = innerMax.y;
				if (outerMax.z < innerMax.z) outerMax.z = innerMax.z;
			}
			else
			{
				outerMin = innerMin;
				outerMax = innerMax;
			}

			imin.vector3Value = innerMin;
			imax.vector3Value = innerMax;
			omin.vector3Value = outerMin;
			omax.vector3Value = outerMax;

			serializedObject.ApplyModifiedProperties();
		}

		private void OnSceneGUI()
		{
			var gob = (GameObjectBounds3D) target;

			var p = gob.gameObject.transform.position;
			var hc = Handles.color;

			var b = gob.InnerBounds;
			var sz = b.size;
			if (sz.x > 0 && sz.y > 0 && sz.z > 0)
			{
				DrawBounds(b, p, Color.red);
			}

			b = gob.OuterBounds;
			sz = b.size;
			if (gob.HasOuterBounds && sz.x > 0 && sz.y > 0 && sz.z > 0)
			{
				DrawBounds(b, p, Color.blue);
			}

			Handles.color = hc;
		}

		private void DrawBounds(Bounds b, Vector3 p, Color color)
		{
			var points = new[]
			{
				new Vector3(p.x + b.min.x, p.y + b.min.y, p.z + b.min.z),
				new Vector3(p.x + b.min.x, p.y + b.max.y, p.z + b.min.z),
				new Vector3(p.x + b.max.x, p.y + b.max.y, p.z + b.min.z),
				new Vector3(p.x + b.max.x, p.y + b.min.y, p.z + b.min.z)
			};
			Handles.color = color;
			Handles.DrawDottedLines(points, _indices, 5f);

			points = new[]
			{
				new Vector3(p.x + b.min.x, p.y + b.min.y, p.z + b.max.z),
				new Vector3(p.x + b.min.x, p.y + b.max.y, p.z + b.max.z),
				new Vector3(p.x + b.max.x, p.y + b.max.y, p.z + b.max.z),
				new Vector3(p.x + b.max.x, p.y + b.min.y, p.z + b.max.z)
			};
			Handles.DrawDottedLines(points, _indices, 5f);

			Handles.DrawDottedLine(
				new Vector3(p.x + b.min.x, p.y + b.min.y, p.z + b.min.z),
				new Vector3(p.x + b.min.x, p.y + b.min.y, p.z + b.max.z),
				5f);

			Handles.DrawDottedLine(
				new Vector3(p.x + b.min.x, p.y + b.max.y, p.z + b.min.z),
				new Vector3(p.x + b.min.x, p.y + b.max.y, p.z + b.max.z),
				5f);

			Handles.DrawDottedLine(
				new Vector3(p.x + b.max.x, p.y + b.max.y, p.z + b.min.z),
				new Vector3(p.x + b.max.x, p.y + b.max.y, p.z + b.max.z),
				5f);

			Handles.DrawDottedLine(
				new Vector3(p.x + b.max.x, p.y + b.min.y, p.z + b.min.z),
				new Vector3(p.x + b.max.x, p.y + b.min.y, p.z + b.max.z),
				5f);
		}

		private static void DrawSeparator(Color color, float size = 1.0f)
		{
			EditorGUILayout.Space();
			Texture2D tex = new Texture2D(1, 1);

			GUI.color = color;
			float y = GUILayoutUtility.GetLastRect().yMax;
			GUI.DrawTexture(new Rect(0.0f, y, Screen.width, size), tex);
			tex.hideFlags = HideFlags.DontSave;
			GUI.color = Color.white;

			EditorGUILayout.Space();
		}
	}
}