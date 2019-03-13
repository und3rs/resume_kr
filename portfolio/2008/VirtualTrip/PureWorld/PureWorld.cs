using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PureWorldLib
{
    public class PureWorld
    {
        private WorldMaker worldMaker;
        private Camera camera;

        private World world;
        private Graphics canvas;
        private Image pano;

        private int canvas_width;
        private int canvas_height;

        private bool bReady = false;

        public event EventHandler<WorldsCreateCompletedEventArgs> AdjacentCompleted;


        
        //=========================================
        // 생성자
        //=========================================
        public PureWorld(Graphics g, int width, int height) {
            canvas = g;
            canvas_width = width;
            canvas_height = height;

            camera = new Camera(width, height);
            pano = new Bitmap(1, 1);
            worldMaker = new WorldMaker();
            worldMaker.AdjacentCompleted += new EventHandler<WorldsCreateCompletedEventArgs>(worldMaker_AdjacentCompleted);
        }


        
        //=========================================
        // 다운로드 완료시 이벤트 발생
        //=========================================
        void worldMaker_AdjacentCompleted(object sender, WorldsCreateCompletedEventArgs e) {
            bReady = true;
            if (AdjacentCompleted != null) {
                AdjacentCompleted(sender, e);
            }
        }



        //=========================================
        // 위도 경도에 해당하는 곳을 여행
        //=========================================
        public void Trip(double lat, double lng) {      // 최초 여행
            bReady = false;
            world = worldMaker.Goto(lat, lng);

            pano.Dispose();
            pano = new Bitmap(world.FilePath);

            Camera.yawDeg = world.yaw_deg;
            
            DrawWorld();
        }


        public void Trip(String panoID) {               // 연결된 이동
            bReady = false;
            world = worldMaker.Goto(panoID);

            pano.Dispose();
            pano = new Bitmap(world.FilePath);

            DrawWorld();
        }



        //=========================================
        // Pitch
        //=========================================
        public void Pitch(double angle) {
            camera.Pitch(angle);
            DrawWorld();
        }



        //=========================================
        // Yaw
        //=========================================
        public void Yaw(double angle) {
            camera.Yaw(angle);
            DrawWorld();
        }


        //=========================================
        // Roll
        //=========================================
        public void Roll(double angle) {
            camera.Roll(angle);
            DrawWorld();
        }


        //=========================================
        // GoAhead
        //=========================================
        public void GoAhead() {
            if (bReady == false) {
                return;
            }
            foreach (Adjacent adjacent in world.adjacents) {
                System.Diagnostics.Debug.WriteLine("갈수있는 곳: " + adjacent.direction_deg);
                if (Camera.yawDeg > adjacent.direction_deg - 10 && Camera.yawDeg < adjacent.direction_deg + 10) {
                    Trip(adjacent.panoID);
                    break;
                }
            }
        }




        //=========================================
        // 해당 Word 를 그림
        //=========================================
        private void DrawWorld() {

            Rectangle left;
            Rectangle right;

            if (Camera.Sight.X + canvas_width > Property.image_width) { //오른족으로 넘치는 경우
                left = new Rectangle(Camera.Pt.X, Camera.Pt.Y, pano.Width - Camera.Pt.X, canvas_height);
                right = new Rectangle(0, Camera.Pt.Y, canvas_width - left.Width, canvas_height);
            } else if (Camera.Sight.X < 0) {    //왼족으로 넘치는 경우
                left = new Rectangle(pano.Width+Camera.Sight.X, Camera.Pt.Y, -Camera.Sight.X, canvas_height);
                right = new Rectangle(0, Camera.Pt.Y, canvas_width - left.Width, canvas_height);
            } else {        // 일반적인 경우
                left = Camera.Sight;
                right = new Rectangle(0, 0, 0, 0);
            }

            // 붙이는 부분
            Rectangle dstRectLeft = new Rectangle(0, 0, left.Width, left.Height);
            Rectangle dstRectRight = new Rectangle(left.Width, 0, right.Width, right.Height);
            canvas.DrawImage(pano, dstRectRight, right, GraphicsUnit.Pixel);
            canvas.DrawImage(pano, dstRectLeft, left, GraphicsUnit.Pixel);

            System.Diagnostics.Debug.WriteLine(Camera.yawDeg.ToString());
        }

    }
}
