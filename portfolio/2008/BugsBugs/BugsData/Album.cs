using System;
using System.Collections.Generic;
using System.Text;

namespace BugsBugs
{
    public class Album
    {
        private string _title;
        private string _artist;
        private string _id;

        private List<Music> musics;
        private string title {
            get { return _title; }
        }
        private string artist {
            get { return _title; }
        }
        private string id {
            get { return _id; }
        }

        public int Count {
            get { return musics.Count; }
        }



        //-------------------------------------------------------
        // 생성자
        //-------------------------------------------------------
        public Album() {
            musics = new List<Music>();
        }

        public Album(string title, string artist, string id) {
            _title = title;
            _artist = artist;
            _id = id;
            musics = new List<Music>();
        }



        //-------------------------------------------------------
        // 앨범에 음악 추가
        //-------------------------------------------------------
        public int AddMusic(Music music) {
            musics.Add(music);
            return Count;
        }
    }
}
