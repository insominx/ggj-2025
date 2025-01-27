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
    GameState nextPhaseState; // State to go back to if you are still alive at end of play.

    // State management.
    int currentGameStateIdx = 0;
    GameState currentState { get { return gameStates[currentGameStateIdx]; } }

    // Game information.
    public List<GameObject> enemies = new();
    bool victory = false; // Most recent state.

    // Public Unity interface. /////////////////////////////////////////////////

    void Awake()
    {
        gameStates = new GameState[]
        {
            new StartState(),
            new InformState("Wait", "", 5),
            new InformState("Title", "Bubble Command\nby\nBubbltroop", 10),
            new SpawnCitiesState(this),
            nextPhaseState = new InformState("GetReady", "Get ready.", 10, true),
            new PlayState(this),
            gameConclusion = new InformState("Report", "<TBD>", 10),
            new TransitionToStateOnCondition(this, nextPhaseState, gameWasWon),
            new DestroyEverythingState(this)
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
            var state = currentState;

            // By default plan to advance to next state (wrapping around).
            // Note that stopping a state might override this value.
            currentGameStateIdx = (currentGameStateIdx + 1) % gameStates.Length;

            StopState(state);

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
        victory = true;
        gameConclusion.SetText("Victory!");
    }

    public void ReportDefeat()
    {
        victory = false;
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

    bool firing = false;
    public void StartFiringMissiles()
    {
        firing = true;
        StartCoroutine(SpawnMissiles());
    }

    public void StopFiringMissiles()
    {
        firing = false;
    }

    IEnumerator SpawnMissiles()
    {
        Debug.Log("Spawning missile");
        var missile = SpawnMissile();

        if (!missile)
        {
            yield break;
        }

        // Find a random base and steer towards it.
        var city = Living.GetRandomLivingThing();
        // Not quite working for some reason.
        // TODO: FIXME!
        missile.SteerTowards(city.gameObject.transform.position);

        yield return new WaitForSeconds(1f);

        // Some game state check
        if (firing && Missile.Count < 10) // Check for too intense
        {
            StartCoroutine(SpawnMissiles());
        }
    }

    public Missile SpawnMissile()
    {
        if (enemies.Count == 0)
        {
            return null;
        }
        
        Debug.Log("Finding a random element in " + enemies.Count);
        int rand = UnityEngine.Random.Range(0, enemies.Count);
        Debug.Log("Got " + rand);

        GameObject randomLocat = enemies[rand];
        var missile = randomLocat.transform.GetChild(0).GetComponent<MissileSpawner>().SpawnMissile();
        return missile;
    }

    public void DestroyAllCities()
    {
        Living.DestroyAll();
    }

    public void DestroyAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            GameObject.Destroy(enemy);
        }
        enemies.Clear();
    }

    bool gameWasWon()
    {
        return victory;
    }

    public void OverrideNextState(GameState state)
    {
        currentGameStateIdx = Array.IndexOf(gameStates, state);
    }

    public bool MissilesAreInTheAir()
    {
        return Missile.Count > 0;
    }
}
