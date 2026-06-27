using UnityEngine;
using UnityEditor;

public class CloudBuildPreExport
{
      // Called by Unity Cloud Build before the build starts
      public static void PreExport()
      {
                Debug.Log("[CloudBuildPreExport] Setting color space to Gamma for WebGL compatibility.");
                PlayerSettings.colorSpace = ColorSpace.Gamma;
                AssetDatabase.SaveAssets();
                Debug.Log("[CloudBuildPreExport] Color space set to: " + PlayerSettings.colorSpace);
      }
}
