using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    // This MonoBehaviour handles the UI.
    // In the scope of this clone, only the level text, remaining moves text,
    // Win/Fail UIs and the next level button is covered.

    [SerializeField] private RectTransform winUIRectTransform = default;
    [SerializeField] private Button nextLevelButton = default;
    [Space]
    [SerializeField] private RectTransform failUIRectTransform = default;
    [SerializeField] private Button retryLevelButton = default;
    [Space]
    [SerializeField] private TextMeshProUGUI levelText = default;
    [SerializeField] private TextMeshProUGUI movesText = default;

    private void Awake()
    {
        // Hide Win UI
        winUIRectTransform.gameObject.SetActive(false);
        failUIRectTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        // Doing these at Start to make sure LevelManager is instantiated

        // Add LoadNextLevel to nextLevelButton
        nextLevelButton.onClick.AddListener(LevelManager.Instance.LoadNextLevel);
        
        // Add ReloadScene to retryLevelButton
        retryLevelButton.onClick.AddListener(LevelManager.Instance.ReloadScene);
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }

    public void HideMovesText()
    {
        movesText.gameObject.SetActive(false);
    }
    
    public void UpdateMovesText(int remainingMoves)
    {
        movesText.text = remainingMoves.ToString();
    }

    public void ShowWinUI()
    {
        // Show Win UI (the next level button)
    }

    public void ShowFailUI()
    {
        // Show Fail UI (the retry level button)
    }
}
