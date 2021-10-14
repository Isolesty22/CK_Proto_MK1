using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ParryingObject : MonoBehaviour
{
    #region
    [Serializable]
    public class ObjectStatus
    {
        public bool isdisable;
        public bool isMove;
        public bool isDamaged;

        [Header("Sub Status")]
        public bool isUpDown;
        public float moveDistance;
        public float moveSpeed;
    }

    [Serializable]
    public class ObjectComponents
    {
        public Collider collider;
        public GameObject objectModel;
        public Renderer renderer;
    }

    [SerializeField] private ObjectStatus status = new ObjectStatus();
    [SerializeField] private ObjectComponents components = new ObjectComponents();

    public ObjectStatus Stat => status;
    public ObjectComponents Com => components;

    [HideInInspector] public bool isParried;

    private float runningTime = 0f;
    private float movePos = 0f;
    private Vector3 initialPos;
    #endregion

    private void Start()
    {
        initialPos = gameObject.transform.position;
    }

    private void Update()
    {
        if (Stat.isMove)
        {
            if (Stat.isUpDown)
            {
                runningTime += Time.deltaTime * Stat.moveSpeed;
                movePos = Mathf.Sin(runningTime) * Stat.moveDistance;
                this.transform.position = new Vector3(initialPos.x, initialPos.y + movePos, initialPos.z);
            }
            else
            {
                runningTime += Time.deltaTime * Stat.moveSpeed;
                movePos = Mathf.Sin(runningTime) * Stat.moveDistance;
                this.transform.position = new Vector3(initialPos.x + movePos, initialPos.y, initialPos.z);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameManager.instance.playerController.State.isParrying == true)
            {

            }
        }
    }
}
