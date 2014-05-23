using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets;

public class Paddler : MonoBehaviour
{
    Vector2 _target;
    Vector2 _targetDir;
    Rigidbody2D _body;

    public float InputMinX;
    public float InputMaxX;

	void Start () 
    {
        _body = GetComponent<Rigidbody2D>();
        _target = _body.transform.position;

        float a = _body.transform.rotation.eulerAngles.z * Mathf.PI / 180f + Mathf.PI * 0.5f;
        _targetDir = -new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        _targetDir.Normalize();

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Paddler"),
                                    LayerMask.NameToLayer("Platform"));
	}

    void FixedUpdate()
    {
        /*float vel = _body.velocity.magnitude;
        if (vel > 40.0f)
            _body.velocity *= 40.0f / vel;*/

        Vector2 f = _body.transform.position;
        Vector2 t = _target - 5f * Time.deltaTime * _body.velocity;
        f = t - f;
        float l = f.magnitude+0.000001f;
        l = Mathf.Sqrt(l) / l;
        if (l > 15.0f)
            l = 15.0f;

        f *=  l;

        _body.AddForce(2000f * f);

        Vector2 rot = new Vector2(Mathf.Cos(_body.transform.rotation.eulerAngles.z * Mathf.PI / 180f), Mathf.Sin(_body.transform.rotation.eulerAngles.z * Mathf.PI / 180f));
        _body.AddTorque(850.0f * Vector2.Dot(_targetDir, rot));
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

        if (tapCount == 0 && Input.GetMouseButton(0))
        {
            Vector3 mp = Input.mousePosition;
            Vector2 wp = Vec.xy(Camera.main.ScreenToWorldPoint(mp));

            if (wp.x >= InputMinX && wp.x <= InputMaxX)
                points.Add(wp);


        }
        if (tapCount == 0 && Input.GetMouseButton(1))
        {
            Vector3 mp = Input.mousePosition;
            Vector2 wp = Vec.xy(Camera.main.ScreenToWorldPoint(mp));

            if (wp.x >= InputMinX && wp.x <= InputMaxX)
            {
                _targetDir = _target - wp;
                _targetDir.Normalize();
            }
        }

        if (points.Count >= 2)
        {
            var t1 = points[0];
            var t2 = points[1];

            Vector2 d = Vec.PerpendicularCCW(t1 - t2);
            d.Normalize();

            _targetDir = d;
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
}
