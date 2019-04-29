using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Explosion Container",menuName = "Explosion Container")]
public class ExplosionsContainer : ScriptableObject
{
    public List<GameObject> Particle_List = new List<GameObject>();
}
