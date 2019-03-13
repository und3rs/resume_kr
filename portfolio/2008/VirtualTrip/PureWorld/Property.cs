using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PureWorldLib {
    public static class Property {
        private static string iniFile = System.IO.Directory.GetCurrentDirectory() + "\\prwdlib.ini";
        private static string section_1 = "URL";
        private static string section_2 = "XML";
        private static string section_3 = "File Path";
        private static string section_4 = "Panorama";

        //////////////////////////////////////
        // Section 1 - URL
        //////////////////////////////////////
        
        // http://cbk0.google.com/cbk?
        public static string URL_DOWN {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return ini.ReadValue(section_1, "DownLoad", "http://cbk0.google.com/cbk?");
            }
        }


        
        //////////////////////////////////////
        // Section 2 - XML
        //////////////////////////////////////
        
        // http://maps.google.com/cbk?output=xml&cb_client=maps_sv&panoid=
        public static string XML_INFO {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return ini.ReadValue(section_2, "PanoInfo", "http://maps.google.com/cbk?output=xml&cb_client=maps_sv&panoid=");
            }
        }

        // http://maps.google.com/cbk?output=xml&cb_client=maps_sv_ta&radius=1024&
        public static string XML_PANOID {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return ini.ReadValue(section_2, "ObtainPanaID", "http://maps.google.com/cbk?output=xml&cb_client=maps_sv_ta&radius=1024&");
            }
        }

        //////////////////////////////////////
        // Section 3 - File Path
        //////////////////////////////////////
        
        // .\\worlds\\
        public static string WorldPath {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return ini.ReadValue(section_3, "WorldPath", ".\\worlds\\");
            }
        }

        // .\\xmls\\
        public static string XmlPath {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return ini.ReadValue(section_3, "XMLPath", ".\\xmls\\");
            }
        }

        // .\\worlds\\temp\\
        public static string TempPath {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return ini.ReadValue(section_3, "TempPath", ".\\worlds\\temp\\");
            }
        }

        
        

        //////////////////////////////////////
        // Section 4 - Panorama
        //////////////////////////////////////
        // HORIZON_IMG_NUM = 7;   // 파노라마 이미지는 총 7장으로 나뉘어 있습니다.
        public static int HORIZON_IMG_NUM {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return Int32.Parse(ini.ReadValue(section_4, "HorizonImageNum", "7"));
            }
        }

        // VERTICAL_IMG_NUM = 4;
        public static int VERTICAL_IMG_NUM {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return Int32.Parse(ini.ReadValue(section_4, "VerticalImageNum", "4"));
            }
        }

        // image_width = 3328;
        public static int image_width {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return Int32.Parse(ini.ReadValue(section_4, "ImageWidth", "3328"));
            }
        }

        // image_height = 1664;
        public static int image_height {
            get {
                INIController.IniCntl ini = new INIController.IniCntl(iniFile);
                return Int32.Parse(ini.ReadValue(section_4, "ImageHeight", "1664"));
            }
        }
    }
}
