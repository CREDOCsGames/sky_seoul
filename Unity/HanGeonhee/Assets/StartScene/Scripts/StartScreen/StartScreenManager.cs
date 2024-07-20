using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Image fade;

    private Color color;

    private void Start()
    {
        color = fade.color;

        StartCoroutine(ScreenLoad());
    }

    private IEnumerator ScreenLoad()
    {
        float timer = 1f;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            color.a = timer;

            fade.color = color;

            yield return null;
        }

        yield return YieldCache.WaitForSeconds(1f);

        fade.gameObject.SetActive(false);

        animator.Play("Start");
    }

    public void StartButton()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        animator.SetTrigger("End");

        yield return YieldCache.WaitForSeconds(0.5f);

        AsyncOperation operation = SceneManager.LoadSceneAsync("CharacterSelect");

        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            yield return null;
        }

        operation.allowSceneActivation = true;
    }

    public void SettingButton()
    {
        SettingManager.Instance.OpenOption();
    }

    public void QuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
