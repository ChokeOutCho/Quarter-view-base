using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    bool invenIsOpen = false;
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;

        Dictionary<int, Data.Stat> dict = Managers.Data.StatDict;

        gameObject.GetOrAddComponent<CursorController>();

        GameObject player = Managers.Game.Spawn(Define.WorldObject.Player, "YBot");
        Camera.main.gameObject.GetOrAddComponent<CameraController>().SetPlayer(player);

        Managers.Game.Spawn(Define.WorldObject.Monster, "Skeleton");


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ShowAndCloseUI();
        }
        
        
    }

    public override void Clear()
    {
        
    }

    void ShowAndCloseUI()
    {
        
            if (invenIsOpen == false)
            {
              Managers.UI.ShowPopupUI<UI_Inven>();
              invenIsOpen = true;
            Debug.Log("Inven is Open !");
            return;
             }
               
            if( invenIsOpen == true)
            {
              Managers.UI.ClosePopupUI();
              invenIsOpen = false;
            Debug.Log("Inven is Close !");
               return;

             }

    }
}
