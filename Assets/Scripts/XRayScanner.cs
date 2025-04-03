using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class XRayScanner : MonoBehaviour
{
    public Image scannerDisplay;
    public Image progressBar;
    public float scanTime = 3f;
    public AudioClip scanSound;
    public AudioClip scanCompleteSound;

    private AudioSource audioSource;
    private Coroutine currentScan;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        scannerDisplay.gameObject.SetActive(false);
        progressBar.fillAmount = 0;
    }

    public void ScanItem(Item item)
    {
        if (currentScan != null)
        {
            StopCoroutine(currentScan);
        }
        currentScan = StartCoroutine(ScanningRoutine(item));
    }

    private IEnumerator ScanningRoutine(Item item)
    {
        // Play scan sound
        if (audioSource && scanSound)
        {
            audioSource.PlayOneShot(scanSound);
        }

        // Show scanning animation
        float elapsedTime = 0f;
        while (elapsedTime < scanTime)
        {
            elapsedTime += Time.deltaTime;
            progressBar.fillAmount = elapsedTime / scanTime;
            yield return null;
        }

        // Show X-ray result
        if (item.xrayImage != null)
        {
            scannerDisplay.sprite = item.xrayImage;
            scannerDisplay.gameObject.SetActive(true);
            item.ShowXRayImage();
        }

        // Play complete sound
        if (audioSource && scanCompleteSound)
        {
            audioSource.PlayOneShot(scanCompleteSound);
        }

        Debug.Log("X-ray scan complete. Hidden item detected: " + item.isHidden);
    }

    public void ClearScan()
    {
        scannerDisplay.gameObject.SetActive(false);
        progressBar.fillAmount = 0;
        if (currentScan != null)
        {
            StopCoroutine(currentScan);
            currentScan = null;
        }
    }
}