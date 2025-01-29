using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyData", menuName = "Enemy Data", order = 51)]
public class EnemySO : ScriptableObject
{
    [SerializeField] private int _enemyNumber;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _damage;
    [SerializeField] private Material _deathMat;
    [SerializeField] private Material _mainMat;

    //[SerializeField] private float _deathMat1;
    //[SerializeField] private float _deathMat2;

    public int enemyNumber { get { return _enemyNumber; } }
    public float maxSpeed { get { return _maxSpeed; } }
    public float maxHealth { get { return _maxHealth ; } }
    public float damage { get { return _damage; } }
    public Material deathMat { get { return _deathMat; } }
    public Material mainMat { get { return _mainMat; } }
}
