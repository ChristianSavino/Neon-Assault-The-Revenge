using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace Keru.Scripts.Engine.Editor
{
    public class JointEditorTool : EditorWindow
    {
        [MenuItem("Tools/Joint Modifier")]
        public static void ShowWindow()
        {
            GetWindow<JointEditorTool>("Joint Modifier");
        }

        private GameObject _selectedObject;
        private string _results;

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Seleccioná un GameObject en la jerarquía", EditorStyles.boldLabel);

            if (Selection.activeGameObject != _selectedObject)
            {
                _selectedObject = Selection.activeGameObject;
                EditorGUILayout.LabelField("Objeto seleccionado:", _selectedObject.name, EditorStyles.boldLabel);
            }

            if (_selectedObject == null)
            {
                EditorGUILayout.HelpBox("No hay ningún objeto seleccionado", MessageType.Info);
                return;
            }

            if (GUILayout.Button("Modificar Joints y Rigidbodies"))
            {
                ModifyJointsAndRigidbodies(_selectedObject);
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Resultados:", EditorStyles.boldLabel);
            if (!string.IsNullOrEmpty(_results))
            {
                EditorGUILayout.LabelField(_results, GUILayout.Height(100));
            }
            else
            {
                EditorGUILayout.LabelField("No se realizaron modificaciones.");
            }
        }

        private void ModifyJointsAndRigidbodies(GameObject root)
        {
            _results = "";
            var allTransforms = root.GetComponentsInChildren<Transform>(true);

            var rightShoulder = allTransforms.FirstOrDefault(t => t.name.Contains("Shoulder_R"));
            GetObjectAndModify(rightShoulder, 3, -10, 10, 20, 20);

            var leftShoulder = allTransforms.FirstOrDefault(t => t.name.Contains("Shoulder_L"));
            GetObjectAndModify(leftShoulder, 3, -10, 10, 20, 20);

            var leftElbow = allTransforms.FirstOrDefault(t => t.name.Contains("Elbow_L"));
            GetObjectAndModify(leftElbow, 3, -10, 10, 20, 20);

            var rightElbow = allTransforms.FirstOrDefault(t => t.name.Contains("Elbow_R"));
            GetObjectAndModify(rightElbow, 3, -10, 10, 20, 20);

            var spine = allTransforms.FirstOrDefault(t => t.name.Contains("Spine_02"));
            GetObjectAndModify(spine, 6, -10, 10, 15, 10);

            var hips = allTransforms.FirstOrDefault(t => t.name.Contains("Hips"));
            GetObjectAndModify(hips, 6, 0, 0,0, 0);

            var leftUpperLeg = allTransforms.FirstOrDefault(t => t.name.Contains("UpperLeg_L"));
            GetObjectAndModify(leftUpperLeg, 3, -10, 10, 20, 20);

            var rightUpperLeg = allTransforms.FirstOrDefault(t => t.name.Contains("UpperLeg_R"));
            GetObjectAndModify(rightUpperLeg, 3, -10, 10, 20, 20);

            var leftLowerLeg = allTransforms.FirstOrDefault(t => t.name.Contains("LowerLeg_L"));
            GetObjectAndModify(leftLowerLeg, 3, -10, 10, 20, 20);

            var rightLowerLeg = allTransforms.FirstOrDefault(t => t.name.Contains("LowerLeg_R"));
            GetObjectAndModify(rightLowerLeg, 3, -10, 10, 20, 20);
        }

        private void GetObjectAndModify(Transform transform, float mass, float lowTwistLimit, float highTwistLimit, float swing1Limit, float swing2Limit)
        {
            if(transform != null)
            {
                ModifyObjetctProperties(transform.gameObject, mass, lowTwistLimit, highTwistLimit, swing1Limit, swing2Limit);
            }
        }

        private void ModifyObjetctProperties(GameObject gameObject, float mass, float lowTwistLimit, float highTwistLimit, float swing1Limit, float swing2Limit)
        {
            var rigidbody = gameObject.GetComponent<Rigidbody>();
            var characterJoint = gameObject.GetComponent<CharacterJoint>();

            if(rigidbody != null)
            {
                rigidbody.mass = mass;
                rigidbody.automaticCenterOfMass = true;
                rigidbody.automaticInertiaTensor = true;
            }

            if(characterJoint != null)
            {
                characterJoint.lowTwistLimit = new SoftJointLimit { limit = lowTwistLimit, bounciness = 0, contactDistance = 0 };
                characterJoint.highTwistLimit = new SoftJointLimit { limit = highTwistLimit, bounciness = 0, contactDistance = 0 };
                characterJoint.swing1Limit = new SoftJointLimit {limit = swing1Limit, bounciness = 0, contactDistance = 0 };
                characterJoint.swing2Limit = new SoftJointLimit {limit = swing2Limit, bounciness = 0, contactDistance = 0 };
            }

            _results += $"{gameObject.name} modificado.\n";
        }
    }
}
