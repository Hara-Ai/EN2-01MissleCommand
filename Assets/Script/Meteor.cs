using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    /// <summary>
    /// 最低落下速度
    /// </summary>
    [SerializeField] private float fallSpeedMin_ = 1;
    /// <summary>
    /// 最大落下速度
    /// </summary>
    [SerializeField] private float fallSpeedMax_ = 3;

    /// <summary>
    /// 爆発プレハブ。生成元から受け取る
    /// </summary>
    private Explosion explosionPrefab_;
    /// <summary>
    /// 地面のコライザー。生成元から受け取る
    /// </summary>
    private BoxCollider2D groundColloder_;
    private Rigidbody2D rb_;
    private GameManager gameManager_;

    // スコアエフェクトプレハブ
    [SerializeField] ScoreEffect scoreEffectPrefab_;

    // Start is called before the first frame update
    private void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        SetupVlocity();
    }

    /// <summary>
    /// 生成元から必要な情報を引き継ぐ
    /// </summary>
    public void Setup(BoxCollider2D ground,GameManager gameManager,Explosion explosionPrefab)
    {
        gameManager_ = gameManager;
        groundColloder_ = ground;
        explosionPrefab_ = explosionPrefab;
    }

    /// <summary>
    /// 移動量の設定
    /// </summary>
    private void SetupVlocity()
    {
        // 地面の上下左右の位置を取得
        float left = groundColloder_.bounds.center.x - groundColloder_.bounds.size.x / 2;
        float right = groundColloder_.bounds.center.x + groundColloder_.bounds.size.x / 2;
        float top = groundColloder_.bounds.center.y + groundColloder_.bounds.size.y / 2;
        float bottom = groundColloder_.bounds.center.y - groundColloder_.bounds.size.y / 2;

        float targetX = Mathf.Lerp(left, right, Random.Range(0.0f, 1.0f));

        Vector3 target = new Vector3(targetX, top, 0);
        Vector3 direction = (target - transform.position).normalized;
        float fallSpeed = Random.Range(fallSpeedMin_, fallSpeedMax_);
        rb_.velocity = direction * fallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Explosion explosion;
        if (collision.gameObject.CompareTag("Explosion") && collision.TryGetComponent(out explosion))
        {
          
            Debug.Log("メテオと接触しました！");
               
            Explosion(explosion);
        }
    
        if(collision.gameObject.CompareTag("Ground"))
        {
            Fall();
        }
    }

    

    /// <summary>
    /// 爆発 
    /// </summary>
    private void Explosion(Explosion otherExplosion)
    {
        // 連鎖数の取得と加算
        int chainNum = otherExplosion.chainNum + 1;
        // 連鎖数に応じたスコアを使用
        int score = chainNum * 10;

        ScoreEffect scoreEffect = Instantiate(
           scoreEffectPrefab_,
           transform.position,
           Quaternion.identity
           );
        scoreEffect.SetScore(score);

        gameManager_.addScore(score); 
        // 生成したExplosionに用がある
        Explosion explosion = Instantiate(
            explosionPrefab_,
            transform.position, 
            Quaternion.identity
            );
        
        explosion.chainNum = chainNum;

        // GameManagerにスコアを加算を通知
        gameManager_.addScore(score);
        // 爆発を生成し
        Instantiate(explosionPrefab_,transform.position, Quaternion.identity);
        // 自身を消滅させる
        Destroy(gameObject);
    }
    /// <summary>
    /// 地面に落下
    /// </summary>
    private void Fall()
    {
        // GameManegerにダメージを通知
        gameManager_.damage(1);
        // 自身は消滅
        Destroy(gameObject);
    }
    
}
