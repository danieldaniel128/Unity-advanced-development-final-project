using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[FirestoreData]
public struct UserLoginData
{
    [FirestoreProperty] public string Username { get; set; }
    [FirestoreProperty] public string Password { get; set; }
    [FirestoreProperty] public int Score { get; set; }
}

