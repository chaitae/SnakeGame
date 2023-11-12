using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    // Start is called before the first frame update
    public AudioSource audioSource;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        audioSource= GetComponent<AudioSource>();
        GameManager.OnDeath += PlayFailGameSound;
    }
    private void OnDisable()
    {
        GameManager.OnDeath -= PlayFailGameSound;
    }
    void PlayFailGameSound()
    {
        audioSource.Play();

    }

}
