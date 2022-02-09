using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CProjectileTrajectory : MonoBehaviour
{
    LineRenderer lineRenderer;

    // Number of points on the line
    public int numPoints = 50;

    // distance between those points on the line

    // The physics layers that will cause the line to stop being drawn
    public LayerMask CollidableLayers;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public List<Vector3> points;
    public void Draw(Vector3 pForceVector, float pProjectileMass, Vector3 pOrigin)
    {
        Vector3 velocity = (pForceVector / pProjectileMass) * Time.fixedDeltaTime;
        float flightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = flightDuration / numPoints;

        points.Clear();

        for(int index = 0; index < numPoints; index++)
        {
            float stepTimePassed = stepTime * index;
            Vector3 movementVector = new Vector3(
                    velocity.x * stepTimePassed, 
                    velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                    velocity.z * stepTimePassed
            );

            points.Add(-movementVector + pOrigin);
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}