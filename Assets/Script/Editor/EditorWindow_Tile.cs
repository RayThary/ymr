using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

public class EditorWindow_Tile : EditorWindow
{
    TileManager tileManager;
    public GameObject[] tiles;

    public int select = -1;

    public void Init(TileManager tile)
    {
        tileManager = tile;
        tiles = Resources.LoadAll<GameObject>("Tiles");
    }

    public void ShowWindow()
    {
        //������ ����
        Show();
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        //������ ���� ���� select�� ��ȣ�� ���
        select = GUILayout.SelectionGrid(tiles.Length, tiles.Select(r => r.name).ToArray(), 3, GUILayout.Width(300), GUILayout.Height(250));

        if (EditorGUI.EndChangeCheck())
        {
            if (select >= 0)
            {
                tileManager.edit = tiles[select];
                //select�� 0���� ���������� ���� ���������� ����� ����� �̸��� 1���� �����̶� +1����
                //miro.Editor_CreateRooms(select + 1);
            }
        }
    }

    private void OnDestroy()
    {
        select = -1;
        tileManager.edit = null;
        tileManager.DeleteTiles();
        //������� ��� ����� �����ϴ� �ڵ�
        //miro.Editor_DeleteCreativeRoom();
        //miro.Editor_DeletePreparatoryList();
    }
}
