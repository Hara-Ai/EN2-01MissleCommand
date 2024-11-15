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

    // �Փ˔���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion")) { GetType(); }

        Debug.Log($"�Փˌ��m: {collision.gameObject.name}");
        if (collision.CompareTag("Explosion"))
        {
            Debug.Log("Explosion�ƏՓ˂��܂����B");
            GetType();
        }

    }
}
