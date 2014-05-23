using UnityEngine;
using System.Collections;
using Assets;
using Assets.Game;
using Assets.Rules;

public class AirhockeyGameBehaviour : MonoBehaviour
{
    public GameObject _ballPrefab;

    private Game _game;
    private GameObject _ballObj;

    public Transform Spawn1;
    public Transform Spawn2;

    private GUIText _roundScore1;
    private GUIText _roundScore2;

    const float NewRoundDelay = 1.4f;
    float _newRoundTimer;
    bool _newRound;

    void Start()
    {
        _game = new AirhockeyGame(3, 5);

        _roundScore1 = GameObject.Find("RoundScore1").GetComponent<GUIText>();
        _roundScore2 = GameObject.Find("RoundScore2").GetComponent<GUIText>();

        SetupGame();
    }

    void Update()
    {
        if (_newRound)
        {
            _newRoundTimer -= Time.deltaTime;
            if (_newRoundTimer < 0f)
            {
                OnNewRound();
                _newRound = false;
            }
        }
    }

    void SetupGame()
    {

        _game.RoundRules.OnWinner += OnRoundWinner;

        _game.SetRules.OnSetStart(PlayerId.Player1);
        _newRound = true;
        _newRoundTimer = NewRoundDelay;
    }

    public void OnRoundWinner(PlayerId prevWinner)
    {
        ((Ball)_ballObj.GetComponent<Ball>()).DestroySlowly(_game);

        _roundScore1.text = _game.SetRules.GetFormattedScore(PlayerId.Player1);
        _roundScore2.text = _game.SetRules.GetFormattedScore(PlayerId.Player2);
        _newRound = true;
        _newRoundTimer = NewRoundDelay;
    }

    public void OnNewRound()
    {
        Transform spawn = null;

        if (_game.SetRules.GetServingPlayer() == PlayerId.Player1)
            spawn = Spawn1;
        else
            spawn = Spawn2;

        _ballObj = (GameObject)Instantiate(_ballPrefab);
        _ballObj.name = "Ball";
        _ballObj.GetComponent<Ball>().Register(_game);
        _ballObj.rigidbody2D.velocity = new Vector2(Random.value - 0.5f, (Random.value - 0.5f) * 7f);
        _ballObj.rigidbody2D.angularVelocity = 3000f * (Random.value - 0.5f);
        _ballObj.transform.position = spawn.position;
        _ballObj.transform.rotation = spawn.rotation;

        Vector3 cv = new Vector3(Random.value, Random.value, Random.value);
        float l = cv.magnitude;
        if (l < 0.5f)
            cv *= 1f / l;
        if (cv.x > 1f)
            cv.x = 1f;
        if (cv.y > 1f)
            cv.y = 1f;
        if (cv.z > 1f)
            cv.z = 1f;

        Color comp = new Color((1f - cv.x) * 1.5f, (1f - cv.y) * 1.5f, (1f - cv.z) * 1.5f);
        _ballObj.GetComponent<SpriteRenderer>().color = comp;

        foreach (TrailRenderer t in _ballObj.gameObject.GetComponentsInChildren<TrailRenderer>())
        {
            Color c = new Color();
            c.r = cv.x;
            c.g = cv.y;
            c.b = cv.z;
            c.a = 0.5f;
            t.material.SetColor("_TintColor", c);
        }

        _game.RoundRules.OnStartRound(_game.SetRules.GetServingPlayer());

    }
}
