using UnityEngine;
using System.Collections.Generic;

public class DynamicDestinationChanger : MonoBehaviour
{
    // Ссылка на объект с компонентом CarAI
    public CarAI carAI;

    // Тег для поиска пустых объектов (например, "Waypoint")
    public string targetTag = "Waypoint";

    // Частота обновления в секундах
    public float updateFrequency = 2.0f;

    // Список пройденных вейпоинтов
    private List<Transform> passedWaypoints = new List<Transform>();

    void Start()
    {
        // Запускаем обновление цели каждые несколько секунд
        InvokeRepeating("UpdateDestination", 0f, updateFrequency);
    }

    void UpdateDestination()
    {
        // Находим все объекты с заданным тегом
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag(targetTag);

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("Не найдено объектов с тегом: " + targetTag);
            return;
        }

        // Ищем ближайший объект, который еще не был пройден
        Transform nearestWaypoint = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = carAI.transform.position;

        foreach (GameObject waypoint in waypoints)
        {
            Transform waypointTransform = waypoint.transform;

            // Пропускаем вейпоинты, которые уже были пройдены
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

        // Если найден ближайший вейпоинт, устанавливаем его как новую цель
        if (nearestWaypoint != null)
        {
            carAI.CustomDestination = nearestWaypoint;
            Debug.Log("Новая цель установлена: " + nearestWaypoint.name);

            // Добавляем текущий вейпоинт в список пройденных
            passedWaypoints.Add(nearestWaypoint);
        }
        else
        {
            Debug.LogWarning("Не удалось найти ближайший вейпоинт.");
        }
    }
}
