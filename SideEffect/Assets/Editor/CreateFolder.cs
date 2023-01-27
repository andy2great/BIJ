using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateFolder : EditorWindow
{
    [MenuItem("Setups/Create Default Folders")]
    private static void SetUpFolder()
    {
        CreateFolder Windows = ScriptableObject.CreateInstance<CreateFolder>();
        Windows.position = new Rect(Screen.width / 2, Screen.height / 2, 400, 150);
        Windows.ShowPopup();
   
    }
    private static void CreateAllFolder()
    {
        List<string> folders = new List<string>
        {
            "Art",
            "Code",
            "Docs",
            "Levels",
            "Developers"
        };
        foreach(string folder in folders)
        {
            if (!Directory.Exists("Assets/" + folder))
            {
                Directory.CreateDirectory("Assets/" + folder);
            }
        }

        List<string> foldersArt = new List<string>
        {
            "Animations",
            "Materials",
            "Models",
            "Musics",
            "Particles",
            "Prefabs",
            "Sounds",
            "Textures",
            "UI"
        };
        foreach (string folder in foldersArt)
        {
            if (!Directory.Exists("Assets/Art/" + folder))
            {
                Directory.CreateDirectory("Assets/Art/" + folder);
            }
        }
        List<string> foldersUI = new List<string>
        {
            "Fonts",
            "Icons",
        };
        foreach (string folder in foldersUI)
        {
            if (!Directory.Exists("Assets/Art/UI/" + folder))
            {
                Directory.CreateDirectory("Assets/Art/UI/" + folder);
            }
        }

        List<string> foldersCode = new List<string>
        {
            "Scripts",
            "Settings",
            "Shaders",
            "ThirdParty"
        };
        foreach (string folder in foldersCode)
        {
            if (!Directory.Exists("Assets/Code/" + folder))
            {
                Directory.CreateDirectory("Assets/Code/" + folder);
            }
        }


        AssetDatabase.Refresh();
    }
    void OnGUI()
    {
        EditorGUILayout.LabelField("Press Generate to make the folder structure for the projects");
        this.Repaint();
        GUILayout.Space(70);
        if (GUILayout.Button("Generate!"))
        {
            CreateAllFolder();
            this.Close();
        }
    }
}
