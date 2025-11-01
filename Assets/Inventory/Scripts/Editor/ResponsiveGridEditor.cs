using UnityEngine;
using UnityEngine.UI;
using UnityEditorInternal;
using UnityEditor.AnimatedValues;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ResponsiveGrid), true)]
    [CanEditMultipleObjects]
    /// <summary>
    /// Custom Editor for the GridLayout Component.
    /// Extend this class to write a custom editor for a component derived from GridLayout.
    /// </summary>
    public class ResponsiveGridEditor : Editor
    {
        SerializedProperty m_columns;
        SerializedProperty m_rows;
        SerializedProperty m_usePercentages;
        SerializedProperty m_paddingPercent;
        SerializedProperty m_spacingPercent;

        protected virtual void OnEnable()
        {
            m_columns = serializedObject.FindProperty("columns");
            m_rows = serializedObject.FindProperty("rows");
            m_usePercentages = serializedObject.FindProperty("usePercentages");
            m_paddingPercent = serializedObject.FindProperty("paddingPercent");
            m_spacingPercent = serializedObject.FindProperty("spacingPercent");
        }

        public override void OnInspectorGUI()
        {
            ResponsiveGrid controller = (ResponsiveGrid)target;
            GridLayoutGroup gridLayout = controller.GetComponent<GridLayoutGroup>();

            serializedObject.Update();

            if(gridLayout.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
                EditorGUILayout.PropertyField(m_columns, true);
            else if(gridLayout.constraint == GridLayoutGroup.Constraint.FixedRowCount)
                EditorGUILayout.PropertyField(m_rows, true);

            EditorGUILayout.PropertyField(m_usePercentages, true);

            if (controller.usePercentages)
            {
                EditorGUILayout.PropertyField(m_paddingPercent, true);
                EditorGUILayout.PropertyField(m_spacingPercent, true);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
