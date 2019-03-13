using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace BugsBugs
{
    public static class RegiManager
    {
        private enum BITRATE { _128, _192, _320 };
        private static readonly string REGISTRY = "Software\\BugsBugs";
        private static readonly string DOWNLOADKEY = "DownLoad";
        private static readonly string CREATEFORALBUM = "bAlbum";
        private static readonly string CREATEFORTOP100 = "bTOP100";
        private static readonly string ALERTDUPLE = "bAlert";
        private static readonly string BITRATE = "bitrate";



        //-------------------------------------------------------
        // 다운로드 폴더
        //-------------------------------------------------------
        public static string ReadSavePath() {
            RegistryKey rKey = Registry.CurrentUser.OpenSubKey(bugsDownloader.registryPath, true);
            // BugsBugs 서브키가 없으면 생성
            if (rKey == null) {
                rKey = Registry.CurrentUser.CreateSubKey(REGISTRY);
            }

            return rKey.GetValue(DOWNLOADKEY) as string;
        }



        public static string SetSavePath(string path) {
            if (path.EndsWith("\\") == false) {
                path += "\\";
            }
            RegistryKey rKey = Registry.CurrentUser.OpenSubKey(REGISTRY, true);
            if (rKey == null) {
                rKey = Registry.CurrentUser.CreateSubKey(REGISTRY);
            }
            rKey.SetValue(DOWNLOADKEY, path);

            return path;
        }



        //-------------------------------------------------------
        // ALBUM 관련 폴더생성
        //-------------------------------------------------------
        public static bool ReadCreateFolderPolicyForAlbum() {
            
        }

        public static void SetCreateFolderPolicyForAlbum(bool flag) {
            if (path.EndsWith("\\") == false) {
                path += "\\";
            }
            RegistryKey rKey = Registry.CurrentUser.OpenSubKey(REGISTRY, true);
            if (rKey == null) {
                rKey = Registry.CurrentUser.CreateSubKey(REGISTRY);
            }
            string value = (flag == true) ? "yes" : "no";
            rKey.SetValue(CREATEFORALBUM, save);

            return path;
        }



        //-------------------------------------------------------
        // TOP100 관련 폴더생성
        //-------------------------------------------------------
        public static bool ReadCreateFolderPolicyForTop100() {
            //
        }

        public static void SetCreateFolderPolicyForTop100(bool flag) {
            if (path.EndsWith("\\") == false) {
                path += "\\";
            }
            RegistryKey rKey = Registry.CurrentUser.OpenSubKey(REGISTRY, true);
            if (rKey == null) {
                rKey = Registry.CurrentUser.CreateSubKey(REGISTRY);
            }
            string value = (flag == true) ? "yes" : "no";
            rKey.SetValue(CREATEFORTOP100, save);

            return path;
        }



        //-------------------------------------------------------
        // 중복알림
        //-------------------------------------------------------
        public static bool ReadPolicyForDuple() {
            //
        }

        public static void SetPolicyForDuple(bool falg) {
            if (path.EndsWith("\\") == false) {
                path += "\\";
            }
            RegistryKey rKey = Registry.CurrentUser.OpenSubKey(REGISTRY, true);
            if (rKey == null) {
                rKey = Registry.CurrentUser.CreateSubKey(REGISTRY);
            }
            string value = (flag == true) ? "yes" : "no";
            rKey.SetValue(ALERTDUPLE, save);

            return path;
        }



        //-------------------------------------------------------
        // 음질
        //-------------------------------------------------------
        public static BITRATE ReadBitRateSetting() {
        }

        public static void SetBitRate(BITRATE bitrate) {
            if (path.EndsWith("\\") == false) {
                path += "\\";
            }
            RegistryKey rKey = Registry.CurrentUser.OpenSubKey(REGISTRY, true);
            if (rKey == null) {
                rKey = Registry.CurrentUser.CreateSubKey(REGISTRY);
            }
            string value = bitrate.ToString();
            rKey.SetValue(BITRATE, value);

            return path;
        }
    }
}
