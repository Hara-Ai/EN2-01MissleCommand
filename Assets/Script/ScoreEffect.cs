using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class ScoreEffect : MonoBehaviour
{
    public void SetScore(int  score)
    {
        GetComponent<TMP_Text>().text = score.ToString();
    }

    // 上昇速度
    [SerializeField]
    float upSpeed_ = 1;
    // 消滅までの時間(秒)
    [SerializeField]
    float aliveTime_ = 1;
    // カウンター
    float alivedTimer_ = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        alivedTimer_ += Time.deltaTime;
        if(alivedTimer_ >= aliveTime_) { Destroy(gameObject); }
        // 上方向へ上昇
        transform.Translate(Vector3.up * upSpeed_ * Time.deltaTime);
    }
}
