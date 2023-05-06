using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class InGameGlobalUI : MonoBehaviour
{
    private const string INGAME_SPRITES_PATH = "09.InGameUI/Sprite/";
    private const string DAY_SUN_NAME = "Ico_DaySun";
    private const string NIGHT_MOON_NAME = "Ico_NightMoon";

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

    public void UpdateUserNumber(int currentUserNumber)
    {
        userNumberText.text = $"{currentUserNumber}";
    }

    private void Awake()
    {
        dayNightIcons[0] = Resources.Load<Sprite>($"{INGAME_SPRITES_PATH}{DAY_SUN_NAME}");
        dayNightIcons[1] = Resources.Load<Sprite>($"{INGAME_SPRITES_PATH}{NIGHT_MOON_NAME}");

        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        defaultLightColor = directionalLight.color;

        UpdateUserNumber(2);
    }

    private void Start()
    {
        StartCoroutine(TimerLoop());
    }

    private void UpdateTimer()
    {
        timerText.text = $"{minutes.ToString().PadLeft(2, '0')} : {seconds.ToString().PadLeft(2, '0')}";
    }

    private IEnumerator TimerLoop()
    {
        WaitForSeconds waitForOneSecond = new WaitForSeconds(0.01f);

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
                    dayNightImage.sprite = isNight ? dayNightIcons[1] : dayNightIcons[0];
                    dayText.text = isNight ? dayText.text : $"DAY {++dayCount}";
                    directionalLight.color = isNight ? Color.grey : defaultLightColor;

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
}
