using UnityEditor;
using UnityEngine;

namespace Keru.Debug
{
    [CustomEditor(typeof(DrawGizmo))]
    public class DrawGizmoEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawGizmo gizmo = (DrawGizmo)target;

            gizmo.GizmoColor = EditorGUILayout.ColorField("Gizmo Color", gizmo.GizmoColor);
            gizmo.Type = (DrawGizmo.GizmoType)EditorGUILayout.EnumPopup("Gizmo Type", gizmo.Type);

            gizmo.LocalPosition = EditorGUILayout.Vector3Field("Local Position", gizmo.LocalPosition);

            switch (gizmo.Type)
            {
                case DrawGizmo.GizmoType.Sphere:
                    gizmo.Radius = EditorGUILayout.FloatField("Radius", gizmo.Radius);
                    break;
                case DrawGizmo.GizmoType.Cube:
                    gizmo.Size = EditorGUILayout.Vector3Field("Size", gizmo.Size);
                    break;
                case DrawGizmo.GizmoType.Line:
                    gizmo.Direction = EditorGUILayout.Vector3Field("Direction", gizmo.Direction);
                    break;
            }

            if (GUI.changed)
                EditorUtility.SetDirty(gizmo);
        }
    }

    public class DrawGizmo : MonoBehaviour
    {
        [SerializeField] public Color GizmoColor = Color.white;

        [SerializeField] public float Radius;
        [SerializeField] public Vector3 LocalPosition;
        [SerializeField] public Vector3 Direction;
        [SerializeField] public Vector3 Size;
        [SerializeField] public GizmoType Type;

        public enum GizmoType
        {
            Sphere,
            Cube,
            Line
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoColor;

            switch (Type)
            {
                case GizmoType.Sphere:
                    Gizmos.DrawSphere(transform.position + LocalPosition, Radius);
                    break;
                case GizmoType.Cube:
                    Gizmos.DrawCube(transform.position + LocalPosition, Size);
                    break;
                case GizmoType.Line:
                    Gizmos.DrawLine(transform.position + LocalPosition, transform.position + Direction);
                    break;
            }
        }
    }
}