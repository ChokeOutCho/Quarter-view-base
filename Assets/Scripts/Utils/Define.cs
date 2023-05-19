using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum WorldObject
    {
        Unknown,
        Player,
        Monster,
    }

public enum State
    {
        Die,
        Moving,
        Idle,
        Active, // 공격상태들 통합해도 좋을듯함
    }

public enum Layer
    {
        Monster = 10,
        Ground = 11,
        Block = 12,
    }
    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game,
        Town,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum MouseEvent
    {

        LeftPress,
        LeftPointerDown,
        LeftPointerUp,
        LeftClick,

        RightPress,
        RightPointerDown,
        RightPointerUp,
        RightClick,
    }

    public enum KeyBoardEvent
    {
        Space,
        SpaceHold,
        SpaceDown,
        SpaceUp,


        Q_Key, //입력
        Q_KeyHold, //홀드
        Q_KeyDown,
        Q_KeyUp,
        

        W_Key,
        W_keyHold,
        W_KeyDown,
        W_KeyUp,


    }

    public enum CameraMode
    {
        QuarterView,
        FirstPersonView,
    }

    public enum Skill
    {
        None,
        Attack,
        SkillQ,
        SkillW,

    }


}
