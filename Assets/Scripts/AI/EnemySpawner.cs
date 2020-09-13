using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    float randX, nextSpawn;
    Vector2 spawnLocation;
    [SerializeField]
    float spawnRate = 2f;
    public bool spawnTimerToggle;

    private void Update()
    {
        if (spawnTimerToggle && Time.time > nextSpawn)
        {
            nextSpawn = Time.time + spawnRate;
            randX = Random.Range(-8.4f, 8.4f);
            spawnLocation = new Vector2(randX, transform.position.y);
            Instantiate(enemy, spawnLocation, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            randX = Random.Range(-8.4f, 8.4f);
            spawnLocation = new Vector2(randX, transform.position.y);
            Instantiate(enemy, spawnLocation, Quaternion.identity);
        }
    }
}
