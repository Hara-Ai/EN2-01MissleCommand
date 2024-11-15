using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{

    private Camera mainCamnera_;

    // プレハブの設定
    [SerializeField, Header("Prefabs")]
    // 爆発のプレハブ
    private Explosion explosionPrefab_;
    [SerializeField]
    // レティクルのプレハブ
    private GameObject reticlePrefab_;
    [SerializeField]
    // ミサイルのプレハブ
    private Missile missilePrefab_;
    // 隕石のプレハブ
    [SerializeField]
    private Meteor meteorPrefab_;


    // 隕石の生成関係
    [SerializeField, Header("MeteorSpawner")]


    // Start is called before the first frame update

    

    // 隕石がぶつかる地面
    private BoxCollider2D ground_;
    // 隕石の生成の時間間隔
    [SerializeField]
    private float meteorInterval_ = 1;
    // 隕石の生成までの時間
    private float meteorTimer_;
    // 隕石の生成位置
    [SerializeField]
    private List<Transform> spawnPositions_;

    /// <summary>
    /// 隕石タイマの更新
    /// </summary>
    private void UpdateMeteorTimer() 
    {
        meteorTimer_ -= Time.deltaTime;
        if(meteorTimer_ > 0) {return;}
        meteorTimer_ += meteorInterval_;
        GenerateMeteor();
    }

    /// <summary>
    /// 隕石の生成
    /// </summary>
    private void GenerateMeteor() 
    {
        int max = spawnPositions_.Count;
        int posIndex = Random.Range(0, max);
        Vector3 spawnPosition = spawnPositions_[posIndex].position;
        Meteor meteor = Instantiate(meteorPrefab_, spawnPosition, Quaternion.identity);
        meteor.Setup(ground_, this, explosionPrefab_);
    }

    private void Start()
    {
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");

        Assert.IsNotNull(mainCameraObject,"MainCameraが見つかりませんでした");

        Assert.IsTrue(mainCameraObject.TryGetComponent(out mainCamnera_), "MainCameraにCameraコンポーネントがありません");

        // 生成位置Listの要素数が1以上であることを確認
        Assert.IsTrue(spawnPositions_.Count > 0, "spawnPoition_に要素が一つもありません。");
        foreach (Transform t in spawnPositions_)
        {
            // 書く要素にNullが含まれていない事を確認
            Assert.IsNotNull(
                t,
                "spawnPositions_にNullが含まれます"
                );
        }
        // 体力の初期化
        ResetLife();
    }

    // Update is called once per frame
    void Update()
    {
        // クリックしたら爆発を生成
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMissile(); 
        }
        UpdateMeteorTimer();
        UpdateItemTimer();

    }

    /// <summary>
    /// ミサイルの生成(追加)
    /// </summary>
    private void GenerateMissile()
    {
         
        Vector3 clickPosition = 
            mainCamnera_.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0;
        GameObject reticle = Instantiate(
            reticlePrefab_, clickPosition, Quaternion.identity);
        
        Vector3 launchPosiion = new Vector3(0, -3, 0);
        Missile missile = Instantiate(
            missilePrefab_, launchPosiion, Quaternion.identity);
        missile.Setup(reticle);
    }

    // スコア関係
    [SerializeField, Header("ScoreUISettings")]
    //スコア表示用
    //テキスト
    private ScoreText scoreText_;
    // スコア
    private int score_;

    /// <summary>
    /// スコアの加算
    /// </summary>
    /// <param name="point">10</param>
    public void addScore(int point) 
    {
        score_ += point;
        scoreText_.SetScore(score_);
    }

    // ライフ関係
    [SerializeField, Header("LifeUISettings")]
    // ライフゲージ
    private LifeBar lifeBar_;
    // 最大体力
    [SerializeField]
    private float maxLife_ = 10;
    // 現在体力
    private float life_;

    /// <summary>
    /// ライフの初期化
    /// </summary>
    private void ResetLife()
    {
        life_ = maxLife_;
        // UIの更新
        UpdateLifeBar();
    }

    /// <summary>
    /// ライフUIの更新
    /// </summary>
    private void UpdateLifeBar() 
    {
        // 最大体力と現在体力の割りあうで何割かを算出
        float lifeRatio = Mathf.Clamp01((life_ / maxLife_));
        // 割合をlifeBar_へ伝え、UIに反映
        lifeBar_.SetGaugeRatio(lifeRatio);
    }

    /// <summary>
    /// ライフを減らす
    /// </summary>
    /// <param name="damage">減らす値</param>
    public void damage(int point)
    {
        life_ -= point;
        // UIの更新
        UpdateLifeBar();
    }

    [SerializeField, Header("Prefabs")]
    private Transform itemSpawnPrefabs_;

    [SerializeField]
    List<ItemBase> items_;

    [SerializeField, Header("ItemSettings")]
    private Transform itemSpawnPoint_;

    [SerializeField]
    private float itemSpawnIntereval_ = 10;
    private float itemTimer_ = 0;

    private ItemBase PickupItem()
    {
        int itemPrefabNum = items_.Count;
        Assert.IsTrue(itemPrefabNum > 0);

        int pickedupIndex = Random.Range(0, itemPrefabNum);
        ItemBase pickedupItem = items_[pickedupIndex];
        return pickedupItem;
    }

    private void UpdateItemTimer()
    {
        // タイマーを減らし、まだ残っていたら早期リターン
        itemTimer_ -= Time.deltaTime;
        if (itemTimer_ > 0) { return; }

        itemTimer_ += itemSpawnIntereval_;
        // タイマーを減算させ、まだ残っていたら早期リターン
        ItemBase pickedUpItem = PickupItem();
        // itemSpawnPrefabs_ が null かどうかチェック
        if (itemSpawnPrefabs_ != null)
        {
            Instantiate(
                pickedUpItem,
                itemSpawnPrefabs_.position,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning("itemSpawnPrefabs_ が破棄されています。アイテムの生成が行われませんでした。");
        }

    }
}
