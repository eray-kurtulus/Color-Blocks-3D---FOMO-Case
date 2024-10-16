using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

    protected override void Awake()
    {
        base.Awake();
        
        // Hide Win UI
        winUIRectTransform.gameObject.SetActive(false);
        failUIRectTransform.gameObject.SetActive(false);
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
        winUIRectTransform.localScale = Vector3.zero;
        winUIRectTransform.gameObject.SetActive(true);
        winUIRectTransform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack);
        
        // Add LoadNextLevel to nextLevelButton
        nextLevelButton.onClick.AddListener(LevelManager.Instance.LoadNextLevel);
    }

    public void ShowFailUI()
    {
        // Show Fail UI (the retry level button)
        failUIRectTransform.localScale = Vector3.zero;
        failUIRectTransform.gameObject.SetActive(true);
        failUIRectTransform.DOScale(Vector3.one, 0.8f).SetEase(Ease.OutBack);
        
        // Add ReloadScene to retryLevelButton
        retryLevelButton.onClick.AddListener(LevelManager.Instance.ReloadScene);
    }
}
