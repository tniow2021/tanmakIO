using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor.PackageManager;
using UnityEngine;

public class InGameNet : MonoBehaviour
{
    List<TempPlayer> players = new List<TempPlayer>();
    Network network = GameManager.Instance.network;

    public void Update()
    {
        foreach (var player in players)
        {
            SendTransform(player);
        }
        print(network.Recieve());
    }
    public void AccessRequest(TempPlayer player)
    {
        players.Add(player);
    }
    void SendTransform(TempPlayer player)
    {
        Vector3 XYZ = player.transform.position;
        string msg = player.UserName + ": " + XYZ.ToString() + "\n";
        byte[] data = Encoding.UTF8.GetBytes(msg);
        network.Send(data);
    }
}
