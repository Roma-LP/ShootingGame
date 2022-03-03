using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEditor.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using UObject = UnityEngine.Object;

static class RepairEmptyComponent
{
    /// <summary>
    /// Menu interface for manually re-generating all ProBuilder geometry in scene.
    /// </summary>
    [MenuItem("Tools/ProBuilder/Repair/Rebuild Empty ProBuilderMesh Components", false)]
    public static void MenuForceSceneRefresh()
    {
        StringBuilder sb = new StringBuilder();
        ProBuilderMesh[] all = UObject.FindObjectsOfType<ProBuilderMesh>();

        for (int i = 0, l = all.Length; i < l; i++)
        {
            UnityEditor.EditorUtility.DisplayProgressBar(
                "Rebuild ProBuilder Objects",
                "Rebuilding ProBuilderMesh " + all[i].name + ".",
                ((float)i / all.Length));

            var mf = all[i].GetComponent<MeshFilter>();
            var sm = mf == null ? null : mf.sharedMesh;

            try
            {
                // If the ProBuilderMesh component data was lost somewhere, don't ToMesh() and nuke the existing (good)
                // mesh filter. Instead try rebuilding from the sharedmesh data.
                if (sm != null && sm.vertexCount > 0 && all[i].vertexCount < 1)
                {
                    var mesh = all[i];
                    var source = mesh.GetComponent<MeshFilter>().sharedMesh;
                    var materials = mesh.GetComponent<MeshRenderer>().sharedMaterials;
                    MeshImporter importer = new MeshImporter(mesh);
                    //importer.Import(source, materials, new MeshImportSettings() { quads = true, smoothing = true, smoothingAngle = 1f });
                    mesh.ToMesh();
                    mesh.Refresh();
                    mesh.Optimize();
                }
            }
            catch (Exception e)
            {
                sb.AppendLine("Failed rebuilding: " + all[i] + "\n\t" + e);
            }
        }

        if (sb.Length > 0)
            Debug.LogError(sb.ToString());

        UnityEditor.EditorUtility.ClearProgressBar();
        UnityEditor.EditorUtility.DisplayDialog("Refresh ProBuilder Objects",
            "Successfully refreshed all ProBuilder objects in scene.",
            "Okay");
    }
}
