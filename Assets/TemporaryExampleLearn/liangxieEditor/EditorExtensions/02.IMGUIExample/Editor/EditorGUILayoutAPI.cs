using UnityEditor;

namespace EditorExtensions
{
    public class EditorGUILayoutAPI
    {
        BuildTargetGroup _buildTargetGroup;

        public static void Draw()
        {
            var selectedBuildTargetGroup = EditorGUILayout.BeginBuildTargetSelectionGrouping();
            switch (selectedBuildTargetGroup)
            {
                case BuildTargetGroup.Android:
                    EditorGUILayout.LabelField("Android specific things");
                    break;
                case BuildTargetGroup.Standalone:
                    EditorGUILayout.LabelField("Standalone specific things");
                    break;
            }

            EditorGUILayout.EndBuildTargetSelectionGrouping();
        }
    }
}