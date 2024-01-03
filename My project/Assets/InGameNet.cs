using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.PackageManager;
using UnityEditor.Tilemaps;
using UnityEngine;

public class InGameNet : MonoBehaviour
{
    List<TempPlayer> players = new List<TempPlayer>();

    private void Start()
    {
        
    }

    public void Update()
    {
        foreach (var player in players)
        {

        }
    }
    public void AccessRequest(TempPlayer player)
    {
        players.Add(player);
    }
}
