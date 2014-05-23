using UnityEngine;
using System.Collections;
using Assets.Rules;
using Assets.Rules.Round;
using Assets.Game;

public class Ball : MonoBehaviour 
{
    Rigidbody2D _body;
    float[] _hitTimers;
    bool _isDying;
    float _deadTimer;
    float _startAlpha;

    public float DyingDelay = 0.8f;
    public float MaxSpeed = 20f;
    public float SpinFactor = 1f;
    public float MinDrag = 0.2f;
    public float MaxDrag = 0.5f;
    public float DragIncreaseDelay = 0.25f;
    public float HitSafeTime = 0.5f;

	void Start ()
    {
        _body = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(1, 2);
        _hitTimers = new float[2];
        _hitTimers[0] = 0;
        _hitTimers[1] = 0;
        _isDying = false;
        _deadTimer = 0.0f;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Paddler"),
                                    LayerMask.NameToLayer("Dying")
                                   );
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Dying"),
                                    LayerMask.NameToLayer("Dying")
                                   );
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"),
                                    LayerMask.NameToLayer("Dying")
                                   );
	}

    void FixedUpdate()
    {
        //Apply Magnus force (force in perpendicular direction from linear vel, based on angular vel)
        float a = SpinFactor * 0.002f * _body.angularVelocity;
        Vector2 perpVel;
        perpVel.x = -_body.velocity.y;
        perpVel.y = _body.velocity.x;

        _body.AddForce(a * perpVel);

        for (int i = 0; i < 2; ++i)
            _hitTimers[i] -= Time.fixedDeltaTime;

        //Unrealistic drag. Increases wit velocity to keep max vel down.
        float vel = _body.velocity.magnitude;
        float drag = (vel - DragIncreaseDelay * MaxSpeed) / MaxSpeed;
        drag = Mathf.Clamp(drag, MinDrag, MaxDrag);
        _body.drag = drag;

        if (vel > MaxSpeed)
            _body.velocity *= MaxSpeed / vel;

        if (_isDying)
        {
            //Fade out and destroy
            Color c = GetComponent<SpriteRenderer>().color;
            c.a = _deadTimer / DyingDelay;
            c.g = c.b = 0.0f;
            c.r = 0.7f;
            if (c.a < 0f)
                c.a = 0f;
            GetComponent<SpriteRenderer>().color = c;

            foreach (TrailRenderer t in gameObject.GetComponentsInChildren<TrailRenderer>())
            {
                  c = t.material.GetColor("_TintColor");
                  c.a = 0.5f * _deadTimer / DyingDelay;
                  if (c.a < 0f)
                      c.a = 0f;
                  t.material.SetColor("_TintColor", c);
            }
            _deadTimer -= Time.fixedDeltaTime;

            if (_deadTimer < -0.3f)
                GameObject.Destroy(gameObject);
        }
    }

    public void Register(Game game)
    {
        OnBounce += game.RoundRules.OnBallGroundBounce;
        OnHit += game.RoundRules.OnBallHit;
        OnOutside += game.RoundRules.OnBallOutside;
    }

    public void DestroySlowly(Game game)
    {
        OnBounce -= game.RoundRules.OnBallGroundBounce;
        OnHit -= game.RoundRules.OnBallHit;
        OnOutside -= game.RoundRules.OnBallOutside;

        name = "OldBall";
        _deadTimer = DyingDelay;
        _isDying = true;
        gameObject.layer = LayerMask.NameToLayer("Dying");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider.gameObject);
    }

    void HandleCollision(GameObject obj)
    {
        switch (obj.name)
        {
            case "Ground1":
                OnBounce(PlayerId.Player1);
                break;
            case "Ground2":
                OnBounce(PlayerId.Player2);
                break;
            case "Paddler1":
                if (_hitTimers[0] < 0f)
                {
                    _hitTimers[0] = HitSafeTime;
                    OnHit(PlayerId.Player1);
                }
                break;
            case "Paddler2":
                if (_hitTimers[1] < 0f)
                {
                    _hitTimers[1] = HitSafeTime;
                    OnHit(PlayerId.Player2);
                }
                break;
            case "OutsideLeft":
                OnOutside(Direction.Left);
                break;
            case "OutsideRight":
                OnOutside(Direction.Right);
                break;
            case "OutsideTop":
                OnOutside(Direction.Top);
                break;
            case "OutsideBottom":
                OnOutside(Direction.Bottom);
                break;
            default:
                OnBounce(PlayerId.Neutral);
                break;
        }

        if (obj.name != "Paddler1")
            _hitTimers[0] = -0.1f;
        if (obj.name != "Paddler2")
            _hitTimers[1] = -0.1f;
    }

    public delegate void OnBallAction(PlayerId player);
    public delegate void OnBallOutside(Direction dir);

    public event OnBallAction OnBounce = delegate { };
    public event OnBallAction OnHit = delegate { };
    public event OnBallOutside OnOutside = delegate { };
}
