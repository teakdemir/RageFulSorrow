using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class InstructionsManager : MonoBehaviour
{
    public Image fadeImage;  // Assign your black UI Image in inspector
    private void Start()
    {
        StartCoroutine(ShowInstructions());
    }

    IEnumerator ShowInstructions()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(3f);

        // Fade out
        if (fadeImage != null)
        {
            float elapsedTime = 0;
            Color startColor = fadeImage.color;
            startColor.a = 0;
            Color endColor = startColor;
            endColor.a = 1;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime;
                fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime);
                yield return null;
            }
        }

        // Load next scene (Level1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}