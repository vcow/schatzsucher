using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    /// <inheritdoc />
    [CustomEditor(typeof(BoundsSettings), true)]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class BoundsInspector : UnityEditor.Editor
    {
        private readonly int[] _indices = {0, 1, 1, 2, 2, 3, 3, 0};
        
        public override void OnInspectorGUI()
        {
            var ib = serializedObject.FindProperty("_innerBounds");
            EditorGUILayout.PropertyField(ib);
            DrawSeparator(Color.gray);
            var hasOuterBounds = serializedObject.FindProperty("_hasOuterBounds");
            EditorGUILayout.PropertyField(hasOuterBounds);
            var ob = serializedObject.FindProperty("_outerBounds");
            EditorGUI.BeginDisabledGroup(!hasOuterBounds.boolValue);
            EditorGUILayout.PropertyField(ob);
            EditorGUI.EndDisabledGroup();

            var innerRect = ib.rectValue;
            var outerRect = ob.rectValue;

            if (hasOuterBounds.boolValue)
            {
                if (innerRect.xMin < outerRect.xMin)
                {
                    outerRect.xMin = innerRect.xMin;
                    ob.rectValue = outerRect;
                }
            
                if (innerRect.yMin < outerRect.yMin)
                {
                    outerRect.yMin = innerRect.yMin;
                    ob.rectValue = outerRect;
                }
            
                if (innerRect.xMax > outerRect.xMax)
                {
                    outerRect.xMax = innerRect.xMax;
                    ob.rectValue = outerRect;
                }
            
                if (innerRect.yMax > outerRect.yMax)
                {
                    outerRect.yMax = innerRect.yMax;
                    ob.rectValue = outerRect;
                }
            }
            else if (outerRect != innerRect)
            {
                ob.rectValue = innerRect;
            }
            
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnSceneGUI()
        {
            var settings = (BoundsSettings) target;

            var p = settings.gameObject.transform.position;
            const int depth = 0;
            var hc = Handles.color;

            var b = settings.InnerBounds;
            if (b.width > 0 && b.height > 0)
            {
                var points = new []
                {
                    new Vector3(p.x + b.xMin, p.y + b.yMin, p.z + depth),
                    new Vector3(p.x + b.xMin, p.y + b.yMax, p.z + depth),
                    new Vector3(p.x + b.xMax, p.y + b.yMax, p.z + depth),
                    new Vector3(p.x + b.xMax, p.y + b.yMin, p.z + depth)
                };
                Handles.color = Color.red;
                Handles.DrawDottedLines(points, _indices, 5f);
            }
            
            b = settings.OuterBounds;
            if (settings.HasOuterBounds && b.width > 0 && b.height > 0)
            {
                var points = new []
                {
                    new Vector3(p.x + b.xMin, p.y + b.yMin, p.z + depth),
                    new Vector3(p.x + b.xMin, p.y + b.yMax, p.z + depth),
                    new Vector3(p.x + b.xMax, p.y + b.yMax, p.z + depth),
                    new Vector3(p.x + b.xMax, p.y + b.yMin, p.z + depth)
                };
                Handles.color = Color.blue;
                Handles.DrawDottedLines(points, _indices, 5f);
            }

            Handles.color = hc;
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