using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public bool inMenu;

    public AudioSource music;

    private void Start()
    {
        music = GetComponent<AudioSource>();
        Debug.Log(GameObject.FindGameObjectsWithTag("MusicPlayer").Length);
        if (GameObject.FindGameObjectsWithTag("MusicPlayer").Length == 1)
        {
            DontDestroyOnLoad(this);
            if (inMenu)
            {
                music.clip = menuMusic;
            }
            else
            {
                music.clip = gameMusic;
            }
            music.Play();
        }
        else
        {
            MusicPlayer activePlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
            if (inMenu)
            {
                if (activePlayer.music.clip != activePlayer.menuMusic)
                {
                    activePlayer.music.clip = activePlayer.menuMusic;
                    activePlayer.music.Play();
                }
            }
            else
            {
                if (activePlayer.music.clip != activePlayer.gameMusic)
                {
                    activePlayer.music.clip = activePlayer.gameMusic;
                    activePlayer.music.Play();
                }

                Destroy(gameObject);
            }
        }
    }
}
    
