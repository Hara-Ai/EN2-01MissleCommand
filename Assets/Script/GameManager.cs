using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{

    private Camera mainCamnera_;

    // �v���n�u�̐ݒ�
    [SerializeField, Header("Prefabs")]
    // �����̃v���n�u
    private Explosion explosionPrefab_;
    [SerializeField]
    // ���e�B�N���̃v���n�u
    private GameObject reticlePrefab_;
    [SerializeField]
    // �~�T�C���̃v���n�u
    private Missile missilePrefab_;
    // 覐΂̃v���n�u
    [SerializeField]
    private Meteor meteorPrefab_;


    // 覐΂̐����֌W
    [SerializeField, Header("MeteorSpawner")]


    // Start is called before the first frame update

    

    // 覐΂��Ԃ���n��
    private BoxCollider2D ground_;
    // 覐΂̐����̎��ԊԊu
    [SerializeField]
    private float meteorInterval_ = 1;
    // 覐΂̐����܂ł̎���
    private float meteorTimer_;
    // 覐΂̐����ʒu
    [SerializeField]
    private List<Transform> spawnPositions_;

    /// <summary>
    /// 覐΃^�C�}�̍X�V
    /// </summary>
    private void UpdateMeteorTimer() 
    {
        meteorTimer_ -= Time.deltaTime;
        if(meteorTimer_ > 0) {return;}
        meteorTimer_ += meteorInterval_;
        GenerateMeteor();
    }

    /// <summary>
    /// 覐΂̐���
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

        Assert.IsNotNull(mainCameraObject,"MainCamera��������܂���ł���");

        Assert.IsTrue(mainCameraObject.TryGetComponent(out mainCamnera_), "MainCamera��Camera�R���|�[�l���g������܂���");

        // �����ʒuList�̗v�f����1�ȏ�ł��邱�Ƃ��m�F
        Assert.IsTrue(spawnPositions_.Count > 0, "spawnPoition_�ɗv�f���������܂���B");
        foreach (Transform t in spawnPositions_)
        {
            // �����v�f��Null���܂܂�Ă��Ȃ������m�F
            Assert.IsNotNull(
                t,
                "spawnPositions_��Null���܂܂�܂�"
                );
        }
        // �̗͂̏�����
        ResetLife();
    }

    // Update is called once per frame
    void Update()
    {
        // �N���b�N�����甚���𐶐�
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMissile(); 
        }
        UpdateMeteorTimer();
        UpdateItemTimer();

    }

    /// <summary>
    /// �~�T�C���̐���(�ǉ�)
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

    // �X�R�A�֌W
    [SerializeField, Header("ScoreUISettings")]
    //�X�R�A�\���p
    //�e�L�X�g
    private ScoreText scoreText_;
    // �X�R�A
    private int score_;

    /// <summary>
    /// �X�R�A�̉��Z
    /// </summary>
    /// <param name="point">10</param>
    public void addScore(int point) 
    {
        score_ += point;
        scoreText_.SetScore(score_);
    }

    // ���C�t�֌W
    [SerializeField, Header("LifeUISettings")]
    // ���C�t�Q�[�W
    private LifeBar lifeBar_;
    // �ő�̗�
    [SerializeField]
    private float maxLife_ = 10;
    // ���ݑ̗�
    private float life_;

    /// <summary>
    /// ���C�t�̏�����
    /// </summary>
    private void ResetLife()
    {
        life_ = maxLife_;
        // UI�̍X�V
        UpdateLifeBar();
    }

    /// <summary>
    /// ���C�tUI�̍X�V
    /// </summary>
    private void UpdateLifeBar() 
    {
        // �ő�̗͂ƌ��ݑ̗͂̊��肠���ŉ��������Z�o
        float lifeRatio = Mathf.Clamp01((life_ / maxLife_));
        // ������lifeBar_�֓`���AUI�ɔ��f
        lifeBar_.SetGaugeRatio(lifeRatio);
    }

    /// <summary>
    /// ���C�t�����炷
    /// </summary>
    /// <param name="damage">���炷�l</param>
    public void damage(int point)
    {
        life_ -= point;
        // UI�̍X�V
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
        // �^�C�}�[�����炵�A�܂��c���Ă����瑁�����^�[��
        itemTimer_ -= Time.deltaTime;
        if (itemTimer_ > 0) { return; }

        itemTimer_ += itemSpawnIntereval_;
        // �^�C�}�[�����Z�����A�܂��c���Ă����瑁�����^�[��
        ItemBase pickedUpItem = PickupItem();
        // itemSpawnPrefabs_ �� null ���ǂ����`�F�b�N
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
            Debug.LogWarning("itemSpawnPrefabs_ ���j������Ă��܂��B�A�C�e���̐������s���܂���ł����B");
        }

    }
}
