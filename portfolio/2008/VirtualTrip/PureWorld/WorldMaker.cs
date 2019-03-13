using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;
using System.Xml;
using System.Threading;

namespace PureWorldLib {
    public class WorldMaker {
        public event EventHandler<WorldsCreateCompletedEventArgs> DownloadCompleted;
        public event EventHandler<WorldsCreateStartEventArgs> DownloadStarted;
        public event EventHandler<WorldsCreateProgressEventArgs> Downloading;

        //private bool IsNowDownloading = false;
        private Stack<string> panoIDs;


        //=========================================
        // 생성자
        //=========================================
        public WorldMaker() {
            // 필요한 각종 폴더를 생성
            if (Directory.Exists(Property.WorldPath) == false) {
                Directory.CreateDirectory(Property.WorldPath);
            }

            if (Directory.Exists(Property.TempPath) == false) {
                Directory.CreateDirectory(Property.TempPath);
            }

            if (Directory.Exists(Property.XmlPath) == false) {
                Directory.CreateDirectory(Property.XmlPath);
            }

            panoIDs = new Stack<string>();
        }




        //=========================================
        // 특정 지역으로 이동
        //=========================================
        public void Move(double lat, double lng) {
            // 메인 World 를 다운로드 후, drawing
            World world = new World(lat, lng);
            CreateWorld(world);
            System.Diagnostics.Debug.WriteLine("현재 World: " + world.panoID);

            //// 방사형태로 주변 World 를 생성
            //foreach (Adjacent adjacent in world.adjacents) {
            //    panoIDs.Push(adjacent.panoID);
            //}

            //// 주변 World 를 다운로드
            //if (bDownload == false) {
            //    Thread td = new Thread(CreateRadialWorld);
            //    td.Start();
            //}

            // 다운완료 이벤트
            if (DownloadCompleted != null) {
                DownloadCompleted(this, new WorldsCreateCompletedEventArgs(world));
            }
        }

        public void Move(String panoID) {
            // 메인 World 를 다운로드 후, drawing
            World world = new World(panoID);
            CreateWorld(world);
            System.Diagnostics.Debug.WriteLine("현재 World: " + world.panoID);

            //// 방사형태로 주변 World 를 생성
            //foreach (Adjacent adjacent in world.adjacents) {
            //    panoIDs.Push(adjacent.panoID);
            //}

            // 주변 World 를 다운로드
            //if(bDownload == false) {
            //    Thread td = new Thread(CreateRadialWorld);
            //    td.Start();
            //}

            // 다운완료 이벤트
            if (DownloadCompleted != null) {
                DownloadCompleted(this, new WorldsCreateCompletedEventArgs(world));
            }
        }



        //=========================================
        // 주변 지역의 World 를 구성
        //=========================================
        //private void CreateRadialWorld(Object world) {
        //    // 방사형태로 주변 World 를 생성
        //    foreach (Adjacent adjacent in (world as World).adjacents) {
        //        CreateWorld(new World(adjacent.panoID));
        //    }

        //    // 이벤트 발생
        //    if (AdjacentCompleted != null) {
        //        AdjacentCompleted(world as World, new WorldsCreateCompletedEventArgs(Property.WorldPath + (world as World).panoID));
        //    }
        //}

        //private void CreateRadialWorld() {
        //    bDownload = true;
        //    string panoID;

        //    while (panoIDs.Count > 0) {
        //        panoID = panoIDs.Pop();

        //        // 다운시작 이벤트
        //        if (DownloadStarted != null) {
        //            DownloadStarted(panoID, new WorldsCreateStartEventArgs(Property.WorldPath + panoID));
        //        }

        //        // 다운로드
        //        CreateWorld(new World(panoID));

        //        // 다운완료 이벤트
        //        if (DownloadCompleted != null) {
        //            DownloadCompleted(panoID, new WorldsCreateCompletedEventArgs(Property.WorldPath + panoID));
        //        }
        //    }
        //    bDownload = false;
        //}



        //=========================================
        // World 를 생성
        //=========================================
        private void CreateWorld(World world) {
            // 만들어 놓은 world 가 없는 경우
            if (ExistWorld(world) == false) {
                // 다운시작 이벤트
                if (DownloadStarted != null) {
                    DownloadStarted(this, new WorldsCreateStartEventArgs(world));
                }

                DownloadSliceImage(world);
                AttachImages(world);
            }
        }



        //=========================================
        // Image 조각들을 다운로드
        //=========================================
        private bool DownloadSliceImage(World world) {
            string prefixParam = "output=tile";
            prefixParam += "&panoid=" + world.panoID + "&zoom=3";
            WebClient client = new WebClient();

            // 다운로드
            for (int y = 0; y < Property.VERTICAL_IMG_NUM; y++) {
                for (int x = 0; x < Property.HORIZON_IMG_NUM; x++) {
                    string param = prefixParam + "&x=" + x + "&y=" + y + "&cb_client=maps_sv_ta";
                    if (File.Exists(Property.TempPath + world.panoID + x + y) == false) {
                        try {
                            client.DownloadFile(new Uri(Property.URL_DOWN + param), Property.TempPath + world.panoID + x + y);
                        } catch (Exception e) {
                            throw new CanNotDownLoadImageException(Property.URL_DOWN + param);
                        }
                    }
                    // 다운로드 진행상태 이벤트
                    if (Downloading != null) {
                        Downloading(this, new WorldsCreateProgressEventArgs(y * Property.VERTICAL_IMG_NUM + x + 1));
                    }
                }
            }

            client.Dispose();

            return true;
        }



        //=========================================
        // slice image 를 붙여서 하나의 world 를 구성
        //=========================================
        private void AttachImages(World world) {
            Bitmap canvas = new Bitmap(Property.image_width, Property.image_height);
            Graphics g = Graphics.FromImage(canvas);

            for (int y = 0; y < Property.VERTICAL_IMG_NUM; y++) {
                for (int x = 0; x < Property.HORIZON_IMG_NUM; x++) {
                    string filePath = Property.TempPath + world.panoID + x + y;
                    Bitmap origin = new Bitmap(filePath);
                    Rectangle srcRect = new Rectangle(0, 0, origin.Width, origin.Height);
                    Rectangle dstRect = new Rectangle(origin.Width * x, origin.Height * y, origin.Width, origin.Height);
                    g.DrawImage(origin, dstRect, srcRect, GraphicsUnit.Pixel);
                    origin.Dispose();
                    try {
                        File.Delete(filePath);
                    } catch {
                        // 파일이 사용중이어서 지울수 없는 경우는 그냥 포기(지장없음)
                    }
                }
            }
            g.Dispose();

            canvas.Save(world.FilePath);
            canvas.Dispose();
        }




        //=========================================
        // 해당 World 가 Cashing 되는지 확인
        //=========================================
        private bool ExistWorld(World world) {
            return File.Exists(world.FilePath);
        }
    }






    [Serializable]
    public class CanNotDownLoadImageException : ApplicationException {
        public CanNotDownLoadImageException() {
        }

        public CanNotDownLoadImageException(string message)
            : base(message) {
        }
    }
}
