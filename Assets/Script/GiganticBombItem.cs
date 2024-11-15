using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticBombItem : ItemBase
{
    // �唚���v���n�u��ݒ�
    [SerializeField]
    Explosion giganticExplosionPrefab_;

    public override void Get()
    {
        if (giganticExplosionPrefab_ == null)
        {
            Debug.Log("giganticExplosionPrefab_ ���ݒ肳��Ă��܂���I");
            return;
        }

        Instantiate(
            giganticExplosionPrefab_,
            transform.position,
            Quaternion.identity
        );
        Destroy(gameObject);
    }
}

