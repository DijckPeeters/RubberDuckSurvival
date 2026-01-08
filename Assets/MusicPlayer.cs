using UnityEngine;
using System.Collections.Generic;

public class MusicPlayer : MonoBehaviour
{
    [Header("Audio Instellingen")]
    public AudioSource audioSource;
    public AudioClip[] playlist; // Sleep hier al je MP3 bestanden in via de Inspector

    void Start()
    {
        // Als er liedjes in de lijst staan, start het eerste willekeurige nummer
        if (playlist.Length > 0)
        {
            PlayRandomSong();
        }
    }

    void Update()
    {
        // Controleert elke frame of het liedje is afgelopen
        if (!audioSource.isPlaying)
        {
            PlayRandomSong();
        }
    }

    public void PlayRandomSong()
    {
        // Kiest een willekeurig getal tussen 0 en de lengte van je lijst
        int randomIndex = Random.Range(0, playlist.Length);

        // Koppelt het gekozen liedje aan de AudioSource
        audioSource.clip = playlist[randomIndex];

        // Speelt het liedje af
        audioSource.Play();
    }
}