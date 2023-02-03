using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoadGenerator))]
public class InspectorTool : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoadGenerator con = (RoadGenerator)target;

        if (GUILayout.Button("Spawn"))
        {

            con.GeneratePlatforms();
        }
        if (GUILayout.Button("Reset"))
        {
            con.ResetPlatforms();
        }
    }
}

[CustomEditor(typeof(AddComp))]
public class InspectorTool2 : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AddComp con = (AddComp)target;

        if (GUILayout.Button("Add Occlude"))
        {

            // con.Add();
            var allRend = con.gameObject.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in allRend)
            {
                r.gameObject.AddComponent<BoxCollider>().isTrigger = true;
                r.gameObject.AddComponent<OcclusionObject>();
            }

            Object.DestroyImmediate(con.gameObject.GetComponent<AddComp>());
        }
    }
}
