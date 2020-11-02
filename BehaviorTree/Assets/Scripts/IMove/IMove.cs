using UnityEngine;

public interface IMove
{

    void Move(ref float speed, ref Vector3 position, ref float distance);

}