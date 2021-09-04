using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    public static ArrowPool instance;

    public Weapon weapon;
    public List<GameObject> prefabList;
    public List<GameObject> arrowList;
    public int arrowCount;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        weapon = GameManager.instance.playerController.Com.weapon;

        prefabList.Add(Resources.Load("Prefabs/BasicArrow") as GameObject);

        initialize();
    }

    public void initialize()
    {
        if(weapon.weaponState == Weapon.State.Basic)
        {
            for(int i=0; i< arrowCount; i++)
            {
                var item = Instantiate(prefabList[0], transform);
                item.SetActive(false);
                arrowList.Add(item);
            }
        }
    }

    public GameObject SpawnArrow(Vector3 position, Quaternion rotation)
    {
        if(arrowList.Count > 0)
        {
            var arrow = arrowList[0];
            arrow.SetActive(true);
            arrow.transform.SetPositionAndRotation(position, rotation);

            arrowList.Remove(arrowList[0]);

            return arrow;
        }
        else
        {
            var item = Instantiate(prefabList[0], transform);
            item.SetActive(false);
            arrowList.Add(item);

            arrowList[0].SetActive(true);
            arrowList[0].transform.SetPositionAndRotation(position, rotation);

            return arrowList[0];
        }
    }



    public void Despawn(GameObject arrow)
    {
        arrow.SetActive(false);
        arrow.transform.position = transform.position;

        arrowList.Add(arrow);
    }
}
