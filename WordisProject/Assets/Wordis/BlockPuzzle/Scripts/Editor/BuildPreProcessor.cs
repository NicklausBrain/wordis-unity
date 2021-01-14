using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildPreProcessor : IPreprocessBuildWithReport
{
    public int callbackOrder
    {
        get { return 0; }
    }
    
    public void OnPreprocessBuild(BuildReport report)
    {
        int currentBuildNumber = Int32.Parse(PlayerSettings.iOS.buildNumber);
        int incrementedBuildNumber = currentBuildNumber + 1;
        PlayerSettings.iOS.buildNumber = incrementedBuildNumber.ToString();
        
        Debug.Log(string.Format("Build number was incremented from {0} to {1}", currentBuildNumber,
            incrementedBuildNumber));
    }
}