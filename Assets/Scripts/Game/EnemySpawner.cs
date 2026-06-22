using Enemies;
using Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [Inject] private BlueBot.Factory _blueBotFactory;
        [Inject] private GreenBot.Factory _greenBotFactory;
        [Inject] private RedBot.Factory _redBotFactory;
        [Inject] private SignalBus _signalBus;

        [Inject] private GameManager _gameManager;

        [SerializeField] private List<WaveInfo> _waves;

        private int enemiesCount;
        private int waveIndex = 0;

        private void Start()
        {
            SpawnNextWave();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<EnemyDiedSignal>(OnEnemyDied);
        }

        private void SpawnNextWave()
        {
            if (waveIndex >= _waves.Count)
            {
                StartCoroutine(CompleteLevelRoutine());
                return;
            }
            
            enemiesCount = CountEnemiesInWave(_waves[waveIndex]);
            SpawnWave(_waves[waveIndex]);
            waveIndex++;
        }

        private void SpawnWave(WaveInfo wave)
        {
            foreach(EnemyInfo enemyInfo in wave.enemyInfos) 
            { 
                foreach(Transform spawnPoint in enemyInfo.spawnPoints) 
                {
                    switch(enemyInfo.type)
                    {
                        case EnemyType.BlueBot:
                            var blueBot = _blueBotFactory.Create();
                            blueBot.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                            break;

                        case EnemyType.GreenBot:
                            var greenBot = _greenBotFactory.Create();
                            greenBot.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                            break;

                        case EnemyType.RedBot:
                            var redBot = _redBotFactory.Create();
                            redBot.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
                            break;
                    }
                }
            }
        }

        private int CountEnemiesInWave(WaveInfo wave)
        {
            int count = 0;

            foreach (EnemyInfo enemyInfo in wave.enemyInfos)
            {
                count += enemyInfo.spawnPoints.Count;
            }

            return count;
        }

        private void OnEnemyDied()
        {
            enemiesCount--;

            if (enemiesCount <= 0)
            {
                SpawnNextWave();
            }
        }

        private IEnumerator CompleteLevelRoutine()
        {
            yield return new WaitForSeconds(2f);
            _gameManager.CompleteLevel();
        }
    }
}
