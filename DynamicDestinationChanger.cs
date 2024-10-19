using UnityEngine;
using System.Collections.Generic;

public class DynamicDestinationChanger : MonoBehaviour
{
    // ������ �� ������ � ����������� CarAI
    public CarAI carAI;

    // ��� ��� ������ ������ �������� (��������, "Waypoint")
    public string targetTag = "Waypoint";

    // ������� ���������� � ��������
    public float updateFrequency = 2.0f;

    // ������ ���������� ����������
    private List<Transform> passedWaypoints = new List<Transform>();

    void Start()
    {
        // ��������� ���������� ���� ������ ��������� ������
        InvokeRepeating("UpdateDestination", 0f, updateFrequency);
    }

    void UpdateDestination()
    {
        // ������� ��� ������� � �������� �����
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag(targetTag);

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("�� ������� �������� � �����: " + targetTag);
            return;
        }

        // ���� ��������� ������, ������� ��� �� ��� �������
        Transform nearestWaypoint = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = carAI.transform.position;

        foreach (GameObject waypoint in waypoints)
        {
            Transform waypointTransform = waypoint.transform;

            // ���������� ���������, ������� ��� ���� ��������
            if (passedWaypoints.Contains(waypointTransform))
            {
                continue;
            }

            float distance = Vector3.Distance(currentPosition, waypointTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestWaypoint = waypointTransform;
            }
        }

        // ���� ������ ��������� ��������, ������������� ��� ��� ����� ����
        if (nearestWaypoint != null)
        {
            carAI.CustomDestination = nearestWaypoint;
            Debug.Log("����� ���� �����������: " + nearestWaypoint.name);

            // ��������� ������� �������� � ������ ����������
            passedWaypoints.Add(nearestWaypoint);
        }
        else
        {
            Debug.LogWarning("�� ������� ����� ��������� ��������.");
        }
    }
}
