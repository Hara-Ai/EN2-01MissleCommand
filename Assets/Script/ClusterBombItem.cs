using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClusterBombItem : ItemBase
{
    [SerializeField]
    private Explosion explosionPrefab_;
    // �擾���Ĕ�����Ԃ��ǂ����𔻒f����
    bool isGet = false;
    // �����������鎞��
    private float explosionEmmitionTimer_ = 3;
    // �ׂ��Ȕ����𐴃Z����Ԋu
    private float explosionInterval_ = 0.2f;
    private float explosionTimer_ = 0.0f;

    public override void Get()
    {
        // Renderer���擾���A����������
        Renderer renderer_;
        if (TryGetComponent(out renderer_))
        {
            renderer_.enabled = false;
        }

        // Collider��ItemBase�Ŏ擾�ς݂Ȃ̂ŁA�����ɂ��邾���ōς�
        collider_.enabled = false;

        // �q�I�u�W�F�N�g�����݂��邩���m�F���Ă��疳��������
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("�q�I�u�W�F�N�g�����݂��܂���B�������������X�L�b�v���܂��B");
        }

        Debug.Log("�e�ƐڐG���܂����I");

        // �擾���ꂽ���Ƃ��L�^����
        isGet = true;
    }
    // ���ړ������łȂ�������Update�ōs������override����B
    protected override void Update()
    {
        // �擾����Ă��Ȃ���Βʏ��Update�A�܂���N���X��Update���Ă�
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
        // Cluster�����̃^�C�}�[�����炵�A�܂��ł���Α������^�[��
        explosionEmmitionTimer_ -= Time.deltaTime;
        if (explosionTimer_ > 0) { return; }

        // `explosionPrefab_` ���j������Ă��Ȃ������m�F����
        if (explosionPrefab_ != null)
        {
            // �����͈͂����߂ă����_����offset�����߂�
            float randomWidth = 2;
            Vector3 offset = new Vector3(
                UnityEngine.Random.Range(-randomWidth, randomWidth),
                UnityEngine.Random.Range(-randomWidth, randomWidth),
                0
            );

            // �����̈ʒu+offset�̈ʒu�ɔ����𐶐��B�^�C�}�[�ɃC���^�[�o�����Z
            Instantiate(
                explosionPrefab_,
                transform.position + offset,
                Quaternion.identity
            );

            explosionTimer_ += explosionInterval_;
        }
        else
        {
            Debug.LogWarning("explosionPrefab_ ���j������Ă��邽�߁A�����𐶐��ł��܂���B");
        }
    }


    // �����Ȕ������N����
    //private void FixedUpdate()
    //{
    //    
    //}
}
