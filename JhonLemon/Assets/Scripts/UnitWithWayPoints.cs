using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitWithWayPoints : MonoBehaviour
{
    public List<Transform> targets;
    float speed = 1;
    Vector3[] path;
    int targetIndex;
    int currentTargetIndex = 0;
    float cubeSize = 0.2f;

    void Start()
    {
        SetDestination();
    }

    void SetDestination()
    {
        if (currentTargetIndex < targets.Count)
        {
            PathRequestManager.RequestPath(transform.position, targets[currentTargetIndex].position, OnPathFound);
        }
    }

    public void OnPathFound(Vector3[] newpath, bool pathSuccessful)
    {
        Debug.Log("pathSuccessful");
        if (pathSuccessful)
        {
            path = newpath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
        else
        {
            // Manejar la falla en la solicitud de camino (opcional)
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    targetIndex = 0; // Reiniciar al primer punto del camino
                    currentTargetIndex++;
                    if (currentTargetIndex >= targets.Count)
                    {
                        currentTargetIndex = 0; // Reiniciar al primer destino
                    }
                    SetDestination();
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);

            Vector3 direction = currentWaypoint - transform.position;
            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * 5f);
            }

            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one * cubeSize);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
