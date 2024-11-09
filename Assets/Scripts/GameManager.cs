using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int MaxNumberOfShots = 3;
    private int _usedNumberOfShots;
    private IconHandler _iconHandler;

    private float _secondsToWaitBeforeDeathCheck = 2f;

    private List<Baddie> _baddies = new List<Baddie>();

    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private SlingShotHandler _slingShotHandler;


    [Obsolete]
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        _iconHandler = FindAnyObjectByType<IconHandler>();
        Baddie[] baddieArray = FindObjectsOfType<Baddie>();
        for (int i = 0; i < baddieArray.Length; i++)
        {
            _baddies.Add(baddieArray[i]);
        }

    }
    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);
        CheckForLastShot();
    }

    public bool HasEnoughShots()
    {
        if (_usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForLastShot()
    {
        if (_usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck);

        if (_baddies.Count == 0)
        {
            WinGame();
        }
        else
        {
            RestartGame();
        }

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void WinGame()
    {
        _restartScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;
    }

    public void RemoveBaddie(Baddie baddie)
    {
        _baddies.Remove(baddie);
        CheckForAllDeadBoddies();
    }

    private void CheckForAllDeadBoddies()
    {
        if (_baddies.Count == 0)
        {
            WinGame();
        }
    }
}
