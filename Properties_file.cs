using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LPD
{
    public static class Values
    {
        public static bool
            calcSizeByHoles = true,
            workMode = true;

        public static MarkMode markMode;

        public const int
            POINT_WIDTH = 25,
            POINT_HEIGHT = 25,
            DIST_X = 5,
            DIST_Y = 5,
            FORM_BORDER = 15;

        public static readonly string[] modes = new string[(int)MarkMode.LAST_INDEX]
        {
            "Lötpunkt",
            "Belegt",
            "Bohrlich",
            "Drahtbrücke",
            "Frei",
            "Widerstand",
            "LED",
            "Stecker",
            "Sensor",
            "Diode",
            "Pin",
            "Kabelanker",
            "Kabelhalter",
            "Kabel links-rechts",
            "Kabel hoch-runter"
        };

        public static readonly Color[] colors = new Color[(int)MarkMode.LAST_INDEX]
        {
        Color.DarkGray,
        Color.Red,
        Color.Black,
        Color.Silver,
        Color.Green,
        Color.Brown,
        Color.Yellow,
        Color.PapayaWhip,
        Color.Blue,
        Color.LightGray,
        Color.White,
        Color.Orange,
        Color.Olive,
        Color.Silver,
        Color.Silver
        };
    }

    public struct PanelObject
    {
        public Panel _panel;
        public int x, y;
        public MarkMode mode;
        public bool isSolder;
        public CableMode cableDirection;
    }

    public enum ImgIdx
    {
        Left,
        Up,
        Solder,
        Cross_All,
        T_Left,
        T_Right,
        T_Up,
        T_Down,
        Cross_Left_Up,
        Cross_Left_Down,
        Cross_Right_Up,
        Cross_Right_Down,
        LAST_INDEX
    }

    public enum CableMode
    {
        LeftRight,
        UpDown,
        None,
        LAST_INDEX
    }

    public enum MarkMode
    {
        SolderPoint,
        Blocked,
        Hole,
        Bridge,
        Free,
        Resistor,
        LED,
        Connector,
        Sensor,
        Diode,
        Pin,
        CableSpot,
        CableHolder,
        CableLeft,
        CableUp,
        LAST_INDEX
    }
}
