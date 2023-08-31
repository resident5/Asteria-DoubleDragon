using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(NewButton), true)]
    [CanEditMultipleObjects]
    /// <summary>
    ///   Custom Editor for the Button Component.
    ///   Extend this class to write a custom editor for a component derived from Button.
    /// </summary>
    public class NewButtonEditor : SelectableEditor
    {
        SerializedProperty m_OnClickProperty;
        SerializedProperty m_TextProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnClickProperty = serializedObject.FindProperty("m_OnClick");
            m_TextProperty = serializedObject.FindProperty("m_Text");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_TextProperty);
            EditorGUILayout.PropertyField(m_OnClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
