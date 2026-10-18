using UnityEngine;
using UnityEngine.UI;

public class UI_Life : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Slider slider;

    void Update()
    {
        if (!slider || !player) return;

        float percentage = player.GetLifePercentage();
        slider.value = percentage;
    }
}
