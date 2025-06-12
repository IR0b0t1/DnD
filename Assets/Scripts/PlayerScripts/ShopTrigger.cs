using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;

public class ShopTrigger : MonoBehaviour
{
    [Header("Shop Scene Settings")]
    public string shopSceneName = "ShopScene";

    [Header("Audio")]
    public AudioClip doorOpenSFX;
    private AudioSource audioSource;
    public AudioMixerGroup mixerGroup;

    private bool isTriggered = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            Debug.LogWarning("ShopTrigger: No AudioSource found, added one automatically.", this);
        }

        if (mixerGroup != null)
        {
            audioSource.outputAudioMixerGroup = mixerGroup;
        }
        else
        {
            Debug.LogWarning("Chest: SFX Mixer Group not assigned for chest. Volume control via mixer might not work.", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered) return;

        if (other.CompareTag("Player"))
        {
            isTriggered = true;

            GameState.playerPosition = other.transform.position;
            StartCoroutine(LoadShopSceneWithDelay());
        }
    }

    private IEnumerator LoadShopSceneWithDelay()
    {
        if (audioSource != null && doorOpenSFX != null)
        {
            audioSource.PlayOneShot(doorOpenSFX);
        }
        else
        {
            Debug.LogWarning("ShopTrigger: AudioSource or Door Open SFX not assigned. Playing sound anyway.", this);
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(shopSceneName);
    }
}