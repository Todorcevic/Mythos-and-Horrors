using UnityEngine;

namespace MythosAndHorrors.GameView
{
    public class ViewValues
    {
        public const float INSTANT_TIME_ANIMATION = 0f;
        public const float VERYFAST_TIME_ANIMATION = 0.1f;
        public const float FAST_TIME_ANIMATION = 0.25f;
        public const float DEFAULT_TIME_ANIMATION = 0.4f;
        public const float MID_TIME_ANIMATION = 0.5f;
        public const float SLOW_TIME_ANIMATION = 0.75f;
        public const float DELAY_TIME_ANIMATION = 0.01f;

        public const float CARD_ORIGINAL_SCALE = 1f;
        public const float CARD_THICKNESS = 0.1f;
        public const string EMPTY_STAT = "0";
        public const float INITIAL_LAYOUT_WIDTH = 24f;
        public const float DEFAULT_FADE = 0.5f;
        public const float DEFAULT_SCALE = 1.2f;

        public static Color ACTIVE_COLOR = new(0.1607843f, 0.4039216f, 0.05098039f);
        public static Color DEACTIVE_COLOR = new(0.4528302f, 0.07903168f, 0.07903168f);
        public static Color DEFAULT_COLOR = new(0.7019608f, 0.772549f, 0.8784314f);
        public static Color EXAUST_COLOR = new(1f, 0.4f, 0.4f);
        public static Color YELLOW_FONT_COLOR = new(1f, 0.9686275f, 0.7686275f);
        public static Color GREEN_FONT_COLOR = new(0f, 1f, 0f);
        public static Color RED_FONT_COLOR = new(1f, 0f, 0f);

        public const string NOT_WAITABLE_ANIMATION = "NOT_WAITABLE";
        public const string MOVE_ANIMATION = "MOVE_ANIMATION";
    }
}
