using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenFlow : MonoBehaviour
{

    [Header("Waves")]
    [SerializeField] private float _waves;
    [SerializeField] private float _curWaveFinished;
    [SerializeField] private bool _loseFlow;

    [Header("Wave1-2")]
    [SerializeField] private GameObject _stars1;
    [SerializeField] private RectTransform _bar12;
    [SerializeField] private GameObject _swordBG2;
    [SerializeField] private GameObject _sword2;
    [SerializeField] private GameObject _lose1;

    [Header("Wave2-3")]
    [SerializeField] private GameObject _stars2;
    [SerializeField] private RectTransform _bar23;
    [SerializeField] private GameObject _swordBG3;
    [SerializeField] private GameObject _sword3;
    [SerializeField] private GameObject _lose2;

    [Header("Wave3-4")]
    [SerializeField] private GameObject _stars3;
    [SerializeField] private RectTransform _bar34;
    [SerializeField] private GameObject _swordBG4;
    [SerializeField] private GameObject _sword4;
    [SerializeField] private GameObject _lose3;

    [Header("Wave4-5")]
    [SerializeField] private GameObject _stars4;
    [SerializeField] private RectTransform _bar45;
    [SerializeField] private GameObject _swordBG5;
    [SerializeField] private GameObject _sword5;
    [SerializeField] private GameObject _lose4;

    [Header("Wave5-6")]
    [SerializeField] private GameObject _stars5;
    [SerializeField] private RectTransform _bar56;
    [SerializeField] private GameObject _swordBG6;
    [SerializeField] private GameObject _sword6;
    [SerializeField] private GameObject _lose5;

    [Header("Wave6-7")]
    [SerializeField] private GameObject _stars6;
    [SerializeField] private RectTransform _bar67;
    [SerializeField] private GameObject _swordBG7;
    [SerializeField] private GameObject _sword7;
    [SerializeField] private GameObject _lose6;

    [Header("Wave7")]
    [SerializeField] private GameObject _stars7;
    [SerializeField] private GameObject _lose7;


    public void ShowWinWave(float curWave)
    {
        gameObject.SetActive(true);
        _curWaveFinished = curWave;

        if (_waves == 1)
            Flow1();
        else
            OpenUnlocked();
    }


    private void Flow1()
    {
        if (!_loseFlow)
            _stars1.SetActive(true);
        else
            _lose1.SetActive(true);


    }

    private void OpenUnlocked()
    {
        if (_curWaveFinished == 1)
        {
            if (!_loseFlow)
                OpenNextLevel(_stars1, _bar12, _swordBG2, _sword2);
            else
                _lose1.SetActive(true);
        }

        if (_curWaveFinished > 1 && _waves > 1)
        {
            _stars1.SetActive(true);
            _bar12.gameObject.SetActive(false);
            _swordBG2.SetActive(false);
            _sword2.SetActive(false);

            if (_curWaveFinished == 2)
            {
                if (!_loseFlow)
                    OpenNextLevel(_stars2, _bar23, _swordBG3, _sword3);
                else
                    _lose2.SetActive(true);
            }
        }
        if (_curWaveFinished > 2)
        {
            _stars2.SetActive(true);
            _bar23.gameObject.SetActive(false);
            _swordBG3.SetActive(false);
            _sword3.SetActive(false);

            if (_curWaveFinished == 3)
            {
                if (!_loseFlow)
                    OpenNextLevel(_stars3, _bar34, _swordBG4, _sword4);
                else
                    _lose3.SetActive(true);
            }
        }
        if (_curWaveFinished > 3 && _waves > 3)
        {
            _stars3.SetActive(true);
            _bar34.gameObject.SetActive(false);
            _swordBG4.SetActive(false);
            _sword4.SetActive(false);

            if (_curWaveFinished == 4)
            {
                if (!_loseFlow)
                    OpenNextLevel(_stars4, _bar45, _swordBG5, _sword5);
                else
                    _lose4.SetActive(true);
            }
        }
        if (_curWaveFinished > 4)
        {
            _stars4.SetActive(true);
            _bar45.gameObject.SetActive(false);
            _swordBG5.SetActive(false);
            _sword5.SetActive(false);

            if (_curWaveFinished == 5)
            {
                if (!_loseFlow)
                    OpenNextLevel(_stars5, _bar56, _swordBG6, _sword6);
                else
                    _lose5.SetActive(true);
            }
        }
        if (_curWaveFinished > 5 && _waves > 5)
        {
            _stars5.SetActive(true);
            _bar56.gameObject.SetActive(false);
            _swordBG6.SetActive(false);
            _sword6.SetActive(false);

            if (_curWaveFinished == 6)
            {
                if (!_loseFlow)
                    OpenNextLevel(_stars6, _bar67, _swordBG7, _sword7);
                else
                    _lose6.SetActive(true);
            }
        }
        if (_curWaveFinished > 6)
        {
            _stars6.SetActive(true);
            _bar67.gameObject.SetActive(false);
            _swordBG7.SetActive(false);
            _sword7.SetActive(false);

            if (_curWaveFinished == 7)
            {
                if (!_loseFlow)
                    OpenNextLevel(_stars7, _bar67, _swordBG7, _sword7);
                else
                    _lose7.SetActive(true);
            }
        }
    }

    private void OpenNextLevel(GameObject stars, RectTransform bar, GameObject swordBG, GameObject sword)
    {
        if (_waves == _curWaveFinished)
        {
            stars.SetActive(true);
        }
        else
            StartCoroutine(Flow(stars, bar, swordBG, sword));
    }

    private IEnumerator Flow(GameObject stars, RectTransform bar, GameObject swordBG, GameObject sword)
    {


        stars.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        float elapsTime = 0;
        var startWidth = bar.sizeDelta.x;

        while (elapsTime < 1f)
        {
            var width = Mathf.Lerp(startWidth, 0, elapsTime / 1f);
            bar.sizeDelta = new Vector2(width, 30);
            elapsTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        swordBG.SetActive(false);
        sword.SetActive(true);
        AudioManager.Instance.OnStarAppear();

    }
}

