using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;


namespace BugsBugs
{
    public static class WebAgent
    {
        public enum SEARCHTYPE {ALBUM, MUSIC};

        // 앨범 검색 URL
        private static string albumSearchURL = "http://srch.bugs.co.kr/album/s_album.asp?s_kind=&useSuggest=true&nil_Search=btn&keyword=";
        private static string albumCodeSearchPoint = @"\('album', (\d)+\);"">(<i)";

        // Top 100
        private static string top100SearchURL = "http://music.bugs.co.kr/bugschart/bugstop100/todaytop100.asp?ct=62&cs=all&page=1&pagesize=100";
        private static string top100SearchPoint = @"(\w)*','n'\);";

        // 음악 검색 URL
        private static string musicSearchURL = "http://srch.bugs.co.kr/song/s_song.asp?keyword=";
        private static string musicDownEx = @"(\w)*','n'\);";

        // 앨범정보 URL
        private static string albumInfoURL = "http://music.bugs.co.kr/Info/album.asp?cat=Base&menu=m&Album=";
        private static string albumMusicEx = @"(\w)*','n'\);";



        //-------------------------------------------------------
        // 해당 키워드의 앨범 또는 곡을 검색
        //-------------------------------------------------------
        public static List<Album> Search(SEARCHTYPE type, string keyword) {
            if (type == SEARCHTYPE.ALBUM) {
                return SearchAlbum(keyword);
            } else if (type == SEARCHTYPE.MUSIC) {
                return SearchMusic(keyword);
            }
            return null;
        }



        //-------------------------------------------------------
        // TOP100 을 검색
        //-------------------------------------------------------
        public static List<Album> Top100() {
            return SearchTop100();
        }



        //-------------------------------------------------------
        // 웹에서 TOP100 검색
        //-------------------------------------------------------
        private static List<Album> SearchTop100() {
            List<Album> albums = new List<Album>();

            WebRequest request;
            HttpWebResponse response;
            Stream dataStream;
            StreamReader reader;

            request = WebRequest.Create(top100SearchURL);
            response = (HttpWebResponse)request.GetResponse();
            dataStream = response.GetResponseStream();
            reader = new StreamReader(dataStream, System.Text.Encoding.GetEncoding("euc-kr"));
            String html = reader.ReadToEnd();

            Regex r = new Regex(top100SearchPoint);
            Match m = r.Match(html);

            string music_id, music_singer, music_title, temp;
            int start;

            while (m.Success) {
                // 음악 id 값
                music_id = m.Value.Substring(0, m.Value.IndexOf("'"));

                // 음악제목
                temp = music_id + "','h');\" style=\"cursor:hand;\" title=\"듣기\">";
                start = html.IndexOf(temp) + temp.Length;
                music_title = html.Substring(start, html.IndexOf("</span>", start) - start);
                music_title = music_title.Replace("<b>", "");
                music_title = music_title.Replace("</b>", "");
                music_title = music_title.Trim();

                // 아티스트
                temp = "\"아티스트정보\">";
                start = html.IndexOf(temp, start) + temp.Length;
                music_singer = html.Substring(start, html.IndexOf("</span>", start) - start);
                music_singer = music_singer.Replace("<b>", "");
                music_singer = music_singer.Replace("</b>", "");
                music_singer = music_singer.Trim();

                // 추가
                Album album = new Album();
                album.AddMusic(new Music(music_title, music_singer, music_id));
                albums.Add(album);
                m = m.NextMatch();
            }

            // 스트림 종료
            reader.Close();
            dataStream.Close();
            response.Close();
            dataStream.Dispose();

            return albums;
        }



