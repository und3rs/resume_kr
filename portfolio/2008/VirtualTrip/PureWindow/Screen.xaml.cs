using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using System.Threading;
using PureWorldLib;

namespace PureWindow {
    /// <summary>
    /// PureScreen.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Screen : UserControl {
        private PureWorldLib.WorldMaker mk;
        private RotateTransform compasRt = new RotateTransform() { CenterX = 0, CenterY = 0, Angle = 0.0 };
        private Thread thread;
        private double sightGap;
        
        // 가공된 벡터값들
        private double curr_yaw_deg = 0;
        private double curr_pitch_deg = 0;
        private double curr_roll_deg = 0;

        // 카메라 보정(북쪽)
        private double north_deg = 0;
        private double defaultValue = -45 - 45;

        // 보정값
        private double fix_yaw_deg = 0;
        private double fix_pitch_deg = 0;
        private double fix_roll_deg = 0;

        private Vector RotationVector = new Vector();
        private Point DownPoint = new Point();

        private PureWorldLib.World world;

        // 델리게이터
        private delegate void ChangeMapDelegate(World world);
        private delegate void ToggleNoticeDelegate(bool value);



        //===========================================
        // 생성자
        //===========================================
        public Screen() {
            InitializeComponent();
            
            string iniFile = System.IO.Directory.GetCurrentDirectory() + "\\config.ini";
            INIController.IniCntl ini = new INIController.IniCntl(iniFile);
            this.sightGap = double.Parse(ini.ReadValue("WALK", "SightGap", "10"));

            Viewer.MouseLeftButtonDown += new MouseButtonEventHandler(Viewer_MouseLeftButtonDown);
            Viewer.MouseMove += new MouseEventHandler(Viewer_MouseMove);

            mk = new PureWorldLib.WorldMaker();
            mk.DownloadStarted += new EventHandler<PureWorldLib.WorldsCreateStartEventArgs>(mk_MapDownloadStarted);
            mk.DownloadCompleted += new EventHandler<PureWorldLib.WorldsCreateCompletedEventArgs>(mk_MapDownloadCompleted);

            compasRt.CenterX = (int)compas.Width / 2;
            compasRt.CenterY = (int)compas.Height / 2;
            compas.RenderTransform = compasRt;
        }




        //===========================================
        // 이동
        //===========================================
        public void Go(double lat, double lng) {
            // 쓰레드가 이미 돌고 있다면 요청 무시
            if (thread != null && thread.ThreadState != ThreadState.Stopped) {
                return;
            }

            ParameterizedThreadStart td = new ParameterizedThreadStart(ThreadMoveRun);
            thread = new Thread(td);
            thread.Start((new MapInfo(lat, lng)) as object);
        }



        //===========================================
        // 이동
        //===========================================
        public void Go(String panoID) {
            // 쓰레드가 이미 돌고 있다면 요청 무시
            if (thread != null && thread.ThreadState != ThreadState.Stopped) {
                return;
            }

            ParameterizedThreadStart td = new ParameterizedThreadStart(ThreadMoveRun);
            thread = new Thread(td);
            thread.Start((new MapInfo(panoID)) as object);
        }




        //============================================
        // 크로스 쓰레드 방지 (다운로드 완료 이벤트에서 호출)
        //============================================
        private void ChangeMap(World world) {
            Viewer.PanoramaImage = new BitmapImage(new Uri(System.IO.Directory.GetCurrentDirectory() + world.FilePath));
            // ### 중요코드 시작 ###
            // 카메라를 파노라마 이미지 중간으로 조정
            north_deg = (defaultValue + 180) + (360 - world.pano_yaw_deg);
            //curr_pitch_deg = world.tilt_pitch_deg;
            //Viewer.RotationZ = curr_pitch_deg;
            // ### 중요코드 종료 ###
            UpdateStatusView();
            

            System.Diagnostics.Debug.WriteLine(world.panoID + " 다운로드 완료");
        }




        //===========================================
        // Go, GoAhead 에서 CrossThread 방지를 위해서사용
        //===========================================
        void ThreadMoveRun(Object obj) {
            MapInfo info = obj as MapInfo;

            if (info.InfoType == MapInfo.Type.ID) {
                mk.Move(info.ID);
            } else if (info.InfoType == MapInfo.Type.POINT) {
                mk.Move(info.Pos.X, info.Pos.Y);
            }
        }



        //===========================================
        // world 다운로드 시작 이벤트 핸들러
        //===========================================
        void mk_MapDownloadStarted(object sender, PureWorldLib.WorldsCreateStartEventArgs e) {
            ToggleNoticeDelegate deleToggleNotice = new ToggleNoticeDelegate(ToggleNotice);
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, deleToggleNotice, true);

