using UnityEngine;
using System.Collections;

public class MysteryBox : MonoBehaviour
{
    public GameObject mushroomPrefab;
    public GameObject coinPrefab;

    private bool opened = false;

    public void Hit()
    {

        Debug.Log(gameObject.name + " Hit");

        if (opened)
            return;

        opened = true;

        if (CompareTag("mushroom_box"))
        {
            Debug.Log("버섯 박스!");

            if (mushroomPrefab != null)
            {
                Instantiate(
                    mushroomPrefab,
                    transform.position + Vector3.up * 0.5f,
                    Quaternion.identity);
            }
        }
        else if (CompareTag("coin_box"))
        {
            Debug.Log("코인 박스!");

            if (coinPrefab != null)
            {
                Instantiate(
                    coinPrefab,
                    transform.position + Vector3.up * 0.8f,
                    Quaternion.identity);
            }
        }

        // 태그 변경
        gameObject.tag = "opened_box";

        // 박스 살짝 튀기
        StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        Vector3 start = transform.position;
        Vector3 up = start + Vector3.up * 0.15f;

        float t = 0f;

        while (t < 0.08f)
        {
            transform.position = Vector3.Lerp(start, up, t / 0.08f);
            t += Time.deltaTime;
            yield return null;
        }

        t = 0f;

        while (t < 0.08f)
        {
            transform.position = Vector3.Lerp(up, start, t / 0.08f);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = start;
    }
}