        //-------------------------------------------------------
        // 웹에서 앨범검색
        //-------------------------------------------------------
        private static List<Album> SearchAlbum(string keyword) {
            List<Album> albums = new List<Album>();

            WebRequest request;
            HttpWebResponse response;
            Stream dataStream;
            StreamReader reader;

            request = WebRequest.Create(albumSearchURL + System.Web.HttpUtility.UrlEncode(keyword, System.Text.Encoding.Default));
            response = (HttpWebResponse)request.GetResponse();
            dataStream = response.GetResponseStream();
            reader = new StreamReader(dataStream, System.Text.Encoding.GetEncoding("euc-kr"));
            String html = reader.ReadToEnd();

            Regex r = new Regex(albumCodeSearchPoint);
            Match m = r.Match(html);


            string album_id, album_title, album_artist, temp;
            int start;

            while (m.Success) {
                // 앨범 id 값
                album_id = m.Value.Substring(10, m.Value.IndexOf(")") - 10);

                // 앨범명
                temp = "pat3\"><a href=\"javascript:mnuSearch('album', " + album_id + ");\">";
                start = html.IndexOf(temp) + temp.Length;
                album_title = html.Substring(start, html.IndexOf("</a>", start) - start);
                album_title = album_title.Replace("<b>", "");
                album_title = album_title.Replace("</b>", "");
                album_title = album_title.Trim();

                // 아티스트
                temp = ");\">";
                start = html.IndexOf(temp, start) + temp.Length;
                album_artist = html.Substring(start, html.IndexOf("</a>", start) - start);
                album_artist = album_artist.Replace("<b>", "");
                album_artist = album_artist.Replace("</b>", "");
                album_artist = album_artist.Trim();

                // 추가
                Album album = new Album(album_title, album_artist, album_id);
                albums.Add(album);

                m = m.NextMatch();
            }

            // 스트림 종료
            reader.Close();
            dataStream.Close();
            response.Close();
            dataStream.Dispose();

            return albums;
        }



        //-------------------------------------------------------
        // 웹에서 곡 검색
        //-------------------------------------------------------
        private static List<Album> SearchMusic(string music) {
            List<Album> albums = new List<Album>();

            WebRequest request;
            HttpWebResponse response;
            Stream dataStream;
            StreamReader reader;

            request = WebRequest.Create(musicSearchURL + System.Web.HttpUtility.UrlEncode(music, System.Text.Encoding.Default));
            response = (HttpWebResponse)request.GetResponse();
            dataStream = response.GetResponseStream();
            reader = new StreamReader(dataStream, System.Text.Encoding.GetEncoding("euc-kr"));
            String html = reader.ReadToEnd();

            Regex r = new Regex(musicDownEx);
            Match m = r.Match(html);


            string music_id, music_title, music_singer, album_title, album_id, temp;
            int start;

            while (m.Success) {
                // 음악 id 값
                music_id = m.Value.Substring(0, m.Value.IndexOf("'"));

                // 음악 제목
                temp = "o('" + music_id + "','h');\" style=\"cursor:hand;\">";
                start = html.IndexOf(temp) + temp.Length;
                music_title = html.Substring(start, html.IndexOf("</span>", start) - start);
                music_title = music_title.Replace("<b>", "");
                music_title = music_title.Replace("</b>", "");
                music_title = music_title.Trim();

                // 가수
                temp = ");\">";
                start = html.IndexOf(temp, start) + temp.Length;
                music_singer = html.Substring(start, html.IndexOf("</span>", start) - start);
                music_singer = music_singer.Replace("<b>", "");
                music_singer = music_singer.Replace("</b>", "");
                music_singer = music_singer.Trim();

                // 앨범 id
                temp = "('album',";
                start = html.IndexOf(temp, start) + temp.Length;
                album_id = html.Substring(start, html.IndexOf(");\">", start) - start);

                // 앨범명
                temp = ");\">";
                start = html.IndexOf(temp, start) + temp.Length;
                album_title = html.Substring(start, html.IndexOf("</span>", start) - start);
                album_title = album_title.Replace("<b>", "");
                album_title = album_title.Replace("</b>", "");
                album_title = album_title.Trim();

                // 추가
                Album album = new Album(album_title, music_singer, album_id);
                album.AddMusic(new Music(music_title, music_singer, music_id));
                albums.Add(album);
                m = m.NextMatch();
            }

            // 스트림 종료
            reader.Close();
            dataStream.Close();
            response.Close();

            return albums;
        }
    }
}
