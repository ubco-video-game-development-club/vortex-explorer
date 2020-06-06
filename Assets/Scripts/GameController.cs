using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [Header("Game Settings")]
    public Player playerPrefab;
    public bool isTimed = false;
    public float gameLength = 30f;
    public float startDelay = 5f;
    public float winDelay = 5f;
    public int pointsPerAsteroid = 10;
    public int pointsPerVortexCrush = 20;

    [Header("Vortex Settings")]
    public Vortex vortexPrefab;
    public SpriteRenderer vortexIndicatorPrefab;
    public LayerMask vortexTargetLayers;
    [Tooltip("The base radius of a vortex.")]
    public float vortexBaseRadius = 0.5f;
    [Tooltip("How much radius does each point of vortex scale add.")]
    public float vortexRadiusGrowthRate = 0.5f;
    [Tooltip("The base pull strength of the inner point of a vortex.")]
    public float vortexBaseInnerStrength = 2f;
    [Tooltip("The base pull strength of the outer edge of a vortex.")] 
    public float vortexBaseOuterStrength = 1f;
    [Tooltip("How much inner pull strength does each point of scale add.")]
    public float vortexInnerStrengthGrowthRate = 2f;
    [Tooltip("How much outer pull strength does each point of scale add.")]
    public float vortexOuterStrengthGrowthRate = 1f;
    [Tooltip("How long you must wait before placing another vortex.")]
    public float vortexCooldown = 1f;
    [Tooltip("How long it takes to reach max vortex scale when holding down left-click.")]
    public float vortexBuildTime = 1f;
    [Tooltip("How much are the vortex values scaled by for the smallest vortex size.")]
    public float vortexMinScale = 1f;
    [Tooltip("How much are the vortex values scaled by for the largest vortex size.")]
    public float vortexMaxScale = 3f;
    [Tooltip("The force threshold at which objects inside the vortex will be crushed.")]
    public float vortexCrushForceThreshold = 10f;
    [Tooltip("The radius threshold at which objects inside the vortex will be crushed.")]
    public float vortexCrushRadiusThreshold = 2f;
    [Tooltip("The color of the indicator when the vortex radius is smaller than the crush radius threshold.")]
    public Color vortexRegularIndicatorColor;
    [Tooltip("The color of the indicator when the vortex radius is greater than the crush radius threshold.")]
    public Color vortexCrushIndicatorColor;

    [Header("Asteroid Settings")]
    public Asteroid asteroidPrefab;
    [Tooltip("Minimum number of seconds between each asteroid spawning.")]
    public float asteroidMinSpawnInterval = 1f;
    [Tooltip("Maximum number of seconds between each asteroid spawning.")]
    public float asteroidMaxSpawnInterval = 2f;
    [Tooltip("Minimum speed of the asteroids when they spawn.")]
    public float asteroidMinSpeed = 1f;
    [Tooltip("Maximum speed of the asteroids when they spawn.")]
    public float asteroidMaxSpeed = 2f;

    private Player player;
    private Transform spawnParent;
    private bool canSpawnVortex;
    private bool isBuildingVortex;
    private bool isSpawningAsteroids;
    private bool isGameActive;
    private int score;
    private float vortexScaleProgress;
    private SpriteRenderer vortexIndicator;

    void Awake() {
        // reinforce a singleton pattern for this object
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }

        // spawn the vortex indicator and disable it
        vortexIndicator = Instantiate(vortexIndicatorPrefab);
        vortexIndicator.enabled = false;
    }

    void Start() {
        // initialize the score
        score = 0;
        HUD.instance.SetScore(score);
        HUD.instance.ShowScore(false);
        
        // hide the main menu button
        HUD.instance.ShowMainMenuButton(false);
    }

    void Update() {
        // upon pressing spacebar
        if (Input.GetKeyDown(KeyCode.Space)) {
            // reload the current level
            ResetGame();
        }

        // upon pressing escape
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // go to main menu
            MainMenu();
        }

        // helper function to handle click input relating to vortex placement
        HandleVortexInput();
    }

    public void StartGame() {
        // create the spawn parent transform
        if (spawnParent != null) {
            Destroy(spawnParent.gameObject);
        }
        spawnParent = new GameObject("SpawnedObjects").transform;

        // spawn the player
        Vector2 spawnPos = (Camera.main.ScreenToWorldPoint(Vector2.zero).x + 1.5f) * Vector2.right;
        player = Instantiate(playerPrefab, spawnPos, Quaternion.identity, spawnParent);

        // set the camera to follow the player
        Camera.main.GetComponent<CameraFollow>().Follow(player.transform);

        // reset the score
        score = 0;
        HUD.instance.SetScore(score);
        HUD.instance.ShowScore(true);

        // show the main menu button
        HUD.instance.ShowMainMenuButton(true);

        // start the game loop coroutine
        if (isTimed) {
            StartCoroutine(TimedGameLoop());
        } else {
            StartCoroutine(InfiniteGameLoop());
        }
    }

    public void StopGame() {
        // reset the camera position
        Camera.main.GetComponent<CameraFollow>().Follow(null);
        Camera.main.transform.position = Vector3.forward * Camera.main.transform.position.z;

        // despawn all spawned objects
        if (spawnParent != null) {
            Destroy(spawnParent.gameObject);
        }
        spawnParent = null;
        player = null;

        // disable vortex placement inputs
        canSpawnVortex = false;
        vortexIndicator.enabled = false;
        vortexScaleProgress = 0;

        // save the current score in Google Play
        Social.ReportScore(score, "CgkImvWcv6cMEAIQAQ", success => {});
        Social.ShowLeaderboardUI();

        // stop the game loop coroutines
        StopAllCoroutines();
    }

    public void LoseGame() {
        StopGame();
        HUD.instance.OpenLoseMenu();
    }

    public void WinGame() {
        StopGame();
        HUD.instance.OpenWinMenu();
    }

    public void ResetGame() {
        StopGame();
        HUD.instance.CloseHUD();
        StartGame();
    }

    public void MainMenu() {
        StopGame();
        HUD.instance.ShowScore(false);
        HUD.instance.ShowMainMenuButton(false);
        HUD.instance.CloseHUD();
        HUD.instance.OpenMainMenu();
    }

    public void AddVortexCrushPoints() {
        score += pointsPerVortexCrush;
        HUD.instance.SetScore(score);
    }

    public void AddAsteroidPoints() {
        score += pointsPerAsteroid;
        HUD.instance.SetScore(score);
    }

    private void HandleVortexInput() {
        // upon pressing the left-click button
        if (Input.GetButtonDown("Fire1") && canSpawnVortex) {
            // enable the visual indicator
            vortexIndicator.enabled = true;

            // start building the vortex scale
            isBuildingVortex = true;
        }

        // while holding the left-click button
        if (Input.GetButton("Fire1") && canSpawnVortex && isBuildingVortex) {
            // build up the power of the vortex over time
            vortexScaleProgress = Mathf.Clamp(vortexScaleProgress + Time.deltaTime, 0, vortexBuildTime);

            // get the current mouse position in world coordinates
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // set the indicator at the current mouse position
            vortexIndicator.transform.position = mousePos;

            // calculate the current radius based on the vortex scale
            float vortexScale = Mathf.Lerp(vortexMinScale, vortexMaxScale, vortexScaleProgress / vortexBuildTime) - 1;
            float radius = vortexBaseRadius + vortexRadiusGrowthRate * vortexScale;

            // scale the indicator based on the current radius
            vortexIndicator.transform.localScale = Vector2.one * radius * 2;

            // set the indicator color based on the current radius
            vortexIndicator.color = radius > vortexCrushRadiusThreshold ? vortexCrushIndicatorColor : vortexRegularIndicatorColor;
        }

        // upon releasing the left-click button
        if (Input.GetButtonUp("Fire1") && canSpawnVortex && isBuildingVortex) {
            // start the vortex spawn cooldown
            StartCoroutine(HandleVortexCooldown());

            // stop building the vortex scale
            isBuildingVortex = false;

            // disable the visual indicator
            vortexIndicator.enabled = false;

            // get the current mouse position in world coordinates
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // spawn a new vortex object at the mouse position
            Vortex vortex = Instantiate(vortexPrefab, mousePos, Quaternion.identity, spawnParent);

            // calculate the vortex stats based on the current vortex scale
            float vortexScale = Mathf.Lerp(vortexMinScale, vortexMaxScale, vortexScaleProgress / vortexBuildTime) - 1;
            float radius = vortexBaseRadius + vortexRadiusGrowthRate * vortexScale;
            float innerStrength = vortexBaseInnerStrength + vortexInnerStrengthGrowthRate * vortexScale;
            float outerStrength = vortexBaseOuterStrength + vortexOuterStrengthGrowthRate * vortexScale;

            // activate the vortex at the current vortex scale
            vortex.Activate(radius, innerStrength, outerStrength, vortexCrushForceThreshold, vortexCrushRadiusThreshold, vortexTargetLayers);

            // reset the scale progress
            vortexScaleProgress = 0;
        }

        // upon pressing the right-click button
        if (Input.GetButtonDown("Fire2") && isBuildingVortex) {
            // stop building the vortex scale
            isBuildingVortex = false;

            // disable the visual indicator
            vortexIndicator.enabled = false;

            // reset the scale progress
            vortexScaleProgress = 0;
        }
    }

    private IEnumerator HandleVortexCooldown() {
        canSpawnVortex = false;
        yield return new WaitForSeconds(vortexCooldown);
        canSpawnVortex = true;
    }

    private IEnumerator TimedGameLoop() {
        // wait before starting the asteroid waves and allowing vortex inputs
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(SpawnAsteroids());
        canSpawnVortex = true;

        // disable asteroid waves after the game is over
        yield return new WaitForSeconds(gameLength);
        isSpawningAsteroids = false;

        // after a short delay open the win menu
        yield return new WaitForSeconds(winDelay);
        WinGame();
    }

    private IEnumerator InfiniteGameLoop() {
        // wait before starting the asteroid waves and allowing vortex inputs
        yield return new WaitForSeconds(startDelay);
        StartCoroutine(SpawnAsteroids());
        canSpawnVortex = true;
    }

    private IEnumerator SpawnAsteroids() {
        // calculate the range of y positions the asteroids could spawn at
        float topY = Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height * 0.9f).y;
        float bottomY = Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height * 0.1f).y;

        // spawn asteroids on a random repeating interval
        isSpawningAsteroids = true;
        while (isSpawningAsteroids) {
            // get the x position directly to the right of the screen
            float spawnX = Camera.main.ScreenToWorldPoint(Vector3.right * Screen.width).x + 1f;

            // choose a random y spawn location
            float spawnY = Random.Range(bottomY, topY);

            // spawn an asteroid
            Asteroid asteroid = Instantiate(asteroidPrefab, new Vector2(spawnX, spawnY), Quaternion.identity, spawnParent);

            // apply a random initial force to the asteroid
            float speed = Random.Range(asteroidMinSpeed, asteroidMaxSpeed);
            asteroid.Push(Vector2.left * speed);

            // wait a random interval before spawning the next asteroid
            float spawnInterval = Random.Range(asteroidMinSpawnInterval, asteroidMaxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
