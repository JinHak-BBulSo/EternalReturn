using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum DayNightType
{
    Day = 0,
    Night = 1
}

public class DayNightSystem : MonoBehaviour
{
    private const string INGAME_SPRITES_PATH = "09.InGameUI/Sprite/";
    private const string DAY_SUN_NAME = "Ico_DaySun";
    private const string NIGHT_MOON_NAME = "Ico_NightMoon";

    private static UnityEvent dayStartEvent = new UnityEvent();
    private static UnityEvent nightStartEvent = new UnityEvent();

    [SerializeField] private Text timerText;
    [SerializeField] private Image dayNightImage;
    [SerializeField] private Text dayText;
    [SerializeField] private Text userNumberText;

    private Light directionalLight;
    private Color defaultLightColor;
    private Sprite[] dayNightIcons = new Sprite[2];

    private int minutes = 2;
    private int seconds = 30;
    private int dayCount = 1;

    private bool isNight = false;

    private bool isGameStart = false;

    public static void AddDayStartAction(UnityAction action)
    {
        dayStartEvent.AddListener(action);
    }

    public static void RemoveDayStartAction(UnityAction action)
    {
        dayStartEvent.RemoveListener(action);
    }

    public static void AddNightStartAction(UnityAction action)
    {
        nightStartEvent.AddListener(action);
    }

    public static void RemoveNightStartAction(UnityAction action)
    {
        nightStartEvent.RemoveListener(action);
    }

    public void UpdateUserNumber(int currentUserNumber)
    {
        userNumberText.text = $"{currentUserNumber}";
    }

    private void Awake()
    {
        dayNightIcons[(int)DayNightType.Day] = Resources.Load<Sprite>($"{INGAME_SPRITES_PATH}{DAY_SUN_NAME}");
        dayNightIcons[(int)DayNightType.Night] = Resources.Load<Sprite>($"{INGAME_SPRITES_PATH}{NIGHT_MOON_NAME}");

        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        defaultLightColor = directionalLight.color;

        dayStartEvent.AddListener(StartDay);
        nightStartEvent.AddListener(StartNight);

        UpdateUserNumber(2);
    }

    private void Update()
    {
        if(PlayerManager.Instance.IsGameStart && !isGameStart)
        {
            isGameStart = true;
            StartCoroutine(TimerLoop());
        }
    }

    private void UpdateTimer()
    {
        timerText.text = $"{minutes.ToString().PadLeft(2, '0')} : {seconds.ToString().PadLeft(2, '0')}";
    }

    private IEnumerator TimerLoop()
    {
        WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

        while (true)
        {
            --seconds;
            if (seconds == -1)
            {
                if (minutes == 0)
                {
                    minutes = 2;
                    seconds = 30;
                    isNight = !isNight;

                    if (!isNight)
                    {
                        dayStartEvent?.Invoke();
                    }
                    else
                    {
                        nightStartEvent?.Invoke();
                    }

                    UpdateTimer();
                    yield return waitForOneSecond;
                    continue;
                }

                seconds = 59;
                --minutes;
            }

            UpdateTimer();
            yield return waitForOneSecond;
        }
    }

    private void StartDay()
    {
        dayText.text = $"DAY {++dayCount}";
        dayNightImage.sprite = dayNightIcons[0];
        directionalLight.color = defaultLightColor;
    }

    private void StartNight()
    {
        dayNightImage.sprite = dayNightIcons[1];
        directionalLight.color = Color.gray;
    }
}
