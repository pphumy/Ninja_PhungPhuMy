using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] protected HealthBar healthbar;
    [SerializeField] protected CombatText CombatTextPrefab;
    private string currentAnimName;
    float hp;
    
    public bool IsDead => hp <= 0;
    
    



    void Start()
    {
        OnInit();
        healthbar.OnInit(100, transform);
    }

    // Update is called once per frame
    void Update()
    {


    }

    public virtual void OnInit()
    {
        hp = 100;
    }

    public virtual void OnDespawn()
    {

    }

    protected virtual void OnDeath()
    {
        ChangeAnim("die");
        Invoke(nameof(OnDespawn), 2f);
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    public void OnHit(float damage)
    {
        if(!IsDead)
        {
            Debug.Log("hit");
            hp -= damage;
            if(IsDead)
            {
                hp = 0;
                OnDeath();
            }
            healthbar.SetNewHp(hp);
            Instantiate(CombatTextPrefab, transform.position + Vector3.up, Quaternion.identity).OnInit(damage);
        }
    }

    
}
