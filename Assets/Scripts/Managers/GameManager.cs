using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void WinCond();
    public WinCond winCond;

    public delegate void LoseCond();
    public LoseCond loseCond;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        winCond = InvokeWinCond;
        loseCond = InvokeLoseCond;
    }
    private void InvokeWinCond()//use when player loses
    {
        Debug.Log("Win invoked"); ;//send message to uimanager to popup losed
    }
    private void InvokeLoseCond()//use when player wins
    {
        Debug.Log("Lose invoked");//send message to uimanager to popup wined
    }
}
