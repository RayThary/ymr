using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;


[CustomEditor(typeof(TileManager), true)]
public class Editor_Tile : Editor
{
    TileManager manager;
    EditorWindow_Tile tile;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        manager = (TileManager)target;

        if (GUILayout.Button("Open Creative Window"))
        {
            tile = EditorWindow.GetWindow<EditorWindow_Tile>();
            tile.Init(manager);

            if (manager.tiles == null)
                manager.tiles = new GameObject[30, 30];

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    if (manager.tiles[i,j] == null)
                    {
                        GameObject obj = manager.CreateTile(tile.tiles[0]);
                        obj.transform.position = new Vector3(i, 0, j);
                        manager.tiles[i, j] = obj;
                    }
                    
                }
            }

            //윈도우를 열어주는 코드
            tile.ShowWindow();
        }

        if(GUILayout.Button("CreateTiles"))
        {
            manager.CreateTiles();
        }
    }

    private void OnSceneGUI()
    {
        if (manager != null && manager.edit != null)
        {
            Event e = Event.current;

            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit, float.MaxValue, manager.Layer))
            {
                //유저가 마우스를 클릭했을때 예비방 위에서 한거라면
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    manager.CreateTile((int)hit.transform.position.x, (int)hit.transform.position.z);
                    
                }
            }
        }
    }
}
