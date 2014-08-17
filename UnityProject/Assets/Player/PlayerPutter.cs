using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Player
{
    public class PlayerPutter : MonoBehaviour
    {
        private void Awake()
        {
            foreach (PlayerData player in FindObjectsOfType<PlayerData>())
            {
                player.transform.parent = gameObject.transform;
                player.enabled = true;
            }
        }
    }
}
