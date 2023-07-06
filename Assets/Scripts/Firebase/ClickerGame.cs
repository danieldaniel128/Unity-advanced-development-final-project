using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickerGame : MonoBehaviour
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _text;

    int _score = 0;

    public int Score { get => _score; private set => _score = value; }

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }
    public void SetScore(int score)
    {
        Score = score;
        _text.text = Score.ToString();
    }

    private void OnButtonClicked()
    {
        SetScore(Score + 1);
    }

}

