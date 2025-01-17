using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillArea : MonoBehaviour
{
    public float _damage = 30;
    public Collider _collider;
    public PlayerMoveController _player;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _player = PlayerMoveController.Instance;
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player.GetHit(collision.transform.position, transform.position, 10, _damage);
        }
    }
}
