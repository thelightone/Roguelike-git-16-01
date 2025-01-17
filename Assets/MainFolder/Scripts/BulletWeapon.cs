using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class BulletWeapon : MonoBehaviour
{
    private List<EnemyController> _enemys = new List<EnemyController>();
    private Transform _player;
    private AimShootWeapon _shootParent;
    public GameObject _shootEnemy;
    public EnemyController _enemyController;
    private Rigidbody _rb;

    private float distance;

    private bool repeated;

    private Collider _collider;

    [SerializeField] private ParticleSystem _miss;

    public void Start()
    {
        _shootParent = GetComponentInParent<AimShootWeapon>();
        _shootParent._bullets.Add(this);
        gameObject.SetActive(false);
        _rb = GetComponent<Rigidbody>();
        _player = PlayerMoveController.Instance.transform;
        _collider = GetComponent<Collider>();
    }

    public void Shoot()
    {
        gameObject.SetActive(true);
        _collider.enabled = false;
        StartCoroutine(PreShoot());
        _enemys = EnemySpawner.Instance.canBeShootList;
        distance = 100000;


        foreach (EnemyController enemy in _enemys)
        {
            if (enemy != null)
            {
                if (!repeated)
                {
                    var distance2 = Vector3.Distance(PlayerMoveController.Instance.transform.position, enemy.gameObject.transform.position);
                    if (distance2 < distance && !_shootParent._busyEnemies.Contains(enemy))
                    {
                        _enemyController = enemy;
                        _shootEnemy = enemy.gameObject;
                        distance = distance2;
                    }
                }
                else
                {
                    var distance2 = Vector3.Distance(transform.position, enemy.gameObject.transform.position);
                    if (distance2 < distance && !_shootParent._busyEnemies.Contains(enemy))
                    {
                        _enemyController = enemy;
                        _shootEnemy = enemy.gameObject;
                        distance = distance2;
                    }
                }
            }
        }

        if (_enemys.Count > 1)
        {
            _shootParent._busyEnemies.Add(_enemyController);
        }
        // _shootEnemy = _enemys[Random.Range(0, _enemys.Count - 1)].gameObject;
        _rb.velocity = Vector3.zero;
        StartCoroutine(Fly());
    }

    private IEnumerator PreShoot()
    {
        float elapsedTime = 0;
        while (elapsedTime < 0.1f)
        {
            gameObject.transform.position = _player.position + _player.forward * 2;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator Fly()
    {


        float elapsedTime = 0;

        var basePos = gameObject.transform.position;

        if (_shootEnemy != null)
        {
            Vector3 targPos = new Vector3(_shootEnemy.transform.position.x, _shootEnemy.transform.position.y + 2, _shootEnemy.transform.position.z);
            //var flySpeed = _shootParent.flySpeed * distance / 10;
            //flySpeed = flySpeed < 1f ? 1f : flySpeed;

            transform.LookAt(targPos);
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        var getRepeatRate = _shootParent.reloadTime;
        _collider.enabled= true;
        _rb.AddForce(transform.forward / getRepeatRate *5, ForceMode.Impulse);

        while (elapsedTime < getRepeatRate * 0.9)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //while (elapsedTime < flySpeed)
        //{
        //    if (_shootEnemy != null)
        //    {
        //        targPos = new Vector3(_shootEnemy.transform.position.x, _shootEnemy.transform.position.y + 1, _shootEnemy.transform.position.z);
        //        gameObject.transform.position = Vector3.Lerp(basePos, targPos, (elapsedTime * 5f) / (flySpeed));
        //        elapsedTime += Time.deltaTime;
        //        if (!_shootEnemy.activeInHierarchy)
        //        {
        //            _miss.transform.position = gameObject.transform.position;
        //            _miss.Play();
        //            gameObject.SetActive(false);
        //            StopCoroutine(Fly());
        //            break;
        //        }
        //    }
        //    else
        //    {
        //        break;
        //    }
        //    yield return null;
        //}

        gameObject.SetActive(false);
        _miss.transform.position = gameObject.transform.position;
        _miss.Play();
        _shootEnemy = null;
        repeated = false;
        yield return null;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            enemy.GetHit(_shootParent._hitForce * Modifiers.multiplHitForce * _shootParent.hitForceModif, _shootParent._upForce * Modifiers.multiplHitForceUp * _shootParent.weapHitForceUpModif, _shootParent._damage * Modifiers.multiplDamage * _shootParent.weapDamageModif);



            _miss.transform.position = gameObject.transform.position;
            _miss.Play();
            // gameObject.SetActive(false);
            //StopCoroutine(Fly());

        }
    }

}
