using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomPool<T> where T : MonoBehaviour
{
    private Transform myTransform;

    private GameObject poolObject;

    private Queue<T> objectQueue;
    private int count;


    /// <summary>
    /// Pool을 사용하기 전, 초기화합니다.
    /// </summary>
    /// <param name="_prefab">Clone될 오브젝트.</param>
    /// <param name="_count">초기 생성할 오브젝트 개수.</param>
    /// <param name="_componentName">큐에 들어갈 타입</param>
    public void Init(GameObject _prefab, int _count, Transform _myTransform)
    {
        poolObject = _prefab;
        count = _count;
        objectQueue = new Queue<T>();
        myTransform = _myTransform;
    }

    /// <summary>
    /// Queue 초기화
    /// </summary>
    public void Init_Queue()
    {
        for (int i = 0; i < count; i++)
        {
            //오브젝트 생성
            GameObject tempGameObject = GameObject.Instantiate(
                            poolObject, Vector3.zero,
                            Quaternion.identity, myTransform);

            tempGameObject.SetActive(false);

            //큐에 넣기
            objectQueue.Enqueue(tempGameObject.GetComponent<T>());
        }
    }

    /// <summary>
    /// 오브젝트를 하나 만들고 반환합니다.
    /// </summary>
    public T CreatePoolObject()
    {
        lock (objectQueue)
        {
            //오브젝트 생성
            GameObject tempGameObject = GameObject.Instantiate(
                        poolObject, Vector3.zero,
                        Quaternion.identity, myTransform);

            tempGameObject.SetActive(false);

            //큐에 넣기
            objectQueue.Enqueue(tempGameObject.GetComponent<T>());

            return objectQueue.Peek();
        }
    }

    /// <summary>
    /// 오브젝트를 스폰합니다.
    /// </summary>
    /// <param name="_position">스폰할 오브젝트의 위치.</param>
    /// <param name="_rotation">스폰할 오브젝트의 회전값.</param>
    /// <param name="_parent">스폰할 오브젝트의 부모 오브젝트. 부모가 필요없다면 null을 넣습니다.</param>
    public T SpawnThis(Vector3 _position, Vector3 _rotation, Transform _parent)
    {
        T tempObject = default(T);
        Transform tempTransform = null;
        lock (objectQueue)
        {
            if (objectQueue.Count == 0)
            {
                Debug.LogWarning("[ " + myTransform.name + " ] 풀이 텅텅 비어있습니다. 오브젝트 하나를 새로 만듭니다.");
                CreatePoolObject();
            }

            tempObject = objectQueue.Dequeue();
            tempTransform = tempObject.transform;

            //빼주기
            tempTransform.SetParent(null);

            //위치, 회전값 설정
            tempTransform.SetPositionAndRotation(_position, Quaternion.Euler(_rotation));
            //tempTransform.rotation = Quaternion.Euler(_rotation);

            //부모 설정
            tempTransform.SetParent(_parent);

            tempObject.gameObject.SetActive(true);
            return tempObject;

        }
    }

    /// <summary>
    /// 오브젝트를 반환합니다.
    /// </summary>
    /// <param name="_object">반환할 오브젝트.</param>
    public void ReleaseThis(T _object)
    {
        _object.gameObject.SetActive(false);
        _object.gameObject.transform.SetParent(myTransform);
        objectQueue.Enqueue(_object);
    }
}

