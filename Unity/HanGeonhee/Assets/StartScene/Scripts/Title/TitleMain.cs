using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMain : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleText;

    private Color color;

    private float timer;

    private void Start()
    {
        timer = 0f;

        color = new Color();

        color = titleText.color;

        StartCoroutine(TitleApear());
    }

    private void Update()
    {
        if(Input.anyKey)
        {
            timer += Time.deltaTime * 10;
        }
    }

    private IEnumerator TitleApear()
    {
        
        while (timer <= 2f)
        {
            timer += Time.deltaTime;

            color.a = timer / 2;

            titleText.color = color;

            yield return null;
        }

        StartCoroutine(TitleWait());
    }

    private IEnumerator TitleWait()
    {

        timer = 0f;

        while (timer <= 3f)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        StartCoroutine(TitleDisApear());
    }

    private IEnumerator TitleDisApear()
    {
        timer = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync("StartScreen");

        operation.allowSceneActivation = false;

        while (timer <= 2f)
        {
            timer += Time.deltaTime;

            color.a = (2 - timer) / 2;

            titleText.color = color;

            yield return null;
        }

        color.a = 0f;

        titleText.color = color;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }
}
