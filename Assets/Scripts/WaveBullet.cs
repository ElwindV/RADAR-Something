using UnityEngine;

public class WaveBullet : Bullet
{
    public override void Init(Vector3 position, Vector3 direction)
    {
        base.Init(position, direction);

        var direction2d = new Vector2(direction.x, direction.z);

        var y = (direction2d.x > 0)
            ? Vector2.Angle(Vector2.up, direction2d)
            : 180 + Vector2.Angle(Vector2.down, direction2d);

        transform.rotation = Quaternion.Euler(0, y, 0);
    }
    public override void OnCollisionEnter(Collision col)
    {
    }
}
