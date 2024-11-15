using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombItem : ItemBase
{
    [SerializeField]
    private Explosion explosionPrefab_;
    // 取得して爆発状態かどうかを判断する
    bool isGet = false;
    // 爆発し続ける時間
    private float explosionEmmitionTimer_ = 3;
    // 細かな爆発を清セする間隔
    private float explosionInterval_ = 0.2f;
    private float explosionTimer_ = 0.0f;

    public override void Get()
    {
        // Rendererを取得し、無効化する
        Renderer renderer_;
        if (TryGetComponent(out renderer_))
        {
            renderer_.enabled = false;
        }

        // ColliderはItemBaseで取得済みなので、無効にするだけで済む
        collider_.enabled = false;

        // 子オブジェクトが存在するかを確認してから無効化する
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("子オブジェクトが存在しません。無効化処理をスキップします。");
        }

        Debug.Log("弾と接触しました！");

        // 取得されたことを記録する
        isGet = true;
    }
    // 左移動だけでない処理をUpdateで行うためoverrideする。
    protected override void Update()
    {
        // 取得されていなければ通常のUpdate、つまり基底クラスのUpdateを呼ぶ
        if(!isGet)
        {
            base.Update();
            return;
        }
        explosionTimer_ -= Time.deltaTime;
        if(explosionEmmitionTimer_ <= 0) { Destroy(gameObject); }
        UpdateClusterExplosion();
    }

    private void UpdateClusterExplosion()
    {
        // Cluster爆発のタイマーを減らし、まだであれば早期リターン
        explosionEmmitionTimer_ -= Time.deltaTime;
        if (explosionTimer_ > 0) { return; }

        // `explosionPrefab_` が破棄されていないかを確認する
        if (explosionPrefab_ != null)
        {
            // 爆発範囲を決めてランダムでoffsetを決める
            float randomWidth = 2;
            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-randomWidth, randomWidth),
                UnityEngine.Random.Range(-randomWidth, randomWidth),
                0
            );

            // 自分の位置+offsetの位置に爆発を生成。タイマーにインターバル加算
            Instantiate(
                explosionPrefab_,
                transform.position + offset,
                Quaternion.identity
            );

            explosionTimer_ += explosionInterval_;
        }
        else
        {
            Debug.LogWarning("explosionPrefab_ が破棄されているため、爆発を生成できません。");
        }
    }


    // 小さな爆発を起こす
    //private void FixedUpdate()
    //{
    //    
    //}
}
