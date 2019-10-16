using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonAnimationTester))]
public class ButtonAnimationTesterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ButtonAnimationTester tester = target as ButtonAnimationTester;
        if (GUILayout.Button("Insert Buttons"))
        {
            tester.InsertButtons();
        }

        if (GUILayout.Button("Insert Mixed Buttons"))
        {
            tester.InsertMixedButtons();
        }

        if (GUILayout.Button("Insert 3 Buttons"))
        {
            tester.Insert3Buttons();
        }

        if (GUILayout.Button("InsertOnlyOneValueButtons"))
        {
            tester.InsertOnlyOneValueButtons();
        }

        if (GUILayout.Button("InsertSameValueButtons"))
        {
            tester.InsertSameValueButtons();
        }

        if (GUILayout.Button("Insert 5 Buttons"))
        {
            tester.Insert5Buttons();
        }

        if (GUILayout.Button("Insert 12 Buttons"))
        {
            tester.Insert12Buttons();
        }

        if (GUILayout.Button("Insert Higher 5 Buttons"))
        {
            tester.InsertHigher5Buttons();
        }
    }
}
