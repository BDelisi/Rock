using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject audioPlayer;

    public List<GameObject> players = new List<GameObject>();

    private void Start()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            players.Add(player);
        }
    }

    public void playSound(float distance, AudioClip sound, float pitch, float volume)
    {
        GameObject temp = Instantiate(audioPlayer, new Vector3(transform.position.x, transform.position.y + distance, transform.position.z), Quaternion.identity, transform);
        temp.GetComponent<SoundPlayer>().Play(sound, pitch, volume);
    }

    public void playSound(Vector3 location, AudioClip sound, float pitch, float volume)
    {
        float distance = float.MaxValue;
        foreach (GameObject player in players)
        {
            float temp = Vector3.Distance(location, player.transform.position);
            if (temp < distance) 
            { 
                distance = temp; 
            }
        }
        playSound(distance, sound, pitch, volume);
    }
}
