using UnityEngine;
using UnityEngine.UI;
using UnityEditorInternal;
using UnityEditor.AnimatedValues;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(GridLayoutGroup), true)]
    [CanEditMultipleObjects]
    /// <summary>
    /// Custom Editor for the GridLayout Component.
    /// Extend this class to write a custom editor for a component derived from GridLayout.
    /// </summary>
    public class GridLayoutGroupEditor : Editor
    {
        SerializedProperty m_Padding;
        SerializedProperty m_CellSize;
        SerializedProperty m_Spacing;
        SerializedProperty m_StartCorner;
        SerializedProperty m_StartAxis;
        SerializedProperty m_ChildAlignment;
        SerializedProperty m_Constraint;
        SerializedProperty m_ConstraintCount;

        protected virtual void OnEnable()
        {
            m_Padding = serializedObject.FindProperty("m_Padding");
            m_CellSize = serializedObject.FindProperty("m_CellSize");
            m_Spacing = serializedObject.FindProperty("m_Spacing");
            m_StartCorner = serializedObject.FindProperty("m_StartCorner");
            m_StartAxis = serializedObject.FindProperty("m_StartAxis");
            m_ChildAlignment = serializedObject.FindProperty("m_ChildAlignment");
            m_Constraint = serializedObject.FindProperty("m_Constraint");
            m_ConstraintCount = serializedObject.FindProperty("m_ConstraintCount");
        }

        public override void OnInspectorGUI()
        {
            GridLayoutGroup controller = (GridLayoutGroup)target;
            ResponsiveGrid rg = controller.GetComponent<ResponsiveGrid>();
            bool isResponsive = rg != null;

            serializedObject.Update();
            EditorGUI.BeginDisabledGroup(rg.usePercentages);
            EditorGUILayout.PropertyField(m_Padding, true);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(isResponsive);
            EditorGUILayout.PropertyField(m_CellSize, true);
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(rg.usePercentages);
            EditorGUILayout.PropertyField(m_Spacing, true);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(m_StartCorner, true);
            EditorGUILayout.PropertyField(m_StartAxis, true);
            EditorGUILayout.PropertyField(m_ChildAlignment, true);
            EditorGUILayout.PropertyField(m_Constraint, true);

            EditorGUI.BeginDisabledGroup(isResponsive);
            if (m_Constraint.enumValueIndex > 0)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_ConstraintCount, true);
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndDisabledGroup();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
