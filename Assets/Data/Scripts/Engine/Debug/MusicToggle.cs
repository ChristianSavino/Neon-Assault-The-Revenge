using Keru.Scripts.Engine.Master;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Engine.Debug
{   
    public class MusicToggle : MonoBehaviour
    {
        private bool _isAssaultMusic;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                _isAssaultMusic = !_isAssaultMusic;

                JukeBox.jukebox.ChangeSong(_isAssaultMusic ? MusicTracks.Assault : MusicTracks.Ambience, true);
            }
        }
    }
}