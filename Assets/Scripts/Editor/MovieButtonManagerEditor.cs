using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(MovieButtonManager))]
//public class MovieButtonManagerEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        base.OnInspectorGUI();

//        MovieButtonManager manager = target as MovieButtonManager;
//        if (GUILayout.Button("Generate Buttons"))
//        {
//            manager.GenerateButtons();
//        }

//    }
//}

[CustomEditor(typeof(MovieMainManager))]
public class MovieMainManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MovieMainManager manager = target as MovieMainManager;
        if (GUILayout.Button("Generate Buttons"))
        {
            manager.GenerateButtons();
        }

    }
}

[CustomEditor(typeof(MovieDetailManager))]
public class MovieDetailManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MovieDetailManager manager = target as MovieDetailManager;
        if (GUILayout.Button("Generate Buttons"))
        {
            manager.GenerateButtons();
        }

    }
}
