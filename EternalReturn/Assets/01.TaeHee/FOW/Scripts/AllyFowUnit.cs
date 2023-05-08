using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyFowUnit : FowUnit
{
    private const float DEFAULT_SIGHT_RANGE = 8f;
    private const float DEFAULT_DAY_NIGHT_COEF = 0.7f;
    private const float DEFAULT_RESTING_COEF = 0.9f;

    //xz 시야 범위
    public float sightRange = DEFAULT_SIGHT_RANGE;

    //y 추가 시야 범위
    public float sightHeight = 0.5f;

    private List<Renderer> renderers = new List<Renderer>();
    private float dayNightCoefficient = 1f;
    private float restingCoefficient = 1f;

    private void Awake()
    {
        renderers.AddRange(GetComponentsInChildren<Renderer>());

        if (gameObject == PlayerManager.Instance.Player)
        {
            DayNightSystem.AddDayStartAction(StartDay);
            DayNightSystem.AddNightStartAction(StartNight);
        }
    }

    private void StartDay()
    {
        dayNightCoefficient = 1f;
        UpdateSightRange();
    }

    private void StartNight()
    {
        dayNightCoefficient = DEFAULT_DAY_NIGHT_COEF;
        UpdateSightRange();
    }

    private void UpdateSightRange()
    {
        sightRange = DEFAULT_SIGHT_RANGE * dayNightCoefficient * restingCoefficient;
    }

    private void OnEnable() => FowManager.AddAllyUnit(this);
    private void OnDisable()
    {
        FowManager.RemoveAllyUnit(this);
        if (gameObject == PlayerManager.Instance.Player)
        {
            DayNightSystem.RemoveDayStartAction(StartDay);
            DayNightSystem.RemoveNightStartAction(StartNight);
        }
    }
    private void OnDestroy()
    {
        FowManager.RemoveAllyUnit(this);
        if (gameObject == PlayerManager.Instance.Player)
        {
            DayNightSystem.RemoveDayStartAction(StartDay);
            DayNightSystem.RemoveNightStartAction(StartNight);
        }
    }
}