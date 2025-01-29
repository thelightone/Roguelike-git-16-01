using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WeaponAura : WeaponParent
{
    private float _activeTime = 0;
    private Collider _mainCollider;
    private Collider _constCollider;
    public List<EnemyController> _listEnemies = new List<EnemyController>();

    public override void Awake()
    {
        base.Awake();
        _mainCollider = GetComponent<Collider>();
        _mainCollider.enabled = true;
        _constCollider = GetComponentInParent<Collider>();
    }

    public override void UpdateLogic()
    {

        elapsedTime += Time.deltaTime;

        if (_listEnemies.Count > 0 && elapsedTime>frequency * Modifiers.multiplAttackSpeed / weapAttackSpeedModif)
        {
            elapsedTime = 0;
            var copy = new List<EnemyController>();
            foreach (var en in _listEnemies)
            {
                if (en != null)
                {
                    if (en._curHealth - _damage < 0)
                    {
                        copy.Add(en);
                    }
                    en.GetHit(_hitForce * Modifiers.multiplHitForce * hitForceModif,
                    _upForce * Modifiers.multiplHitForceUp * weapHitForceUpModif,
                    _damage * Modifiers.multiplDamage * weapDamageModif);
                }
            }

            _listEnemies= _listEnemies.Except(copy).ToList();
        }
    }

    public override void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            _enemy = collision.gameObject.GetComponent<EnemyController>();

            if (_enemy != null)
            {
                if (slowEffect > 0)
                {
                    _enemy._moveSpeed /= slowEffect;
                }
                _listEnemies.Add(_enemy);
            }
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        return;
    }

    public void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy") )
        {
            _enemy = collision.gameObject.GetComponent<EnemyController>();
            if (_enemy != null)
            {
                if (slowEffect > 0)
                    _enemy._moveSpeed *= slowEffect;


                _listEnemies.Remove(_enemy);
            }
        }
    }

}