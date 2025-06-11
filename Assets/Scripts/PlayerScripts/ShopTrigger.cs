using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShopTrigger : MonoBehaviour
{
    [Header("Shop Scene Settings")]
    public string shopSceneName = "ShopScene";

    [Header("Audio")]
    public AudioClip doorOpenSFX;
    private AudioSource audioSource;

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