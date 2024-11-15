using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    private float maxLifeTime_ = 1;
    private float time_ = 0;

    public int chainNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        time_ += maxLifeTime_;
    }

    // Update is called once per frame
    void Update()
    {
        time_ -= Time.deltaTime;
        if (time_ > 0) { return; }
        Destroy(gameObject);
    }

    // 衝突判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion")) { GetType(); }

        Debug.Log($"衝突検知: {collision.gameObject.name}");
        if (collision.CompareTag("Explosion"))
        {
            Debug.Log("Explosionと衝突しました。");
            GetType();
        }

    }
}
