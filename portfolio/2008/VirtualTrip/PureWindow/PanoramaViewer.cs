using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;


namespace PureWindow {
    internal class PanoramaViewer : Viewport3D
    {
        private static string iniFile = System.IO.Directory.GetCurrentDirectory() + "\\3dview.ini";
        private static string section_camera = "Camera";
        private static string section_sphere = "Sphere";


        #region 프로퍼티
        public double FieldOfView {
            get { return (double)GetValue(FieldOfViewProperty); }
            set { SetValue(FieldOfViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FieldOfView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FieldOfViewProperty =
            DependencyProperty.Register("FieldOfView", typeof(double), typeof(PanoramaViewer), new PropertyMetadata(
                    (double)0, new PropertyChangedCallback(OnFieldOfViewChanged)));

        internal static void OnFieldOfViewChanged(Object sender, DependencyPropertyChangedEventArgs e) {
            PanoramaViewer Viewer = ((PanoramaViewer)sender);
            PerspectiveCamera Camera = Viewer.Camera as PerspectiveCamera;
            Camera.FieldOfView = Viewer.FieldOfView;
        }

        public ImageSource PanoramaImage {
            get { return (ImageSource)GetValue(PanoramaImageProperty); }
            set { SetValue(PanoramaImageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PanoramaImage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PanoramaImageProperty =
            DependencyProperty.Register("PanoramaImage", typeof(ImageSource), typeof(PanoramaViewer), new PropertyMetadata(
                    null, new PropertyChangedCallback(OnPanoramaImageChanged)));

        internal static void OnPanoramaImageChanged(Object sender, DependencyPropertyChangedEventArgs e) {
            PanoramaViewer Viewer = ((PanoramaViewer)sender);
            ImageBrush PanoramaBrush = new ImageBrush(Viewer.PanoramaImage);
            Viewer.PanoramaGeometry.BackMaterial = new DiffuseMaterial(PanoramaBrush);
        }
        #endregion

        #region Rotation

        static private readonly Vector3D AxisX = new Vector3D(1, 0, 0);
        static private readonly Vector3D AxisY = new Vector3D(0, 1, 0);
        static private readonly Vector3D AxisZ = new Vector3D(0, 0, 1);

        public static readonly DependencyProperty RotationXProperty =
            DependencyProperty.Register("RotationX", typeof(double), typeof(PanoramaViewer), new UIPropertyMetadata(0.0));
        public static readonly DependencyProperty RotationYProperty =
            DependencyProperty.Register("RotationY", typeof(double), typeof(PanoramaViewer), new UIPropertyMetadata(0.0));
        public static readonly DependencyProperty RotationZProperty =
            DependencyProperty.Register("RotationZ", typeof(double), typeof(PanoramaViewer), new UIPropertyMetadata(0.0));



        public Vector3D Rotation {
            get { return (Vector3D)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rotation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(Vector3D), typeof(PanoramaViewer), new UIPropertyMetadata(new Vector3D(), (d, s) => ((PanoramaViewer)d).UpdateRotation((d as PanoramaViewer).Rotation)));




        public double RotationX {
            get { return (double)GetValue(RotationXProperty); }
            set { SetValue(RotationXProperty, value); UpdateRotation(); }
        }
        public double RotationY {
            get { return (double)GetValue(RotationYProperty); }
            set { SetValue(RotationYProperty, value); UpdateRotation(); }
        }
        public double RotationZ {
            get { return (double)GetValue(RotationZProperty); }
            set { SetValue(RotationZProperty, value); UpdateRotation(); }
        }

        private void UpdateRotation() {
            Quaternion qx = new Quaternion(AxisX, RotationX);
            Quaternion qy = new Quaternion(AxisY, RotationY);
            Quaternion qz = new Quaternion(AxisZ, 0);
            PanoramaRotation.Quaternion = qx * qy * qz;
        }


        public void UpdateRotation(Vector3D rotation) {
            Quaternion qx = new Quaternion(AxisX, rotation.X);
            Quaternion qy = new Quaternion(AxisY, rotation.Y);
            Quaternion qz = new Quaternion(AxisZ, 0);
            PanoramaRotation.Quaternion = qx * qy * qz;
            CameraRotation.Angle = rotation.Z;

            SetValue(RotationXProperty, rotation.X);
            SetValue(RotationYProperty, rotation.Y);
            SetValue(RotationZProperty, rotation.Z);
        }


        QuaternionRotation3D PanoramaRotation { get; set; }
        AxisAngleRotation3D CameraRotation { get; set; }

        #endregion


        public PanoramaViewer() {
            InitializeViewer();
        }


        #region Sphere-mesh
        public ModelVisual3D PanoramaObject { get; set; }
        public GeometryModel3D PanoramaGeometry { get; set; }

        public void InitializeViewer() {
            INIController.IniCntl ini = new INIController.IniCntl(iniFile);

            ///////////////////////////////////////////
            // Camera Initialize
            ///////////////////////////////////////////
            PerspectiveCamera PanoramaCamera = new PerspectiveCamera();
            double posX = double.Parse(ini.ReadValue(section_camera, "PositionX", "0"));
            double posY = double.Parse(ini.ReadValue(section_camera, "PositionY", "-0"));
            double posZ = double.Parse(ini.ReadValue(section_camera, "PositionZ", "0"));
            double field_of_view = double.Parse(ini.ReadValue(section_camera, "FieldOfView", "89"));
            PanoramaCamera.Position = new Point3D(posX, posY, posZ);
            PanoramaCamera.UpDirection = new Vector3D(0, 1, 0);
            PanoramaCamera.LookDirection = new Vector3D(0, 0, 1);
            PanoramaCamera.FieldOfView = field_of_view;
            this.Camera = PanoramaCamera;

            FieldOfView = field_of_view;

            ///////////////////////////////////////////
            // Light Initialize
            ///////////////////////////////////////////
            ModelVisual3D LightModel = new ModelVisual3D();
            //LightModel.Content = new DirectionalLight(Colors.White, new Vector3D(0, 0, 1));
            LightModel.Content = new AmbientLight(Colors.White);
            
            this.Children.Add(LightModel);

            ///////////////////////////////////////////
            // Panorama Object Initialize
            ///////////////////////////////////////////

            PanoramaObject = new ModelVisual3D();
            PanoramaGeometry = new GeometryModel3D();
            PanoramaGeometry.Geometry = CreateGeometry();
            PanoramaObject.Content = PanoramaGeometry;

            RotateTransform3D RotateTransform = new RotateTransform3D();
            double scale_x = double.Parse(ini.ReadValue(section_sphere, "ScaleX", "1"));
            double scale_y = double.Parse(ini.ReadValue(section_sphere, "ScaleY", "1.60"));
            double scale_z = double.Parse(ini.ReadValue(section_sphere, "ScaleZ", "1"));
            ScaleTransform3D ScaleTransform = new ScaleTransform3D() { ScaleX = scale_x, ScaleY = scale_y, ScaleZ = scale_z };
            Transform3DGroup Group = new Transform3DGroup();
            
            PanoramaRotation = new QuaternionRotation3D();
            RotateTransform.Rotation = PanoramaRotation;
 
            Group.Children.Add(ScaleTransform);
            Group.Children.Add(RotateTransform);
            PanoramaObject.Transform = Group;
            

            RotateTransform3D CameraRotateTrnasfrom = new RotateTransform3D();
            CameraRotation = new AxisAngleRotation3D(new Vector3D(0, 0, 1), 0);
            CameraRotateTrnasfrom.Rotation = CameraRotation;
            PanoramaCamera.Transform = CameraRotateTrnasfrom;

            this.Children.Add(PanoramaObject);
        }


        private Geometry3D CreateGeometry() {
            int tDiv = 64;
            int yDiv = 64;
            double maxTheta = (360.0 / 180.0) * Math.PI;
            double minY = -1.0;
            double maxY = 1.0;

            double dt = maxTheta / tDiv;
            double dy = (maxY - minY) / yDiv;

            MeshGeometry3D mesh = new MeshGeometry3D();

            for (int yi = 0; yi <= yDiv; yi++) {
                double y = minY + yi * dy;

                for (int ti = 0; ti <= tDiv; ti++) {
                    double t = ti * dt;

                    mesh.Positions.Add(GetPosition(t, y));
                    mesh.Normals.Add(GetNormal(t, y));
                    mesh.TextureCoordinates.Add(GetTextureCoordinate(t, y));
                }
            }

            for (int yi = 0; yi < yDiv; yi++) {
                for (int ti = 0; ti < tDiv; ti++) {
                    int x0 = ti;
                    int x1 = (ti + 1);
                    int y0 = yi * (tDiv + 1);
                    int y1 = (yi + 1) * (tDiv + 1);

                    mesh.TriangleIndices.Add(x0 + y0);
                    mesh.TriangleIndices.Add(x0 + y1);
                    mesh.TriangleIndices.Add(x1 + y0);

                    mesh.TriangleIndices.Add(x1 + y0);
                    mesh.TriangleIndices.Add(x0 + y1);
                    mesh.TriangleIndices.Add(x1 + y1);
                }
            }

            mesh.Freeze();
            return mesh;
        }

        internal Point3D GetPosition(double t, double y) {
            double r = Math.Sqrt(1 - y * y);
            double x = r * Math.Cos(t);
            double z = r * Math.Sin(t);

            return new Point3D(x, y, z);
        }

        private Vector3D GetNormal(double t, double y) {
            return (Vector3D)GetPosition(t, y);
        }

        private Point GetTextureCoordinate(double t, double y) {
            Matrix TYtoUV = new Matrix();
            TYtoUV.Scale(1 / (2 * Math.PI), -0.5);

            Point p = new Point(t, y);
            p = p * TYtoUV;

            return p;
        }
        #endregion
    }
}
