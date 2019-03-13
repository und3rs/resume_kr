using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;
using System.Xml;

namespace PureWorldLib
{
    //##############################################
    // Class World
    //##############################################
    public class World {
        public String panoID;
        public String region;
        public String country;
        public double pano_yaw_deg;
        public double tilt_pitch_deg;
        public double tilt_yaw_deg;
        public List<Adjacent> adjacents;

        #region 프로퍼티
        public String FilePath {
            get {
                return Property.WorldPath + panoID;
            }
        }
        #endregion


        //=========================================
        // 생성자
        //=========================================
        public World(double lat, double lng) {
            this.panoID = GetPanoID(lat, lng);
            this.region = String.Empty;
            this.country = String.Empty;
            adjacents = new List<Adjacent>();
            //state = new WorldState();

            updateInfo();
        }


        //=========================================
        // 생성자
        //=========================================
        public World(String panoID) {
            this.panoID = panoID;
            this.region = String.Empty;
            this.country = String.Empty;
            adjacents = new List<Adjacent>();

            updateInfo();
        }



        //=========================================
        // 각종 정보를 구함
        //=========================================
        private void updateInfo() {
            XmlTextReader xmlReader = GetXmlInfo(panoID);
            while (xmlReader.Read()) {
                if (xmlReader.Name.Equals("region") == true) {
                    region = xmlReader.ReadString();
                } else if (xmlReader.Name.Equals("projection_properties") == true) {
                    this.pano_yaw_deg = double.Parse(xmlReader.GetAttribute("pano_yaw_deg"));
                    this.tilt_pitch_deg = double.Parse(xmlReader.GetAttribute("tilt_pitch_deg"));
                    this.tilt_yaw_deg = double.Parse(xmlReader.GetAttribute("tilt_yaw_deg"));
                } else if (xmlReader.Name.Equals("country") == true) {
                    country = xmlReader.ReadString();
                } else if (xmlReader.Name.Equals("link") == true) {
                    if (xmlReader.GetAttribute("pano_id") != null) {
                        adjacents.Add(new Adjacent(xmlReader.GetAttribute("pano_id"), Double.Parse(xmlReader.GetAttribute("yaw_deg"))));
                    }
                } else if (xmlReader.Name.EndsWith("street_range") == true) {
                    pano_yaw_deg = double.Parse(xmlReader.ReadString());
                }
            }
        }



        //=========================================
        // panoid 를 구함
        //=========================================
        private String GetPanoID(double lat, double lng) {
            String pano_id = String.Empty;
            XmlTextReader reader = GetUmlXmlPanoID(lat, lng);

            while (reader.Read()) {
                if (reader.Name.Equals("data_properties") == true) {
                    if (reader.GetAttribute("pano_id") != null) {
                        pano_id = reader.GetAttribute("pano_id");
                    }
                }
            }

            return pano_id;
        }




        //=========================================
        // GetPanoID(double lat, double lng)의 캐쉬
        //=========================================
        private XmlTextReader GetUmlXmlPanoID(double lat, double lng) {
            if (File.Exists(Property.XmlPath + lat + lng) == false) {
                try {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(Property.XML_PANOID + "ll=" + lat + "%2C" + lng, Property.XmlPath + lat + lng);
                    wc.Dispose();
                } catch (Exception) {
                    throw new CanNotDownLoadXMLException(Property.XML_PANOID + "ll=" + lat + "%2C" + lng);
                }
            }
            return new XmlTextReader(Property.XmlPath + lat + lng);
        }




        //=========================================
        // updateInfo()의 캐쉬
        //=========================================
        private XmlTextReader GetXmlInfo(String panoID) {
            if (File.Exists(Property.XmlPath + panoID) == false) {
                try {
                    WebClient wc = new WebClient();
                    wc.DownloadFile(Property.XML_INFO + panoID, Property.XmlPath + panoID);
                    wc.Dispose();
                } catch (Exception) {
                    throw new CanNotDownLoadXMLException(Property.XML_INFO + panoID);
                }
            }
            return new XmlTextReader(Property.XmlPath + panoID);
        }
    }



    [Serializable]
    public class CanNotDownLoadXMLException : ApplicationException {
        public CanNotDownLoadXMLException() {
        }

        public CanNotDownLoadXMLException(string message)
            : base(message) {
        }
    }




    //##############################################
    // Class WorldState
    //##############################################
    //public class WorldState
    //{
    //    public double rollDeg = 0;
    //    public double pitchDeg = 0;
    //    public double yawDeg = 0;

    //    public WorldState() {
    //        rollDeg = pitchDeg = yawDeg = 0;
    //    }


    //    public WorldState(double init_roll, double init_pitch, double init_yaw) {
    //        roll(init_roll);
    //        pitch(init_pitch);
    //        yaw(init_yaw);
    //    }


    //    public void roll(double deg) {
    //        rollDeg = deg;
    //    }

    //    public void pitch(double deg) {
    //        pitchDeg = deg;
    //    }

    //    public void yaw(double deg)
    //    {
    //        yawDeg += deg;
    //        if (yawDeg < 0) {
    //            yawDeg = 360 - yawDeg;
    //        }

    //        yawDeg %= 360;
    //    }
    //}





    //##############################################
    // Class Adjacent
    //##############################################
    public class Adjacent {
        public String panoID;
        public double direction_deg;

        public Adjacent(String id, double deg) {
            panoID = id;
            direction_deg = deg;
        }

    }

}
