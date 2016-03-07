using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public List<WaveInterval> Sequence = new List<WaveInterval>();

    private float TotalLifeTime = 0.0f;
    public float CurrentWaveIntervalLife = 0.0f;
    public GameObject DeathPrefab;
    private WaveInterval CurrentWaveInterval;
    private int CurrentWaveIntervalIndex = 0;

    public float Health = 4.0f;

	// Use this for initialization
	void Start ()
    {
        if (Sequence.Count > 0)
        {
            CurrentWaveInterval = Sequence[0];

            while (CurrentWaveIntervalLife > CurrentWaveInterval.Time)
            {
                NextWaveInterval();
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        TotalLifeTime += Time.deltaTime;

        if (Sequence.Count == 0)
        {
            return;
        }
        CurrentWaveIntervalLife += Time.deltaTime;
        if (CurrentWaveIntervalLife > CurrentWaveInterval.Time)
        {
            NextWaveInterval();
        }

        if (CurrentWaveInterval.Time > 0.0f)
        {
            var t = CurrentWaveIntervalLife / CurrentWaveInterval.Time;
            if (CurrentWaveInterval.Type == "sweep")
            {
                t = SmoothStep(t);
                transform.position = Vector3.Lerp(CurrentWaveInterval.From, CurrentWaveInterval.To, t);
            }
            else if (CurrentWaveInterval.Type == "circle")
            {
                var center = (CurrentWaveInterval.From + CurrentWaveInterval.To) * 0.5f;
                var radius = (CurrentWaveInterval.From - CurrentWaveInterval.To).magnitude * 0.5f;

                var x = Mathf.Cos(t * Mathf.PI * 2.0f);
                var y = Mathf.Sin(t * Mathf.PI * 2.0f);
                transform.position = center + new Vector3(x * radius, 0, y * radius);
            }
        }
	}

    void NextWaveInterval()
    {
        if (Sequence.Count == 0)
        {
            return;
        }

        CurrentWaveIntervalLife -= CurrentWaveInterval.Time;
        if (++CurrentWaveIntervalIndex >= Sequence.Count)
        {
            CurrentWaveIntervalIndex = 0;
        }
        CurrentWaveInterval = Sequence[CurrentWaveIntervalIndex];
    }

    private static float SmoothStep(float t)
    {
        return t * t * (3 - 2 * t);
    }

    public void DealDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0.0f)
        {
            Instantiate(DeathPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}

[System.Serializable]
public struct WaveInterval
{
    public string Type;
    public float Time;
    public Vector3 From;
    public Vector3 To;
}
