using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour {

    [Header("Time")]
    [Range(0, 100)] public float beginTime = 120f;
    private float time;
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
    private float deathTime = 0f;
    [System.NonSerialized] public bool isDead = false;
    [Range(0, 20)] public float extraTimePerEnemy = 10f;

    [System.NonSerialized] public int enemyCount = 0;
    [System.NonSerialized] public int multiplier = 1;

    [Header("Spawning of Enemies")]
    public Text waveText;
    public Text Remaining;
    public GameObject[] enemies;
    private GameObject[] spawnPoints;
    private int wave = 0;
    public int[] enemyPerWave;
    private float waveTime = 0f;

    private Player player;
    private bool hasWon = false;

    public void Awake()
    {
        AnalyticsEvent.GameStart();
        AnalyticsEvent.LevelStart($"wave_{wave}");
        time = beginTime;
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
    }

    public void Update()
    {
        time -= Time.deltaTime;
        
        HandleText();

        CheckForFailure();
        CheckForWin();
    }

    private void HandleText() 
    {
        if (timeText == null || scoreText == null || waveText || waveText) {
            return;
        }
        timeText.text = "Time: " + (int)time;
        scoreText.text = "Score: " + score + "   " + multiplier + "X";
        waveText.text = "Wave: " + wave + "/" + enemyPerWave.Length;
        waveText.text = "Remaining: " + enemyCount;// + "/" + GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    private void CheckForFailure() 
    {
        if (! (time < 0f || isDead || hasWon)) {
            return;
        }

        deathTime += Time.unscaledDeltaTime;
        Time.timeScale = slowDownRate.Evaluate(deathTime);
        Color c = deathScreen.color;
        c.a = deathScreenRate.Evaluate(deathTime);
        deathScreen.color = c;

        Color textColor = gameOver.color;
        textColor.a = deathScreenTextRate.Evaluate(deathTime);
        gameOver.color = textColor;
        totalScore.color = textColor;

        totalScore.text = "Total score: " + score;
        if (!hasWon) {
            return;
        }
        
        gameOver.text = "You've won!";
        totalScore.text += (" + " + Mathf.FloorToInt(time) + " seconds = " + score + Mathf.FloorToInt(time));
    }

    private void CheckForWin() 
    {
        if (!(enemyCount == 0 && !hasWon && !isDead)) {
            return;
        }

        AnalyticsEvent.LevelComplete($"wave_{wave}");
        AnalyticsEvent.Custom($"wave_{wave}_completed", new Dictionary<string, object> {
            {"wave", wave},
            {"score", score},
            {"time_elapsed", Time.timeSinceLevelLoad - waveTime}
        });

        waveTime = Time.timeSinceLevelLoad;
        wave++;

        if (wave < enemyPerWave?.Length) {
            player.RestoreHealth(); time += extraTimePerWave;
            StartCoroutine(spawnEnemies(enemyPerWave[wave - 1]));
            AnalyticsEvent.LevelStart($"wave_{wave}");
        }
        else {
            hasWon = true;
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
        time += extraTimePerEnemy;
        enemyCount--;
    }

    public void endGame()
    {
        AnalyticsEvent.GameOver();
        AnalyticsEvent.LevelFail($"wave_{wave}");
        isDead = true;
    }

    public void exitGame() 
    {
        AnalyticsEvent.LevelQuit($"wave_{wave}");
        Application.Quit();
    }

    private IEnumerator spawnEnemies(int spawnThisWave)
    {
        enemyCount += spawnThisWave;
        for (int i = 0; i < spawnThisWave; i++)
        {
            GameObject toInstantiate = enemies[Mathf.RoundToInt(Random.Range(0, enemies.Length-1))];
            Transform transform = spawnPoints[Mathf.RoundToInt(Random.Range(0, spawnPoints.Length-1))].transform;
            GameObject instantiated = Instantiate(toInstantiate, transform) as GameObject;
            instantiated.transform.parent = transform;
            yield return new WaitForSeconds(2f);
        }
        yield return null;
    }
}
