using System;
using UnityEngine;

[RequireComponent(typeof(Move))]
public class GroundedCheck : MonoBehaviour
{
    [SerializeField] private Move move;

    private void Awake()
    {
        move = GetComponent<Move>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, Vector2.down * move.ExtendedGrCheck, Color.red);
    }
}
