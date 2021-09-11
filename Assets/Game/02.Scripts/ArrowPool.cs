using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    public static ArrowPool instance;

    public Weapon weapon;
    public List<GameObject> prefabList;
    public List<GameObject> arrowList;
    public List<GameObject> counterList;
    public int arrowCount;
    public int counterCount;

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
        prefabList.Add(Resources.Load("Prefabs/Counter") as GameObject);

        initialize();
    }

    public void initialize()
    {
        if (weapon.weaponState == Weapon.State.Basic)
        {
            for(int i=0; i< arrowCount; i++)
            {
                var item = Instantiate(prefabList[0], transform);
                item.SetActive(false);
                arrowList.Add(item);
            }
        }

        for (int i = 0; i < counterCount; i++)
        {
            var item = Instantiate(prefabList[1], transform);
            item.SetActive(false);
            counterList.Add(item);
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

    public GameObject SpawnCounter(Vector3 position, Quaternion rotation)
    {
        if (counterList.Count > 0)
        {
            var counter = counterList[0];
            counter.SetActive(true);
            counter.transform.SetPositionAndRotation(position, rotation);

            counterList.Remove(counterList[0]);

            return counter;
        }
        else
        {
            var item = Instantiate(prefabList[1], transform);
            item.SetActive(false);
            counterList.Add(item);

            counterList[0].SetActive(true);
            counterList[0].transform.SetPositionAndRotation(position, rotation);

            return counterList[0];
        }
    }

    public void DespawnCounter(GameObject counter)
    {
        counter.SetActive(false);
        counter.transform.position = transform.position;

        counterList.Add(counter);
    }
}
