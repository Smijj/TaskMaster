using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Explodable))]
public class ExplodableEditor : Editor {

    SerializedProperty m_FragmentsParent;
    SerializedProperty fragmentLayer;
    SerializedProperty sortingLayerName;
    SerializedProperty orderInLayer;
    SerializedProperty m_DrawGizmos;

    private void OnEnable() {
        m_FragmentsParent = serializedObject.FindProperty("m_FragmentsParent");
        fragmentLayer = serializedObject.FindProperty("fragmentLayer");
        sortingLayerName = serializedObject.FindProperty("sortingLayerName");
        orderInLayer = serializedObject.FindProperty("orderInLayer");
        m_DrawGizmos = serializedObject.FindProperty("m_DrawGizmos");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Explodable myTarget = (Explodable)target;
        //myTarget.allowRuntimeFragmentation = EditorGUILayout.Toggle("Allow Runtime Fragmentation", myTarget.allowRuntimeFragmentation);
        EditorGUILayout.PropertyField(m_DrawGizmos);
        myTarget.shatterType = (Explodable.ShatterType)EditorGUILayout.EnumPopup("Shatter Type", myTarget.shatterType);
        myTarget.extraPoints = EditorGUILayout.IntField("Extra Points", myTarget.extraPoints);
        myTarget.subshatterSteps = EditorGUILayout.IntField("Subshatter Steps",myTarget.subshatterSteps);
        if (myTarget.subshatterSteps > 1)
        {
            EditorGUILayout.HelpBox("Use subshatter steps with caution! Too many will break performance!!! Don't recommend more than 1", MessageType.Warning);
        }

        //myTarget.fragmentLayer = EditorGUILayout.TextField("Fragment Layer", myTarget.fragmentLayer);
        //myTarget.sortingLayerName = EditorGUILayout.TextField("Sorting Layer", myTarget.sortingLayerName);
        EditorGUILayout.PropertyField(fragmentLayer);
        EditorGUILayout.PropertyField(sortingLayerName);
        EditorGUILayout.PropertyField(orderInLayer);
        EditorGUILayout.PropertyField(m_FragmentsParent);

        if (myTarget.GetComponent<PolygonCollider2D>() == null && myTarget.GetComponent<BoxCollider2D>() == null)
        {
            EditorGUILayout.HelpBox("You must add a BoxCollider2D or PolygonCollider2D to explode this sprite", MessageType.Warning);
        }
        else
        {
            if (GUILayout.Button("Generate Fragments"))
            {
                myTarget.fragmentInEditor();
                EditorUtility.SetDirty(myTarget);
            }
            if (GUILayout.Button("Destroy Fragments"))
            {
                myTarget.deleteFragments();
                EditorUtility.SetDirty(myTarget);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
