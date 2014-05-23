using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;

public class Paddler2 : MonoBehaviour
{
    public float InputMinX;
    public float InputMaxX;

    private Vector2 _target;
    private Vector2 _smoothVel;
    private Rigidbody2D _body;

    private float _cooldown;

	void Start () 
    {
	    _body = GetComponent<Rigidbody2D>();
        _target = _body.transform.position;
        _smoothVel = Vector2.zero;
        _cooldown = 0f;
	}

    void FixedUpdate()
    {
        Vector2 v = _target - Vec.xy(_body.transform.position);

        _body.velocity = (0.4f / Time.fixedDeltaTime) * v;

        _smoothVel += (_body.velocity - _smoothVel) * Time.fixedDeltaTime * 5.0f;

        _cooldown -= Time.fixedDeltaTime;
    }

	void Update () 
    {
        List<Vector2> points = new List<Vector2>();

        int tapCount = Input.touchCount;

        for (int i = 0; i < tapCount; ++i)
        {
            var t = Input.GetTouch(i);
            Vector2 wp = Vec.xy(Camera.main.ScreenToWorldPoint(t.position));

            if (wp.x >= InputMinX && wp.x <= InputMaxX)
                points.Add(wp);
        }

        if (tapCount == 0)
        {
            Vector3 mp = Input.mousePosition;
            Vector2 wp = Vec.xy(Camera.main.ScreenToWorldPoint(mp));

            if (wp.x >= InputMinX && wp.x <= InputMaxX)
                points.Add(wp);
        }

        if (points.Count > 0)
        {
            Vector2 avgPos = Vector2.zero;
            for (int i = 0; i < points.Count; ++i)
            {
                avgPos += points[i];
            }

            avgPos *= (1f / points.Count);
            _target = avgPos;
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (_smoothVel.magnitude > 0.1f && _cooldown < 0f)
        {
            Vector2 impulse;
             print(_smoothVel + "   :   " + other.rigidbody2D.velocity);
            //Vector2 toOther = Vec.xy(other.transform.position - transform.position);
            Vector2 toOther = _smoothVel;
            toOther.Normalize();

            float v1 = Vector2.Dot(toOther, other.rigidbody2D.velocity);
            float v2 = Vector2.Dot(toOther, _smoothVel);

            if(v1 - v2 < 0)
            {
                impulse = toOther;
                impulse *= -2.0f * (v1*0.0f - v2) * other.rigidbody2D.mass / Time.fixedDeltaTime;
                impulse -= other.rigidbody2D.velocity;

                other.rigidbody2D.AddForce(impulse);

               /* Vector2 point;
                float r = other.GetComponent<CircleCollider2D>().radius;

                //print(r);
                Vector2 vec = Vec.xy(other.transform.position - other.transform.position).normalized;
                point = Vec.xy(other.transform.position) + r * vec;
                other.rigidbody2D.AddForceAtPosition(impulse, point);*/

                _cooldown = 0.5f;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
    }
}
