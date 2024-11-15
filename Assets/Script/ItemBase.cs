using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
abstract public class ItemBase : MonoBehaviour
{
    // 移動速度。派生クラスでも使えるようにprotected
    [SerializeField]
    protected float speed_ = 3;
    // 画面サイズ確認用。派生クラスでも使えるようにprotected
    protected Camera camera_;
    // 自身のSize確認用。派生クラスでも使えるようにprotected
    protected Collider2D collider_;
    // 初期化
    private void Awake()
    {
        camera_ = Camera.main;
        collider_ = GetComponent<Collider2D>();
    }

    // 更新処理。派生クラスで買い替えられるようにvirtualを付ける
    protected virtual void Update() 
    {
        transform.Translate(Vector3.right * speed_ * Time.deltaTime);

        float worldScreenRight =
            camera_.orthographicSize * camera_.aspect;

        float boundsSize = collider_.bounds.size.x;

        if (transform.position.x > worldScreenRight + boundsSize)
        {
            Destroy(gameObject);
        }
    }
    // 衝突判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion")) { Get(); }

        Debug.Log($"衝突検知: {collision.gameObject.name}");
        if (collision.CompareTag("Explosion"))
        {
            Debug.Log("Explosionと衝突しました。");
            Get();
        }

    }

    public abstract void Get();
}
