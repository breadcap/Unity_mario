using UnityEngine;
using System.Collections;
public class Coin : MonoBehaviour
{
    public int score = 100; // 코인 점수(선택)

    void Start()
    {

        // 스코어 ++

        // 게임매니저 호출, 스코어 올리기 ㄱㄱ
        StartCoroutine(destroy_after());
        
    }
    
    void Update()
    {

    }

    private IEnumerator destroy_after()
    {
 
        
        
        yield return new WaitForSeconds(0.5f); // 1초 대기

        Destroy(gameObject);
         
    }
}