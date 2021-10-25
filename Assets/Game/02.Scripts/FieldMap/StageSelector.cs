using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent((typeof(Rigidbody)))]
/// <summary>
/// ���������� �����ϴ�, �̵��� �� �ִ� ����
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
