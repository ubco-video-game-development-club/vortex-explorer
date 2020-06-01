using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
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
    public Asteroids asteroidsPrefab;
    public int asteroidSpawnFrequency = 200;
    public int asteroidSpawnCounter = 0;

    private bool canSpawnVortex;
    private bool canSpawnAsteroids;
    private bool isBuildingVortex;
    private float vortexScaleProgress;
    private SpriteRenderer vortexIndicator;

    void Start() {
        // spawn the vortex indicator and disable it
        vortexIndicator = Instantiate(vortexIndicatorPrefab);
        vortexIndicator.enabled = false;

        canSpawnVortex = true;
        canSpawnAsteroids = true;
    }

    void Update() {
        // upon pressing spacebar
        if (Input.GetKeyDown(KeyCode.Space)) {
            // reload the current level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

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
            StartCoroutine(SpawnVortexCooldown());

            // stop building the vortex scale
            isBuildingVortex = false;

            // disable the visual indicator
            vortexIndicator.enabled = false;

            // get the current mouse position in world coordinates
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // spawn a new vortex object at the mouse position
            Vortex vortex = Instantiate(vortexPrefab, mousePos, Quaternion.identity);

            // calculate the vortex stats based on the current vortex scale
            float vortexScale = Mathf.Lerp(vortexMinScale, vortexMaxScale, vortexScaleProgress / vortexBuildTime) - 1;
            float radius = vortexBaseRadius + vortexRadiusGrowthRate * vortexScale;
            float innerStrength = vortexBaseInnerStrength + vortexInnerStrengthGrowthRate * vortexScale;
            float outerStrength = vortexBaseOuterStrength + vortexOuterStrengthGrowthRate * vortexScale;

            Debug.Log(innerStrength + ", " + outerStrength);

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
