using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiganticBombItem : ItemBase
{
    // 大爆発プレハブを設定
    [SerializeField]
    Explosion giganticExplosionPrefab_;

    public override void Get()
    {
        if (giganticExplosionPrefab_ == null)
        {
            Debug.Log("giganticExplosionPrefab_ が設定されていません！");
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

