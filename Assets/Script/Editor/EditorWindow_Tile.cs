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
        //윈도우 열기
        Show();
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        //유저가 뭔가 고르면 select에 번호가 담김
        select = GUILayout.SelectionGrid(tiles.Length, tiles.Select(r => r.name).ToArray(), 3, GUILayout.Width(300), GUILayout.Height(250));

        if (EditorGUI.EndChangeCheck())
        {
            if (select >= 0)
            {
                tileManager.edit = tiles[select];
                //select는 0부터 시작이지만 지금 프리팹으로 저장된 방들의 이름은 1부터 시작이라 +1해줌
                //miro.Editor_CreateRooms(select + 1);
            }
        }
    }

    private void OnDestroy()
    {
        select = -1;
        tileManager.edit = null;
        tileManager.DeleteTiles();
        //만들어진 모드 예비방 삭제하는 코드
        //miro.Editor_DeleteCreativeRoom();
        //miro.Editor_DeletePreparatoryList();
    }
}
