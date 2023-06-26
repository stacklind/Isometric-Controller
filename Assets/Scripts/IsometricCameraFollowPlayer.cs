using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCameraFollowPlayer : MonoBehaviour
{
    [Tooltip("The transform you want the character to follow")]
    [SerializeField] private Transform target;

    public float DistanceToPlayer()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    private void LateUpdate()
    {
        transform.position = target.position;
    }
}
