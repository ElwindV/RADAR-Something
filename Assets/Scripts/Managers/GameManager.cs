using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Managers
{
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
        private float _deathTime;
        private bool _isDead;
        [Range(0, 20)] public float extraTimePerEnemy = 10f;

        private int _enemyCount;
        [NonSerialized] public int multiplier = 1;

        [Header("Spawning of Enemies")]
        public Text waveText;
        public Text remainingText;
        public GameObject[] enemies;
        private GameObject[] _spawnPoints;
        private int _wave;
        public int[] enemyPerWave;
        private float _waveTime;

        private Player _player;
        private bool _hasWon;

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
            if (timeText != null) timeText.text = "Time: " + (int)_time;
        
            if (scoreText != null) scoreText.text = "Score: " + score + "   " + multiplier + "X";

            if (waveText != null) waveText.text = "Wave: " + _wave + "/" + enemyPerWave.Length;
        
            if (remainingText != null) remainingText.text = "Remaining: " + _enemyCount;
        }

        private void CheckForFailure() 
        {
            if (! (_time < 0f || _isDead || _hasWon)) {
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
            if (!(_enemyCount == 0 && !_hasWon && !_isDead)) {
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
                StartCoroutine(SpawnEnemies(enemyPerWave[_wave - 1]));
                AnalyticsEvent.LevelStart($"wave_{_wave}");
            }
            else {
                _hasWon = true;
            }
        
        }

        public void ResetMultiplier()
        {
            multiplier = 1;
        }

        public void AddScore(int value)
        {
            score += value * multiplier;
            multiplier++;
            _time += extraTimePerEnemy;
            _enemyCount--;
        }

        public void EndGame()
        {
            AnalyticsEvent.GameOver();
            AnalyticsEvent.LevelFail($"wave_{_wave}");
            _isDead = true;
        }

        public void ExitGame() 
        {
            AnalyticsEvent.LevelQuit($"wave_{_wave}");
            Application.Quit();
        }

        private IEnumerator SpawnEnemies(int spawnThisWave)
        {
            _enemyCount += spawnThisWave;
            for (var i = 0; i < spawnThisWave; i++)
            {
                var toInstantiate = enemies[Mathf.RoundToInt(Random.Range(0, enemies.Length-1))];
                var spawnPointTransform = _spawnPoints[Mathf.RoundToInt(Random.Range(0, _spawnPoints.Length-1))].transform;
                var instantiated = Instantiate(toInstantiate, spawnPointTransform);
                instantiated.transform.parent = spawnPointTransform;
            
                yield return new WaitForSeconds(2f);
            }
            yield return null;
        }
    }
}
