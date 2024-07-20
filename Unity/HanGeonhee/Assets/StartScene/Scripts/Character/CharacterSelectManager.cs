using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] Transform mainCam;

    [SerializeField] List<Transform> moveLocate;

    [SerializeField] GameObject selectCanvas;

    [SerializeField] Animator animator;

    [SerializeField] GameObject wait;

    private float moveSpeed = 0.5f;

    private void Start()
    {
        StartCoroutine(MoveCamera());
    }

    private IEnumerator MoveCamera()
    {
        float timer = 0f;

        while(timer < moveSpeed)
        {
            timer += Time.deltaTime;
            mainCam.localPosition = Vector3.Lerp(mainCam.localPosition, moveLocate[0].localPosition, timer / moveSpeed);
            mainCam.localRotation = Quaternion.Lerp(mainCam.localRotation, moveLocate[0].localRotation, timer / moveSpeed);

            yield return null;
        }

        selectCanvas.SetActive(true);
    }

    public void CharacterSelect(int num)
    {
        if (!selectCanvas.activeSelf)
            return;

        wait.SetActive(true);

        StartCoroutine(Selected(num));
    }

    private IEnumerator Selected(int num)
    {
        float timer = 0f;

        while (timer < moveSpeed)
        {
            timer += Time.deltaTime;
            mainCam.localPosition = Vector3.Lerp(mainCam.localPosition, moveLocate[num].localPosition, timer / moveSpeed);

            yield return null;
        }

        animator.SetTrigger(num.ToString());

        yield return YieldCache.WaitForSeconds(0.5f);

        wait.SetActive(false);
    }

    public void Back()
    {
        wait.SetActive(true);

        StartCoroutine(BackToSelect());
    }

    private IEnumerator BackToSelect()
    {
        animator.SetTrigger("Back");

        yield return YieldCache.WaitForSeconds(0.5f);

        float timer = 0f;

        while (timer < moveSpeed)
        {
            timer += Time.deltaTime;
            mainCam.localPosition = Vector3.Lerp(mainCam.localPosition, moveLocate[0].localPosition, timer / moveSpeed);

            yield return null;
        }

        wait.SetActive(false);
    }

}
