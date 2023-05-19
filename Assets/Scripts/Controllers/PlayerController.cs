using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseController
{
    protected  enum Active
    {
        Off,
        Dash,
        Attack,
        skill_Q,

    }

    int attackMotion = 0;
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    public float dashCoolTime = 0;



    PlayerStat _stat;

    RaycastHit hit;

    Active _active = Active.Off;

    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
        Managers.Input.KeyBoardAction -= OnKeyBoard;
        Managers.Input.KeyBoardAction += OnKeyBoard;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

  

    protected override void UpdateMoving()
    {
        if (_active != Active.Off)
            return;
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {//도착
            State = Define.State.Idle;
        }
        else
        { 
           // Debug.DrawRay(transform.position+Vector3.up*0.5f, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                State = Define.State.Idle;
                return;
            }
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);

        }
    }

    protected override void UpdateIdle()
    {
        if (_active != Active.Off)
        {
            _active = Active.Off;
            _state = Define.State.Idle;

        }
            
    }

    protected override void UpdateDie()
    {

    }
       
    protected override void UpdateActive()
    {
    }

    void ActiveStart()
    {
        

    }
    void ActiveOver()
    {
        _active = Active.Off;
        State = Define.State.Idle;
    }
    protected override void ActiveOn(Animator anim)
    {
        switch (_active)
        {
            case Active.Attack:
                {
                    LookMousePosition();
                    AttackMotion(anim);
                }
                break;
            case Active.skill_Q:
                {
                    LookMousePosition();
                    anim.CrossFade("SKILL_Q", 0.2f);
                }
                break;
            case Active.Dash:
                {
                    if (dashCoolTime == 0)
                    anim.CrossFade("DASH", 0.2f);
                    LookMousePosition();
                    //Dash(anim);
                }
                break;
        }
    }

    void AttackMotion(Animator anim)
    {
        attackMotion = 0;
        if (attackMotion == 0) 
            anim.CrossFade("ATTACK", 0.2f, -1, 0); 
        else if (attackMotion == 1)
        anim.CrossFade("ATTACK2", 0.2f, -1, 0);         
        else if (attackMotion == 2)
         anim.CrossFade("ATTACK3", 0.2f, -1, 0);

        Debug.Log($"모션{attackMotion}");
        attackMotion++;
        if (attackMotion > 2)
            attackMotion = 0;
    }

    void Dash(Animator anim)
    {
        dashCoolTime = 0.0f;
        LookMousePosition();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hit, LayerMask.GetMask("Ground")))
        {
            Vector3 dir = hit.point - transform.position;
            dir = new Vector3(dir.x, 0.0f, dir.z);
            transform.position += dir.normalized * 10.0f;

        }
           
    }
    

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == Define.State.Die)
            return;


        //이동 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);
      //  Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        switch (evt)
        {

            case Define.MouseEvent.LeftPointerDown:
            case Define.MouseEvent.LeftPress:
                {
                    if (_active == Active.Off)
                    {
                        _active = Active.Attack;
                        State = Define.State.Active;
                        
                    }
                }
                break;

            case Define.MouseEvent.RightPointerDown:
                GameObject ClickMoveEffect = Managers.Resource.Instantiate($"CME");
                ClickMoveEffect.transform.position = new Vector3(hit.point.x, 0.0f, hit.point.z);
                break;
            case Define.MouseEvent.RightPress:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        if(_active==Active.Off)
                           State = Define.State.Moving;
                        
                        // Debug.Log($"Raycast Camera @ {hit.collider.gameObject.tag}");

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            _lockTarget = hit.collider.gameObject;
                            Debug.Log("Monster Click");
                        }
                        else
                        {
                            _lockTarget = null;
                            Debug.Log("Ground Click");
                        }
                    }
                }
                break;
       
            break;
           case Define.MouseEvent.RightPointerUp:
            _lockTarget = null;
           break;
        }      
    }

    void OnKeyBoard(Define.KeyBoardEvent evt)
    {
        
        if (State == Define.State.Die)
            return;

        if (_active != Active.Off)
            return;
        switch (evt)
        {
            case Define.KeyBoardEvent.Q_Key:
            case Define.KeyBoardEvent.Q_KeyDown:
                {
                    _active = Active.skill_Q;
                    State = Define.State.Active;


                }
            break;

            case Define.KeyBoardEvent.Space:
            case Define.KeyBoardEvent.SpaceDown:
                {
                    
                    if (dashCoolTime == 0)
                    {
                        _active = Active.Dash;
                        State = Define.State.Active;
                    }
                 
                }
                break;
        }

    }
    void LookMousePosition()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
            Vector3 dir = hit.point - transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3 (dir.x,0.0f,dir.z));
        
    }

    void Attack()
    {

        GameObject hitBox = Managers.Resource.Instantiate("HitBox/HitBoxAttack");
        hitBox.transform.position = transform.position+transform.forward + new Vector3(0.0f,1.0f,0.0f);
        hitBox.transform.rotation = transform.rotation;
    }
    void SkillQ()
    {
        GameObject hitBox = Managers.Resource.Instantiate("HitBox/HitBoxSkillQ");
        hitBox.transform.position = transform.position + transform.forward + new Vector3(0.0f, 1.0f, 0.0f);
        hitBox.transform.rotation = transform.rotation;
    }

}

