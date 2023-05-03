using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameGlobalUI : MonoBehaviour
{
    private const string INGAME_SPRITES_PATH = "09.UI/InGame/Sprites/";
    private const string DAY_SUN_NAME = "Ico_DaySun";
    private const string NIGHT_MOON_NAME = "Ico_NightMoon";

    [SerializeField] private Text timer;
    [SerializeField] private Image dayNight;
    private Sprite[] dayNightIcons = new Sprite[2];

    private int minutes = 2;
    private int seconds = 30;

    private bool isNight = false;

    private void Awake()
    {
        dayNightIcons[0] = Resources.Load($"{INGAME_SPRITES_PATH}{DAY_SUN_NAME}") as Sprite;
        dayNightIcons[1] = Resources.Load($"{INGAME_SPRITES_PATH}{NIGHT_MOON_NAME}") as Sprite;
    }

    private void Start()
    {
        StartCoroutine(TimerLoop());
    }

    private void UpdateTimer()
    {
        timer.text = $"{minutes.ToString().PadLeft(2, '0')} : {seconds.ToString().PadLeft(2, '0')}";
    }

    private IEnumerator TimerLoop()
    {
        WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

        while (true)
        {
            if (seconds == 0)
            {
                if (minutes == 0)
                {
                    minutes = 2;
                    seconds = 30;
                    isNight = !isNight;

                    yield return waitForOneSecond;
                    continue;
                }

                --minutes;
            }

            --seconds;

            yield return waitForOneSecond;
        }
    }
}
