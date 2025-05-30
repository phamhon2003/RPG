using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RangeAttackBase : MonoBehaviour
{
    protected Transform target;
    [SerializeField] protected float _Damage;
    protected Coroutine attackCoroutine;
    protected float attackCooldown = 2f;
    protected float distance;
   
    protected abstract IEnumerator AttackEverySecond();
}
