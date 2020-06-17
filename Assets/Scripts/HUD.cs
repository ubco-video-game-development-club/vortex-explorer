using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public CanvasGroup mainMenu;
    public CanvasGroup loseMenu;
    public CanvasGroup winMenu;
    public CanvasGroup mainMenuButton;
    public Text scoreText;
    public Text debugText;
    public float fadeTime = 0.2f;

    void Awake() {
        // reinforce a singleton pattern for this object
        if (instance != null) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
    }

    public void OpenMainMenu() {
        EnableMenu(mainMenu, true);
    }

    public void OpenLoseMenu() {
        EnableMenu(loseMenu, true);
    }

    public void OpenWinMenu() {
        EnableMenu(winMenu, true);
    }

    public void CloseHUD() {
        EnableMenu(mainMenu, false);
        EnableMenu(loseMenu, false);
        EnableMenu(winMenu, false);
    }

    public void ShowMainMenuButton(bool show) {
        mainMenuButton.alpha = show ? 1 : 0;
    }

    public void ShowScore(bool show) {
        scoreText.enabled = show;
    }

    public void SetScore(int score) {
        scoreText.text = "Score: " + score;
    }

    public void PrintDebug(string debugString) {
        debugText.text = debugString;
    }

    private void EnableMenu(CanvasGroup menu, bool enabled) {
        // fade the menu in or out
        StartCoroutine(FadeMenu(menu, enabled));

        // set interaction properties on the menu
        menu.interactable = enabled;
        menu.blocksRaycasts = enabled;
    }

    private IEnumerator FadeMenu(CanvasGroup menu, bool fadeIn) {
        // get the initial alpha of the menu
        float startAlpha = menu.alpha;
        
        // determine whether we are fading in or out
        float targetAlpha = fadeIn ? 1 : 0;

        // fade the menu over fadeTime seconds
        float f = 0;
        while (f < fadeTime) {
            f += Time.deltaTime;
            menu.alpha = Mathf.Lerp(startAlpha, targetAlpha, f / fadeTime);
            yield return null;
        }
    }
}
