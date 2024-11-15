using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
abstract public class ItemBase : MonoBehaviour
{
    // �ړ����x�B�h���N���X�ł��g����悤��protected
    [SerializeField]
    protected float speed_ = 3;
    // ��ʃT�C�Y�m�F�p�B�h���N���X�ł��g����悤��protected
    protected Camera camera_;
    // ���g��Size�m�F�p�B�h���N���X�ł��g����悤��protected
    protected Collider2D collider_;
    // ������
    private void Awake()
    {
        camera_ = Camera.main;
        collider_ = GetComponent<Collider2D>();
    }

    // �X�V�����B�h���N���X�Ŕ����ւ�����悤��virtual��t����
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
    // �Փ˔���
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Explosion")) { Get(); }

        Debug.Log($"�Փˌ��m: {collision.gameObject.name}");
        if (collision.CompareTag("Explosion"))
        {
            Debug.Log("Explosion�ƏՓ˂��܂����B");
            Get();
        }

    }

    public abstract void Get();
}
