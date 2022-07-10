using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public Transform TapToStartObject;

    void Start()
    {
        StartCoroutine(AnimateText(0.8f));
    }

    public void PlayGame()
    {
        SoundManager.Instance.PlayAudio(SoundManager.Instance.LEVELUP);
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator AnimateText(float time)
    {
        float timeElapsed = 0f;
        while (true)
        {
            timeElapsed = 0f;
            while (timeElapsed < time)
            {
                TapToStartObject.localScale = Vector3.Lerp(Vector3.one, Vector3.one * 1.2f, timeElapsed / time);
                timeElapsed += Time.deltaTime;

                yield return null;
            }

            timeElapsed = 0f;
            while (timeElapsed < time)
            {
                TapToStartObject.localScale = Vector3.Lerp(Vector3.one * 1.2f, Vector3.one, timeElapsed / time);
                timeElapsed += Time.deltaTime;

                yield return null;
            }
        }

    }
}