            System.Diagnostics.Debug.WriteLine(e.world.panoID + " 다운로드 시작");
        }



        //===========================================
        // world 다운로드 완료 이벤트 핸들러
        //===========================================
        void mk_MapDownloadCompleted(object sender, PureWorldLib.WorldsCreateCompletedEventArgs e) {
            this.world = e.world;
            ChangeMapDelegate deleChangeMap = new ChangeMapDelegate(ChangeMap);
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, deleChangeMap, e.world);

            ToggleNoticeDelegate deleToggleNotice = new ToggleNoticeDelegate(ToggleNotice);
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, deleToggleNotice, false);
        }




        //===========================================
        // 다운로드 안내창 토글
        //===========================================
        void ToggleNotice(bool value) {
            if (value == true) {
                notice.Visibility = Visibility.Visible;
            } else {
                notice.Visibility = Visibility.Hidden;
            }
        }




        //===========================================
        // 현재 화면상태 업데이트 - 모든경우에 대해서 호출됨
        //===========================================
        private void UpdateStatusView() {
            // 나침반에 보여지는 각도
            this.curr_pitch_deg = Viewer.RotationX;
            //this.curr_yaw_deg = (Viewer.Rotation.Y - world.pano_yaw_deg + world.tilt_yaw_deg + 360) % 360;
            this.curr_yaw_deg = (Viewer.RotationY - north_deg + 360) % 360;
            //this.curr_yaw_deg = (Viewer.RotationY + 45 + world.tilt_yaw_deg + 360) % 360;
            //this.curr_yaw_deg = (Viewer.Rotation.Y + this.fix_yaw_deg + world.pano_yaw_deg) % 360;
            this.curr_roll_deg = Viewer.RotationZ;

            textBlock1.Text = world.region;
            lblAxisX.Content = this.curr_pitch_deg as object;
            lblAxisY.Content = this.curr_yaw_deg as object;
            lblAxisZ.Content = this.curr_roll_deg as object;
            

            compasRt.Angle = -curr_yaw_deg;

            System.Diagnostics.Debug.WriteLine(String.Format("실제 스피어 : {0}\n보정값 : {1}\n화면상 값: {2}\n", Viewer.RotationY, fix_yaw_deg, curr_yaw_deg));
        }




        //===========================================
        // 앞으로 이동(보행)
        //===========================================
        public void GoAhead() {
            // 쓰레드가 이미 돌고 있다면 요청 무시
            if (thread != null && thread.ThreadState != ThreadState.Stopped) {
                return;
            }
            
            // World 가 있는 상태가 아니면 요청무시
            if (this.world == null) {
                return;
            }

            foreach (PureWorldLib.Adjacent adj in world.adjacents) {
                if (curr_yaw_deg > adj.direction_deg - sightGap && curr_yaw_deg < adj.direction_deg + sightGap) {
                    System.Diagnostics.Debug.WriteLine("["+adj.panoID + "]" + adj.direction_deg + " 앞으로 이동합니다.");
                    ParameterizedThreadStart td = new ParameterizedThreadStart(ThreadMoveRun);
                    thread = new Thread(td);
                    thread.Start((new MapInfo(adj.panoID)) as object);
                    break;
                }
            }
        }



        //===========================================
        // 마우스 이벤트
        //===========================================
        void Viewer_MouseMove(object sender, MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Released) return;
            Vector Offset = Point.Subtract(e.GetPosition(Viewer), DownPoint) * 0.25;

            double x, y;
            x = (RotationVector.X + Offset.Y + 360) % 360;
            y = (RotationVector.Y - Offset.X + 360) % 360;

            // 추가
            //curr_pitch_deg = x;
            //curr_yaw_deg = y;


            Viewer.Rotation = new Vector3D(x, y, Viewer.RotationZ);

            UpdateStatusView();
        }



        //===========================================
        // 마우스 이벤트
        //===========================================
        void Viewer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            DownPoint = e.GetPosition(Viewer);
            RotationVector.X = Viewer.RotationX;
            RotationVector.Y = Viewer.RotationY;
        }



        //======================================
        // 화면(시야)을 돌림 -- 하드웨어 접목
        //======================================
        public void SetRotation(Vector3D rotation) {
            Viewer.Rotation = rotation;
            UpdateStatusView();
        }



        //======================================
        // 사용자 메뉴 F1
        //======================================
        public void ToggleDebugMode() {
            if (DebugWindow.Visibility != Visibility.Visible) {
                DebugWindow.Visibility = Visibility.Visible;
            } else {
                DebugWindow.Visibility = Visibility.Hidden;
            }
        }


        //======================================
        // 사용자 메뉴 F2
        //======================================
        public void ToggleCompass() {
            if (compas.Visibility != Visibility.Visible) {
                compas.Visibility = Visibility.Visible;
            } else {
                compas.Visibility = Visibility.Hidden;
            }
        }



        //===========================================
        // 디버그용
        //===========================================
        public void DebugPrintAdjacents() {
            System.Diagnostics.Debug.WriteLine("[ID] " + world.panoID);
            System.Diagnostics.Debug.WriteLine(String.Format("pano_yaw_deg: {0}, tilt_yaw_deg: {1}", world.pano_yaw_deg, world.tilt_yaw_deg));
            if (world != null) {
                foreach (Adjacent adj in world.adjacents) {
                    System.Diagnostics.Debug.WriteLine(adj.direction_deg);
                }
            }
        }
    }



    //=========================================
    // 다운로드 쓰레드를 위해서 만든 정보 클래스
    //=========================================
    class MapInfo {
        public enum Type { POINT, ID }
        public MapInfo.Type InfoType { get; set; }
        public Point Pos { get; set; }
        public String ID { get; set; }

        public MapInfo(double lat, double lng) {
            this.InfoType = Type.POINT;
            this.Pos = new Point(lat, lng);
        }

        public MapInfo(String panoID) {
            this.InfoType = Type.ID;
            this.ID = panoID;
        }
    }
}
