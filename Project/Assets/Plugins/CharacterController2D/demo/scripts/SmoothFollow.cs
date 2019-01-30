using UnityEngine;
using System.Collections.Generic;
using Root.DesignPatterns;
using System.Linq;
using System;

public class SmoothFollow : SceneSingleton<SmoothFollow>
{
    public List<Transform> Targets
    {
        get; set;
    } = new List<Transform>();

    [Space(20)]

    [SerializeField]
    private Camera camera;

    [Space(20)]

    [SerializeField]
    private float smoothDampTime = 0.2f;

    [Space(20)]

    [SerializeField]
    private float orthographicScale = 1f;

    [SerializeField]
    private float orthographicSizeMin = 1;

    [SerializeField]
    private float orthographicSizeMax = 10;

    private Vector2 center;

    private Vector3 middlePoint;
    private float distanceFromMiddlePoint;
    private float distanceBetweenPlayers;

    private void FixedUpdate()
    {
        this.center = Vector2.zero;
        foreach (var target in this.Targets)
        {
            this.center += (Vector2)target.transform.position;
        }

        this.center = this.center / this.Targets.Count;

        float interpolation = this.smoothDampTime * Time.deltaTime;

        Vector3 position = this.transform.position;
        position.y = Mathf.Lerp(this.transform.position.y, this.center.y, interpolation);
        position.x = Mathf.Lerp(this.transform.position.x, this.center.x, interpolation);
        this.transform.position = position;

        var points = this.GetFarthestPoints();
        Vector3 position1 = points[0].position;
        Vector3 position2 = points[1].position;

        // Calculate the new orthographicSize
        this.middlePoint = position1 + 0.5f * (position2 - position1);

        this.distanceBetweenPlayers = (position2 - position1).magnitude;
        this.distanceFromMiddlePoint = (this.camera.transform.position - this.middlePoint).magnitude;
        var size = (2.0f * Mathf.Rad2Deg * Mathf.Atan((0.5f * this.distanceBetweenPlayers) / (this.distanceFromMiddlePoint * Screen.width / Screen.height))) * this.orthographicScale;
        size = Mathf.Clamp(size, this.orthographicSizeMin, this.orthographicSizeMax);

        this.camera.orthographicSize = Mathf.Lerp(this.camera.orthographicSize, size, interpolation);
    }

    private List<Transform> GetFarthestPoints()
    {
        Transform position1 = null;
        Transform position2 = null;
        float distance = 0;

        for (int i = 0; i < this.Targets.Count; i++)
        {
            for (int j = 0; j < this.Targets.Count; j++)
            {
                if(i == j)
                {
                    continue;
                }

                var currentDistance = Vector3.Distance(this.Targets[i].position, this.Targets[j].position);
                
                if(currentDistance > distance)
                {
                    position1 = this.Targets[i];
                    position2 = this.Targets[j];
                    distance = currentDistance;
                }
            }
        }

        Debug.Log($"{position1.name} || {position2.name}");

        return new List<Transform>()
        {
            position1,
            position2
        };
    }
}
