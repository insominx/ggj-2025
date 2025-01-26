using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manage game state machine etc.
public class GameManager : MonoBehaviour
{
    // Editor/data/prefab interface.
    [Header("References)")]
    [SerializeField] GameObject prefabMissileSpawner; // Enemy missile spawner.
    [SerializeField] GameObject prefabCity; // You defend these.

    // States.
    public GameState[] gameStates;
    InformState gameConclusion;

    // State management.
    int currentGameStateIdx = 0;
    GameState currentState { get { return gameStates[currentGameStateIdx]; } }

    // Game information.
    public List<GameObject> cities = new();
    public List<GameObject> enemies = new();

    // Public Unity interface. /////////////////////////////////////////////////

    void Awake()
    {
        gameStates = new GameState[]
        {
            new StartState(),
            new InformState("Wait", "", 5),
            new InformState("Title", "Bubble Command\nby\nBubbltroop", 10),
            new InformState("GetReady", "Get ready.", 10, true),
            new PlayState(this),
            gameConclusion = new InformState("Report", "<TBD>", 10),
        };
    }

    void Start()
    {
        // Nada ftm.
        StartState(currentState);
    }

    // Update is called once per frame.
    void Update()
    {
        currentState.Update();

        if (currentState.ShouldEnd())
        {
            StopState(currentState);

            // TODO: Update this in a better way (don't want to repeat start or title).
            currentGameStateIdx = (currentGameStateIdx + 1) % gameStates.Length;

            StartState(currentState);
        }
    }

    void StopState(GameState state)
    {
        state.Stop();
    }

    void StartState(GameState state)
    {
        Debug.Log("Game Manager starting game state " + state.name);
        state.Start();
    }

    public void ReportVictory()
    {
        gameConclusion.SetText("Victory!");
    }

    public void ReportDefeat()
    {
        gameConclusion.SetText("Defeat!");
    }

    public GameObject SpawnBase(Pose pose)
    {
        //var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var sideLength = 0.1f;
        var position = pose.position + new Vector3(0, sideLength / 2, 0);
        var city = GameObject.Instantiate(prefabCity, position, pose.rotation);

        // Position and scale it.
        city.transform.localScale = new Vector3(sideLength, sideLength, sideLength);
        //city.transform.SetPositionAndRotation(zz, pose.rotation);

        cities.Add(city);

        return city;
    }

    public GameObject SpawnEnemy(Pose pose)
    {
        //var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        var sideLength = 0.1f;
        var position = pose.position + pose.forward * sideLength / 2;
        var enemy = GameObject.Instantiate(prefabMissileSpawner, position, pose.rotation);

        // Tell spawner not to collide with itself.
        //enemy.GetComponent<MissileSpawner>().parentObject = enemy;

        // Position and scale it.
        enemy.transform.localScale = new Vector3(sideLength, sideLength, sideLength);
        //enemy.transform.SetPositionAndRotation(zz, pose.rotation);

        enemies.Add(enemy);

        return enemy;
    }

    public void StartFiringMissiles()
    {
        StartCoroutine(SpawnMissiles());
    }
    
    IEnumerator SpawnMissiles()
    {
        SpawnMissile();
        
        Debug.Log("Spawning missile");

        yield return new WaitForSeconds(1f);

        // Some game state check

        StartCoroutine(SpawnMissiles());

    }

    public void SpawnMissile()
    {

        if (enemies.Count == 0)
            return;
        
        Debug.Log("Finding a random element in " + enemies.Count);
        int rand = UnityEngine.Random.Range(0, enemies.Count);
        Debug.Log("Got " + rand);
        
        GameObject randomLocat = enemies[rand];
        randomLocat.transform.GetChild(0).GetComponent<MissileSpawner>().SpawnMissile();
    }

    public void DestroyAllCities()
    {
        foreach (var city in cities)
        {
            GameObject.Destroy(city);
        }
        cities.Clear();
    }

    public void DestroyAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }
        enemies.Clear();
    }
}
