using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private Vector3 _rotation;

    private int damage;
    private TMP_Text text;
    private Vector3 pos;
    private Vector3 initPos;
    private int goTop;

    private void Awake()
    {
        text = GetComponentInChildren<TMP_Text>();
        if (GetComponentInParent<BossController>() != null)
        {
            goTop = 10;
        }
        else
        {
            goTop = 6;
        }

    }

    private void OnEnable()
    {
        // Subscribe to the OnTick event when the script is enabled
        Ticker.OnTick += UpdateLogic;

    }

    private void OnDisable()
    {
        // Unsubscribe from the OnTick event when the script is disabled or destroyed
        Ticker.OnTick -= UpdateLogic;
        StopCoroutine("ShowCor");
    }

    public void UpdateLogic()
    {
        transform.rotation = Quaternion.Euler(_rotation);
    }

    public void Show(float dam, bool crit)
    {
        if (!gameObject.activeInHierarchy && gameObject.transform.parent.gameObject.activeInHierarchy)
        {
            text.color = crit? Color.yellow : Color.white;

            text.text = crit ? "Crit!"+"\n"+Convert.ToInt32(dam).ToString() : Convert.ToInt32(dam).ToString();

            gameObject.SetActive(true);
            StartCoroutine(ShowCor(dam));
        }
    }

    public IEnumerator ShowCor(float dam)
    {
        initPos = new Vector3(transform.parent.position.x, transform.parent.position.y + goTop, transform.parent.position.z);

        float elaps = 0;

        while (elaps<0.9f)
        {
            transform.position = initPos;
            elaps += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }

    public void Stop()
    {
        StopCoroutine("ShowCor");
        gameObject.SetActive(false);
    }
}

