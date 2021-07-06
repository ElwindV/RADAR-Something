using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour {

    [Header("Time")]
    [Range(0, 100)] public float beginTime = 120f;
    private float _time;
    public Text timeText;
    [Range(0, 30)] public float extraTimePerWave;

    [Header("Score")]
    [HideInInspector]
    public int score;
    public Text scoreText;

    [Header("Death")]
    public AnimationCurve slowDownRate;
    public AnimationCurve deathScreenRate;
    public AnimationCurve deathScreenTextRate;
    public Image deathScreen;
    public Text gameOver, totalScore;
    private float _deathTime = 0f;
    [System.NonSerialized] public bool isDead = false;
    [Range(0, 20)] public float extraTimePerEnemy = 10f;

    [System.NonSerialized] public int enemyCount = 0;
    [System.NonSerialized] public int multiplier = 1;

    [Header("Spawning of Enemies")]
    public Text waveText;
    public Text remaining;
    public GameObject[] enemies;
    private GameObject[] _spawnPoints;
    private int _wave = 0;
    public int[] enemyPerWave;
    private float _waveTime = 0f;

    private Player _player;
    private bool _hasWon = false;

    public void Awake()
    {
        AnalyticsEvent.GameStart();
        AnalyticsEvent.LevelStart($"wave_{_wave}");
        _time = beginTime;
        _spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");
        _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    }

    public void Update()
    {
        _time -= Time.deltaTime;
        
        HandleText();

        CheckForFailure();
        CheckForWin();
    }

    private void HandleText() 
    {
        if (timeText == null || scoreText == null || waveText || waveText) {
            return;
        }
        timeText.text = "Time: " + (int)_time;
        scoreText.text = "Score: " + score + "   " + multiplier + "X";
        waveText.text = "Wave: " + _wave + "/" + enemyPerWave.Length;
        waveText.text = "Remaining: " + enemyCount;// + "/" + GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void CheckForFailure() 
    {
        if (! (_time < 0f || isDead || _hasWon)) {
            return;
        }

        _deathTime += Time.unscaledDeltaTime;
        Time.timeScale = slowDownRate.Evaluate(_deathTime);
        var c = deathScreen.color;
        c.a = deathScreenRate.Evaluate(_deathTime);
        deathScreen.color = c;

        var textColor = gameOver.color;
        textColor.a = deathScreenTextRate.Evaluate(_deathTime);
        gameOver.color = textColor;
        totalScore.color = textColor;

        totalScore.text = "Total score: " + score;
        if (!_hasWon) {
            return;
        }
        
        gameOver.text = "You've won!";
        totalScore.text += (" + " + Mathf.FloorToInt(_time) + " seconds = " + score + Mathf.FloorToInt(_time));
    }

    private void CheckForWin() 
    {
        if (!(enemyCount == 0 && !_hasWon && !isDead)) {
            return;
        }

        AnalyticsEvent.LevelComplete($"wave_{_wave}");
        AnalyticsEvent.Custom($"wave_{_wave}_completed", new Dictionary<string, object> {
            {"wave", _wave},
            {"score", score},
            {"time_elapsed", Time.timeSinceLevelLoad - _waveTime}
        });

        _waveTime = Time.timeSinceLevelLoad;
        _wave++;

        if (_wave < enemyPerWave?.Length) {
            _player.RestoreHealth(); _time += extraTimePerWave;
            StartCoroutine(spawnEnemies(enemyPerWave[_wave - 1]));
            AnalyticsEvent.LevelStart($"wave_{_wave}");
        }
        else {
            _hasWon = true;
        }
        
    }

    public void resetMultiplier()
    {
        multiplier = 1;
    }

    public void addScore(int value)
    {
        score += value * multiplier;
        multiplier++;
        _time += extraTimePerEnemy;
        enemyCount--;
    }

    public void endGame()
    {
        AnalyticsEvent.GameOver();
        AnalyticsEvent.LevelFail($"wave_{_wave}");
        isDead = true;
    }

    public void exitGame() 
    {
        AnalyticsEvent.LevelQuit($"wave_{_wave}");
        Application.Quit();
    }

    private IEnumerator spawnEnemies(int spawnThisWave)
    {
        enemyCount += spawnThisWave;
        for (int i = 0; i < spawnThisWave; i++)
        {
            GameObject toInstantiate = enemies[Mathf.RoundToInt(Random.Range(0, enemies.Length-1))];
            Transform transform = _spawnPoints[Mathf.RoundToInt(Random.Range(0, _spawnPoints.Length-1))].transform;
            GameObject instantiated = Instantiate(toInstantiate, transform) as GameObject;
            instantiated.transform.parent = transform;
            yield return new WaitForSeconds(2f);
        }
        yield return null;
    }
}
