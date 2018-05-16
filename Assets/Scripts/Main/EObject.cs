using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ColaFrame的Object基类
/// </summary>
public class EObject
{
    public int id = 0;
    public string server_id;
    public Vector3 initPos = Vector3.zero;
    public Vector3 curPos = Vector3.zero;
    public Quaternion initDir = Quaternion.identity;
    public float modelScale = 1f;
    public EModel model;

    public EObject()
    {

    }

    public void InitBaseInfo()
    {

    }

    public virtual void OnModelLoaded()
    {

    }

    public virtual void Release()
    {

    }

    public virtual void AnimationSkill()
    {

    }

    public virtual void AnimationSkillPause()
    {

    }

    public virtual void AnimationAttack()
    {

    }

    public virtual void AnimationRun()
    {

    }

    public virtual void AnimationStand()
    {

    }

    public virtual void AnimationDie()
    {

    }

    public virtual void OnClick()
    {

    }

    public virtual void OnMove()
    {

    }


}
