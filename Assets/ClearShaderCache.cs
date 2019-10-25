using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ClearShaderCache : MonoBehaviour
{
    [MenuItem("Tools/Clear shader cache")]
    static public void ClearShaderCache_Command()
    {
        var shaderCachePath = Path.Combine(Application.dataPath, "../Library/ShaderCache");
        Directory.Delete(shaderCachePath, true);
    }
}
