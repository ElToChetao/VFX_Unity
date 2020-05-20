using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonController : MonoBehaviour
{
    public Transform leftPivot;
    public Transform rightPivot;

    public Transform leftPivotNear;
    public Transform rightPivotNear;

    public float minTime;
    public float maxTime;

    public GameObject prefab;
    public GameObject whitePrefab;

    public Transform parent;
    void Start()
    {
        StartCoroutine(Spawn(leftPivot, leftPivotNear));
        StartCoroutine(Spawn(rightPivot, rightPivotNear));
        StartCoroutine(SpawnWhiteStuff());
    }

    private void Update()
    {
        Debug.DrawLine(leftPivot.position, leftPivotNear.position, Color.red);
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    IEnumerator SpawnWhiteStuff()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        int rand = Random.Range(0, 4);
        if(rand == 0)
        {
            Vector3 rot;
            rot.x = Random.Range(-10f, 10f);
            rot.y = 0;
            rot.z = Random.Range(-10f, 10f);
            GameObject go = ObjectPooler.Instance.SpawnWhite(parent.GetChild(0).position + Vector3.up * 3, Quaternion.Euler(rot));
            go.transform.up = rightPivot.transform.forward;
        }
        else if(rand == 1)
        {
            Vector3 rot;
            rot.x = Random.Range(-10f, 10f);
            rot.y = 0;
            rot.z = Random.Range(-10f, 10f);
            GameObject go = ObjectPooler.Instance.SpawnWhite(parent.GetChild(1).position + Vector3.up * 3, Quaternion.Euler(rot));
            go.transform.up = rightPivot.transform.forward;
        }
        else if (rand == 2)
        {
            Vector3 rot;
            rot.x = Random.Range(-10f, 10f);
            rot.y = 0;
            rot.z = Random.Range(-10f, 10f);
            GameObject right = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * 3, Quaternion.Euler(rot));
            right.transform.up = rightPivot.transform.forward;

            rot.x = Random.Range(-10f, 10f);
            rot.y = 0;
            rot.z = Random.Range(-10f, 10f);
            yield return new WaitForSeconds(0.2f);
            GameObject left = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * 3, Quaternion.Euler(rot));
            left.transform.up = leftPivot.transform.forward;
        }
        else if (rand == 3)
        {
            Transform first = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * 3, Quaternion.identity).transform;
            Vector3 rot;
            rot.x = Random.Range(-10f, 10f);
            rot.y = first.eulerAngles.y;
            rot.z = Random.Range(-10f, 10f);

            first.up = rightPivot.transform.forward;
            first.eulerAngles = rot;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            Transform second = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * Random.Range(2f, 2.5f), Quaternion.identity).transform;
            rot.x = Random.Range(-10f, 10f);
            rot.y = second.eulerAngles.y;
            rot.z = Random.Range(-10f, 10f);

            second.up = rightPivot.transform.forward;
            second.eulerAngles = rot;
        }
        StartCoroutine(SpawnWhiteStuff());
    }
    IEnumerator Spawn(Transform trans, Transform nearTrans)
    {
        float randAngle = Random.Range(1, 360) + Mathf.Deg2Rad;
        float radius = Random.Range(0f, 0.5f);
        Vector3 randPos;
        randPos.x = radius * Mathf.Cos(randAngle);
        randPos.y = 0;
        randPos.z = radius * Mathf.Sin(randAngle);

        randPos += trans.position;

        GameObject go = ObjectPooler.Instance.SpawnFromPool(randPos, Quaternion.identity);
        Vector3 direction = nearTrans.position - trans.position;

        go.transform.up = direction;

        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        StartCoroutine(Spawn(trans, nearTrans));
    }
}
