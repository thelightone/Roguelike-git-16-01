using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.ParticleSystem;
using Random = UnityEngine.Random;

public class ChakrumBulletWeapon : MonoBehaviour
{
    private List<EnemyController> _enemys = new List<EnemyController>();
    private Transform _player;
    private WeaponParent _shootParent;
    public GameObject _shootEnemy;
    public EnemyController _enemyController;
    private Rigidbody _rb;

    private float distance;

    private bool repeated;

    private Collider _collider;

    [SerializeField] private ParticleSystem _miss;

    public void Start()
    {
        gameObject.SetActive(false);
        _rb = GetComponent<Rigidbody>();
        _player = PlayerMoveController.Instance.transform;
        _collider = GetComponent<Collider>();
        _shootParent = transform.parent.GetComponent<WeaponParent>();

        transform.position = PlayerMoveController.Instance.transform.position;
    }

    public void Shoot()
    {
        gameObject.SetActive(true);
        //gameObject.SetActive(false);
        _collider.enabled = false;
        StartCoroutine(PreShoot());

        _enemys = EnemySpawner.Instance.canBeShootList;
       // distance = 10;

        _rb.linearVelocity = Vector3.zero;
        gameObject.SetActive(true);
        StartCoroutine(Fly());
    }

    private IEnumerator PreShoot()
    {   gameObject.SetActive(false);
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
            Vector3 targPos = new Vector3(Random.Range(-360, 360), _shootEnemy.transform.position.y + 2, Random.Range(-360, 360));
            //var flySpeed = _shootParent.flySpeed * distance / 10;
            //flySpeed = flySpeed < 1f ? 1f : flySpeed;

            transform.LookAt(targPos);
        }

        else
        {
            transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        var getRepeatRate = 4;
        _collider.enabled= true;
        _rb.linearVelocity = Vector3.zero;

        _rb.AddForce(transform.forward* GetComponentInParent<ChakrumWeapon>().flySpeed*3, ForceMode.Impulse);

        while (elapsedTime < getRepeatRate)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
        _shootEnemy = null;
        repeated = false;
        yield return null;
    }

    public void OnTriggerEnter(Collider collision)
    {Debug.Log(collision.name);
        if (collision.TryGetComponent<EnemyController>(out EnemyController enemy))
        {
            Debug.Log("col inside");
            enemy.GetHit(_shootParent._hitForce * Modifiers.multiplHitForce * _shootParent.hitForceModif, _shootParent._upForce * Modifiers.multiplHitForceUp * _shootParent.weapHitForceUpModif, _shootParent._damage * Modifiers.multiplDamage * _shootParent.weapDamageModif);

            // gameObject.SetActive(false);
            //StopCoroutine(Fly());

        }

        //else if (collision.CompareTag("LevelCollider"))
        //{

        //    _rb.velocity = Vector3.zero;
        //    var y = transform.rotation.y;
        //    transform.LookAt(PlayerMoveController.Instance.transform);
        //    transform.rotation = Quaternion.Euler(transform.rotation.x, y, transform.rotation.z);
        //    _rb.AddForce(transform.forward * 3, ForceMode.Impulse);
        //}
    }
    void Update()
    {
        if (transform.localPosition.x < -9 || transform.localPosition.x > 9 || transform.localPosition.z < -14 || transform.localPosition.z > 12)
        {
            _rb.linearVelocity = Vector3.zero;

            Vector3 newtrs = new Vector3(PlayerMoveController.Instance.transform.position.x+Random.Range(-20,20),
                PlayerMoveController.Instance.transform.position.y,
                PlayerMoveController.Instance.transform.position.z + Random.Range(-20, 20));

            transform.LookAt(newtrs);
            _rb.AddForce(transform.forward * 3, ForceMode.Impulse);

        }
    }
}
