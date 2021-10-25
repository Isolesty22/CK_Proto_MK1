using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent((typeof(Rigidbody)))]
/// <summary>
/// 스테이지를 선택하는, 이동할 수 있는 무언가
/// </summary>
public class StageSelector : MonoBehaviour
{
    [System.Serializable]
    public class Components
    {
        public Rigidbody rigidBody;
        public Animator animator;
    }

    public Components Com => _components;

    [SerializeField]
    private Components _components;

}
