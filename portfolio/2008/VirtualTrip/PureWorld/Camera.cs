using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PureWorldLib {
    public class Camera {
        public static double rollDeg = 0;
        public static double pitchDeg = 0;
        public static double yawDeg = 0;

        public static Point point;
        public static Point Pt {
            get { return point; }
        }

        public static Size size;

        private static Rectangle sight;
        public static Rectangle Sight {
            get { return sight; }
        }


        public Camera(int screen_width, int screen_height) {
            size = new Size(screen_width, screen_height);
            point = new Point(0, 0);

            pitchDeg = 90;
            rollDeg = 90;
            yawDeg = 178;

            sight = new Rectangle(point, size);
            UpdateSight();
        }



        private void UpdateSight() {
            point.X = (int)(yawDeg * 3328 / 360);
            point.Y = (int)(pitchDeg * 1664 / 180);
            sight.Location = point;
        }



        public void Roll(double deg) {
            rollDeg += deg;

            if (rollDeg < 0) {
                rollDeg = 0;
            } else if(rollDeg > 180) {
                rollDeg = 180;
            }

            UpdateSight();
        }



        public void Pitch(double deg) {
            pitchDeg += deg;

            if (pitchDeg < 0) {
                pitchDeg = 0;
            } else if (pitchDeg > 180) {
                pitchDeg = 180;
            }

            UpdateSight();
        }

        
        
        public void Yaw(double deg)
        {
            yawDeg += deg;

            if (yawDeg < 0) {
                yawDeg += 360;
            }
            yawDeg %= 360;

            UpdateSight();
        }
    }
}
