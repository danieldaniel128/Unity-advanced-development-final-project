using Firebase.Extensions;
using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour
{
    [SerializeField] string _loginDataPath = "/Users/";

    [SerializeField] TMP_InputField _username, _password;

    [SerializeField] Button _loginButton;

    [SerializeField] Button _registerButton;

    [SerializeField] ClickerGame _clickerGame;


    private void Awake()
    {
        _loginButton.onClick.AddListener(OnLoginButtonClicked);
        _registerButton.onClick.AddListener(OnRegisterButtonClicked);
    }

    private void OnRegisterButtonClicked()
    {
        var loginData = new UserLoginData
        {
            Username = _username.text,
            Password = _password.text,
            Score = _clickerGame.Score
        };

        //register the user to Firestore database
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(_loginDataPath + _username.text).SetAsync(loginData);
    }

    private void OnLoginButtonClicked()
    {
        //load the firestone document
        var firestore = FirebaseFirestore.DefaultInstance;
        firestore.Document(_loginDataPath + _username.text).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to load user data");
                return;
            }

            var snapshot = task.Result;
            if (!snapshot.Exists)
            {
                Debug.LogError("User does not exist");
                return;
            }

            var loginData = snapshot.ConvertTo<UserLoginData>();
            if (loginData.Password != _password.text)
            {
                Debug.LogError("Wrong password");
                return;
            }

            _clickerGame.SetScore(loginData.Score);
        });
    }
}
