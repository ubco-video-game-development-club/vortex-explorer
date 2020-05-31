using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Vortex vortexPrefab;
    public Asteroids asteroidsPrefab;
    public float vortexCooldown = 1f;
    public float vortexBuildTime = 1f;
    public float minVortexPower = 1f;
    public float maxVortexPower = 3f;
    public int asteroidSpawnFrequency = 200;
    public int asteroidSpawnCounter = 0;

    private bool canSpawnVortex;
    private bool canSpawnAsteroids;
    private float vortexPowerProgress;


    void Start() {
        canSpawnVortex = true;
        canSpawnAsteroids = true;
    }

    void Update() {
        // upon pressing spacebar
        if (Input.GetKeyDown(KeyCode.Space)) {
            // reload the current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // while holding the left-click button
        if (Input.GetButton("Fire1") && canSpawnVortex) {
            // build up the power of the vortex over time
            vortexPowerProgress = Mathf.Clamp(vortexPowerProgress + Time.deltaTime, 0, vortexBuildTime);
        }

        // upon releasing the left-click button
        if (Input.GetButtonUp("Fire1") && canSpawnVortex) {
            // start the vortex spawn cooldown
            StartCoroutine(SpawnVortexCooldown());

            // get the current mouse position in world coordinates
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // spawn a new vortex object at the mouse position
            Vortex vortex = Instantiate(vortexPrefab, mousePos, Quaternion.identity);

            // activate the vortex at the current power level
            float powerLevel = Mathf.Lerp(minVortexPower, maxVortexPower, vortexPowerProgress / vortexBuildTime);
            vortex.Activate(powerLevel);

            // reset the power progress
            vortexPowerProgress = 0;
        }

        //SpawnAsteroids
        asteroidSpawnCounter++;
        if (asteroidSpawnCounter % asteroidSpawnFrequency == 0) {
            //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            Asteroids asteroid = Instantiate(asteroidsPrefab, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
            asteroid.transform.position = new Vector3(asteroid.transform.position.x+20, asteroid.transform.position.y, 0); 
        }
    }

    private IEnumerator SpawnVortexCooldown() {
        canSpawnVortex = false;
        yield return new WaitForSeconds(vortexCooldown);
        canSpawnVortex = true;
    }
}
