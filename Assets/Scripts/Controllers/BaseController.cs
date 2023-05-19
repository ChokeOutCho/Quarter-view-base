using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos;

    [SerializeField]
    protected GameObject _lockTarget;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    public Define.WorldObject WorldObjectType { get; protected set; } = Define.WorldObject.Unknown;

    private void Start()
    {
            Init();
    }


    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (State)
            {
                case Define.State.Die:

                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.2f);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.2f);
                    break;
                case Define.State.Active:
                    {
                        ActiveOn(anim);
                    }
                    break;
            }
        }
    }

    void Update()
    {
        switch (State)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Active:
                UpdateActive();
                break;

        }
 }

    public abstract void Init();
    protected virtual void UpdateDie() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateActive() { }

    protected virtual void ActiveOn(Animator anim) { }
}

