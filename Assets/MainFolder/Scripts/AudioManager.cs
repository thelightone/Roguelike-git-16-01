using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] public AudioSource audioSource1;
    [SerializeField] public AudioSource audioSource2;

    [SerializeField] private AudioClip _startBattle;
    [SerializeField] private AudioClip _bossFight;
    [SerializeField] private AudioClip _lose;
    [SerializeField] private AudioClip _win;
    [SerializeField] private AudioClip _menu;
    [SerializeField] private AudioClip _takeXP;
    [SerializeField] private AudioClip _takeSpecial;
    [SerializeField] private AudioClip _levelUp;
    [SerializeField] private AudioClip _skill;
    public  AudioClip _baseAttack;

    [SerializeField] private AudioClip _buttonTap;
    [SerializeField] private List<AudioClip> _steps;
    [SerializeField] private List<AudioClip> _getHit;
    [SerializeField] private List<AudioClip> _enemyGetHit;

    [SerializeField] private List <Button> _buttons;
    [SerializeField] private AudioClip _starAppear;

    [SerializeField] private List<Button> _soundOffButs;
    public bool _soundOff;


    public Transform _musicCheck;

    public static AudioManager Instance;

    private void Start()
    {
        //if (Instance != null)
        //    Destroy(Instance.gameObject);

        Instance = this;

       // DontDestroyOnLoad(gameObject);

        foreach (var but in _buttons)
        {
            but?.onClick.AddListener(() => OnButtonTap());
        }

        //foreach (var but in _soundOffButs)
        //{
        //    but.onClick.AddListener(() => SwitchSound());
        //}

        switch (SceneManager.GetActiveScene().name)
        {
            case "BattleScene":
                StartBattle();
                break;
            case "BattleSceneIce":
                StartBattle();
                break;
            case "MenuScene":
                OnMenu();
                break;
        }

        audioSource2.volume =  PrefsManager.GetSound();
        audioSource1.volume = PrefsManager.GetSound();
        _soundOff = PrefsManager.GetSound() == 1? false : true;


        if (PrefsManager.GetSound() == 1)
        {
            _musicCheck.GetChild(1).gameObject.SetActive(true);
            _musicCheck.GetChild(0).gameObject.SetActive(false);

        }
        else
        {
            _musicCheck.GetChild(0).gameObject.SetActive(true);
            _musicCheck.GetChild(1).gameObject.SetActive(false);

        }
    }

    public void OnMenu()
    {
        audioSource1.clip = _menu;
        audioSource1.Play();
    }

    public void StartBattle()
    {
        //SwitchAudio(_startBattle);
        audioSource1.clip = _startBattle;
        audioSource1.Play();
    }

    public void BossFight()
    {
  SwitchAudio(_bossFight);
 
    }

    public void OnLose() {
SwitchAudio(_lose);
    }

    public void OnWin()
    {
        audioSource2.PlayOneShot(_win);
    }

    public void OnButtonTap()
    {
        audioSource2.PlayOneShot(_buttonTap);
    }

    public void OnStarAppear()
    {
        audioSource2.PlayOneShot(_starAppear);
    }

    public void OnStep()
    {
        audioSource2.PlayOneShot(_steps[Random.Range(0, _steps.Count-1)]);
    }

    public void OnGetHit()
    {
        audioSource2.PlayOneShot(_getHit[Random.Range(0, _getHit.Count - 1)]);
    }

    public void OnTakeXP()
    {
        audioSource2.PlayOneShot(_takeXP);
    }

    public void OnSkill()
    {
        audioSource2.PlayOneShot(_skill);
    }

    public void OnLevelUp()
    {
        audioSource2.PlayOneShot(_levelUp);
    }

    public void OnEnemyGetHit()
    {
        audioSource2.PlayOneShot(_enemyGetHit[Random.Range(0, _enemyGetHit.Count - 1)]);
    }

    public void OnTakeSpecial()
    {
        audioSource2.PlayOneShot(_takeSpecial);
    }
    public void BaseAttack()
    {
        audioSource2.PlayOneShot(_baseAttack);
    }

    private void SwitchAudio(AudioClip clip)
    {
        StartCoroutine(ISwitchAudio(clip));
    }

    private IEnumerator ISwitchAudio(AudioClip clip)
    {
        float elapsedTime = 0;

        while (elapsedTime <1f)
        {
            if (!_soundOff)
            {
                audioSource1.volume = 1 - elapsedTime;
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }
        audioSource1.clip = clip;
        audioSource1.Play();
        audioSource1.volume = 0;

        elapsedTime = 0;

        while (elapsedTime < 1f)
        {
            if (!_soundOff)
            {
                audioSource1.volume = 0 + elapsedTime;
                elapsedTime += Time.deltaTime;
            }
            yield return null;
        }
    }

    public void SwitchSound()
    {
        _soundOff = !_soundOff;

        if (!_soundOff)
        {
            audioSource1.volume = 1;
            audioSource2.volume = 1;
            PrefsManager.ChangeSound(1);
        }

        else
        {
            audioSource1.volume = 0;
            audioSource2.volume = 0;
            PrefsManager.ChangeSound(0);
        }
    }
}
