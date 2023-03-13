using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WaveInitializer : MonoBehaviour
{
    public List<Wave> Waves;
    [SerializeField] private List<Transform> spawnPositions;
    [SerializeField] private float time;
    [SerializeField] private PlayerCore player;
    [SerializeField] private Text timeText;
    [SerializeField] private GameObject startButton;
    private int currentWave;
    private int startedCount;
    private int currentCount;
    private void Start()
    {
        for (int i = 0; i < Waves.Count; i++)
        {
            for (int j = 0; j < Waves[i].Enemies.Count; j++)
            {
                Waves[i].Enemies[j].pool = ObjectPoolSpawner.GetObjectPool(Waves[i].Enemies[j].Prefab) as ObjectPool;
            }
        }
        
    }
    public void StartGame()
    {
        StartCoroutine(StartWaves());
    }
    public IEnumerator StartWaves()
    {
        for (int i = 0; i < Waves.Count; i++)
        {
            StartCoroutine(StartSpawning(i));
            if(timeText != null)
                StartCoroutine(StartChangingTime(Waves[i].Time));
            yield return new WaitForSeconds(Waves[i].Time);
            currentWave++;
        }
        
    }
    private IEnumerator StartChangingTime(float time)
    {
        for (int i = 0; i < time; i++)
        {
            timeText.text = $"{time - i}";
            yield return new WaitForSeconds(1f);
        }
    }
    private IEnumerator StartSpawning(int waveNum)
    {
        for (int i = 0; i < Waves[waveNum].Enemies.Count; i++)
        {
            for (int j = 0; j < Waves[waveNum].Enemies[i].Count; j++)
            {
                var unit = Waves[waveNum].Enemies[i].pool.Instantiate(spawnPositions[Random.Range(0, spawnPositions.Count)].position, Quaternion.identity);
                var component = unit.GetComponent<Unit>();
                component.Pool = Waves[waveNum].Enemies[i].pool;
                component.Activate(player);
                component.OnUnitDestroyed += (unit) => { unit.Pool.Destroy(unit.gameObject); unit.ClearSubs(); };
                yield return new WaitForSeconds(0.1f);
            }
            
        }       
    }
}
