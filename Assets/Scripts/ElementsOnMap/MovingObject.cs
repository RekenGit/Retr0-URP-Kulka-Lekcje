using System.Collections;
using TMPro;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private float duration = 4;
    [SerializeField] private float waitTime = 1;
    [SerializeField] private Vector3 pos2;
    private Vector3 pos1;
    private bool isGoingToSecondPos = true;

    void Start()
    {
        pos1 = transform.position;
        StartCoroutine(MoveObject(pos1, pos2));
        InvokeRepeating("ChangeObjectDestination", 0, 1);
    }

    private void ChangeObjectDestination()
    {
        Vector3 targetPosition = isGoingToSecondPos ? pos2 : pos1;
        if (Vector3.Distance(transform.position, targetPosition) == 0)
        {
            StartCoroutine(MoveObject(targetPosition, isGoingToSecondPos ? pos1 : pos2));
            isGoingToSecondPos = !isGoingToSecondPos;
        }
    }

    private IEnumerator MoveObject(Vector3 from, Vector3 to)
    {
        yield return new WaitForSeconds(waitTime);
        float time = 0;
        float axisX, axisY, axisZ;

        while (time < duration)
        {
            time += Time.deltaTime;
            axisX = Mathf.Lerp(from.x, to.x, time / duration);
            axisY = Mathf.Lerp(from.y, to.y, time / duration);
            axisZ = Mathf.Lerp(from.z, to.z, time / duration);
            transform.position = new Vector3(axisX, axisY, axisZ);

            yield return null;
        }
    }
}
