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
    private Vector3 GenerateRotation(Vector3 angles)
    {
        Vector3 rot;
        rot.x = Random.Range(-10f, 10f) + angles.x;
        rot.y = angles.y;
        rot.z = Random.Range(-10f, 10f) + angles.z;
        return rot;
    }
    IEnumerator SpawnWhiteStuff()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
            int rand = Random.Range(0, 4);
            if (rand == 0)
            {
                Transform go = ObjectPooler.Instance.SpawnWhite(parent.GetChild(0).position + Vector3.up * 3, Quaternion.identity).transform;
                go.up = rightPivot.transform.forward;
                go.eulerAngles = GenerateRotation(go.eulerAngles);
            }
            else if (rand == 1)
            {
                Transform go = ObjectPooler.Instance.SpawnWhite(parent.GetChild(1).position + Vector3.up * 3, Quaternion.identity).transform;
                go.up = rightPivot.transform.forward;
                go.eulerAngles = GenerateRotation(go.eulerAngles);
            }
            else if (rand == 2)
            {
                Transform right = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * 3, Quaternion.identity).transform;
                right.up = rightPivot.transform.forward;
                right.eulerAngles = GenerateRotation(right.eulerAngles);
                yield return new WaitForSeconds(0.2f);
                Transform left = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * 3, Quaternion.identity).transform;
                left.up = leftPivot.transform.forward;
                left.eulerAngles = GenerateRotation(left.eulerAngles);
            }
            else if (rand == 3)
            {
                Transform first = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * 3, Quaternion.identity).transform;
                first.up = rightPivot.transform.forward;
                first.eulerAngles = GenerateRotation(first.eulerAngles);

                yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
                Transform second = ObjectPooler.Instance.SpawnWhite(parent.position + Vector3.up * Random.Range(2f, 2.5f), Quaternion.identity).transform;
                second.up = rightPivot.transform.forward;
                second.eulerAngles = GenerateRotation(second.eulerAngles);
            }
        }
    }
    IEnumerator Spawn(Transform trans, Transform nearTrans)
    {
        while (true)
        {
            Transform go = ObjectPooler.Instance.SpawnFromPool(trans.position, Quaternion.identity).transform;
            Vector3 direction = nearTrans.position - trans.position;
            go.up = direction;

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
