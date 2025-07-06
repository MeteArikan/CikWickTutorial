using System;
using MaskTransitions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControllerUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton;

    private void Awake()
    {
        _playButton.onClick.AddListener(OnPlayButtonClick);
        _quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    private void OnPlayButtonClick()
    {
        TransitionManager.Instance.LoadLevel(Consts.SceneNames.GAME_SCENE);
    }

    private void OnQuitButtonClick()
    {
        Debug.Log("Quitting the Game");
        Application.Quit();
    }


}
