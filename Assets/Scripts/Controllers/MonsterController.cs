using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterController : BaseController
{
    [SerializeField]
    float _scanRange = 15.0f;

    [SerializeField]
    float _attackRange = 2.0f;

    [SerializeField]
    float _activeCoolTime = 0.0f;

    [SerializeField]
    float _waitActive = 1.5f;
    protected enum Active
    {
        Off,
        Attack,

    }
    Active _active = Active.Off;

    Stat _stat;
    public override void Init()
    {
        WorldObjectType = Define.WorldObject.Monster;
        _stat = gameObject.GetComponent<Stat>();

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateIdle() // Ž��������
    {
        _activeCoolTime -= Time.deltaTime;

        GameObject player = Managers.Game.GetPlayer();
        if (player == null || _activeCoolTime > 0)
            return;

        float distance = (player.transform.position - transform.position).magnitude;
        if (distance <= _scanRange)
        {
            _lockTarget = player;
            State = Define.State.Moving;
            return;
        }

    }

    protected override void UpdateMoving()
    {
        if (_active != Active.Off || _activeCoolTime > 0)
        {
            _activeCoolTime -= Time.deltaTime;
            return;
        }


        if (_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= _attackRange)
            {
                NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
                nma.SetDestination(transform.position);
                _active = Active.Attack;
                State = Define.State.Active;

                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();
            nma.SetDestination(_destPos);
            nma.speed = _stat.MoveSpeed;
        }

    }
    protected override void UpdateActive()
    {
        if (_activeCoolTime > 0)
            return;

        if (_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);


        }

    }
    // TODO ��׷� Ž�� ���� �� ACTIVE!!
    //����Ž��(ȸ��) > 1�� �� > ��Ƽ�� > �������� > ����Ž��
    protected override void ActiveOn(Animator anim)
    {

        switch (_active)
        {
            case Active.Attack:
                {
                    anim.CrossFade("ATTACK", 0.1f);
                    _activeCoolTime = 2.0f;
                }
                break;
        }
    }

    void ActiveOver()
    {

        _active = Active.Off;
        State = Define.State.Idle;
    }

    void OnHit()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
            if (targetStat.Hp > 0)
            {
                float distance = (_lockTarget.transform.position - transform.position).magnitude;
                if (distance <= _attackRange)
                    State = Define.State.Active;
                //else
                //{
                //    _active = Active.Off;
                //    State = Define.State.Moving;
                //}

            }

        }
    }

    private void OnTriggerEnter(Collider hitbox)
    {
        if (hitbox.gameObject.tag != "HitBox")
            return;
        GameObject player = Managers.Game.GetPlayer();
        Stat playerStat = player.GetComponent<Stat>();
        Define.Skill hitBox = Define.Skill.None;
        if (hitbox.gameObject.name == "HitBoxAttack")
            hitBox = Define.Skill.Attack;
        if (hitbox.gameObject.name == "HitBoxSkillQ")
            hitBox = Define.Skill.SkillQ;

        _stat.OnHitBoxed(playerStat, hitBox);


    }

}
