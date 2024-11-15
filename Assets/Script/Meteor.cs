using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    /// <summary>
    /// �Œᗎ�����x
    /// </summary>
    [SerializeField] private float fallSpeedMin_ = 1;
    /// <summary>
    /// �ő嗎�����x
    /// </summary>
    [SerializeField] private float fallSpeedMax_ = 3;

    /// <summary>
    /// �����v���n�u�B����������󂯎��
    /// </summary>
    private Explosion explosionPrefab_;
    /// <summary>
    /// �n�ʂ̃R���C�U�[�B����������󂯎��
    /// </summary>
    private BoxCollider2D groundColloder_;
    private Rigidbody2D rb_;
    private GameManager gameManager_;

    // �X�R�A�G�t�F�N�g�v���n�u
    [SerializeField] ScoreEffect scoreEffectPrefab_;

    // Start is called before the first frame update
    private void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();
        SetupVlocity();
    }

    /// <summary>
    /// ����������K�v�ȏ��������p��
    /// </summary>
    public void Setup(BoxCollider2D ground,GameManager gameManager,Explosion explosionPrefab)
    {
        gameManager_ = gameManager;
        groundColloder_ = ground;
        explosionPrefab_ = explosionPrefab;
    }

    /// <summary>
    /// �ړ��ʂ̐ݒ�
    /// </summary>
    private void SetupVlocity()
    {
        // �n�ʂ̏㉺���E�̈ʒu���擾
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
          
            Debug.Log("���e�I�ƐڐG���܂����I");
               
            Explosion(explosion);
        }
    
        if(collision.gameObject.CompareTag("Ground"))
        {
            Fall();
        }
    }

    

    /// <summary>
    /// ���� 
    /// </summary>
    private void Explosion(Explosion otherExplosion)
    {
        // �A�����̎擾�Ɖ��Z
        int chainNum = otherExplosion.chainNum + 1;
        // �A�����ɉ������X�R�A���g�p
        int score = chainNum * 10;

        ScoreEffect scoreEffect = Instantiate(
           scoreEffectPrefab_,
           transform.position,
           Quaternion.identity
           );
        scoreEffect.SetScore(score);

        gameManager_.addScore(score); 
        // ��������Explosion�ɗp������
        Explosion explosion = Instantiate(
            explosionPrefab_,
            transform.position, 
            Quaternion.identity
            );
        
        explosion.chainNum = chainNum;

        // GameManager�ɃX�R�A�����Z��ʒm
        gameManager_.addScore(score);
        // �����𐶐���
        Instantiate(explosionPrefab_,transform.position, Quaternion.identity);
        // ���g�����ł�����
        Destroy(gameObject);
    }
    /// <summary>
    /// �n�ʂɗ���
    /// </summary>
    private void Fall()
    {
        // GameManeger�Ƀ_���[�W��ʒm
        gameManager_.damage(1);
        // ���g�͏���
        Destroy(gameObject);
    }
    
}
