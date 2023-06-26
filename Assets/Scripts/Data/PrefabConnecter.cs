using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabConnecter : MonoBehaviour 
{
    //[SerializeField] CellStateEnum[] PossibleToConnectStates; 
    [SerializeField] List<GameObject> TopCornerLeft;
    [SerializeField] List<GameObject> TopCornerMid;
    [SerializeField] List<GameObject> TopCornerRight;
    [SerializeField] List<GameObject> MidLeft;
    [SerializeField] List<GameObject> MidRight;
    [SerializeField] List<GameObject> BottomCornerMid;
    [SerializeField] List<GameObject> BottomCornerRight;
    [SerializeField] List<GameObject> BottomCornerLeft;
}
/*

TopCornerLeft
TopCornerMid
TopCornerRight
BottomCornerMid
BottomCornerRight
BottomCornerLeft
MidLeft
MidRight
MidMidlle 

 */