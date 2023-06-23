using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveAbleArea 
{
    public bool CanMove { get; set; }
    public RaycastHit[] HitInfo { get; set; }
    public Ray Ray { get; set; }
    public Camera ActiveCamera { get; set; }
    public void MoveArea(GameObject denemeArea);
}
