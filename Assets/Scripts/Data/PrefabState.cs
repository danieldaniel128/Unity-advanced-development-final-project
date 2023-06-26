using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PrefabState : MonoBehaviour
{
    public CellStateEnum CellStateEnum;
    public GameObject CellPrefab;
    public PrefabConnecter prefabConnecter 
    { 
        get 
        { 
            if (CellPrefab != null)
                return CellPrefab?.GetComponent<PrefabConnecter>(); 
            return null; 
        }
    }

}
