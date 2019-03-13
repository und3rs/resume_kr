using System;
using System.Collections.Generic;
using System.Text;

namespace BugsBugs
{
    public class Music
    {
        private string _title;
        private string _singer;
        private string _id;

        public string title {
            get { return _title; }
        }

        public string singer {
            get { return _singer; }
        }

        public string id {
            get { return _id; }
        }



        //-------------------------------------------------------
        // 생성자
        //-------------------------------------------------------
        public Music(string title, string singer, string id) {
            _title = title;
            _singer = singer;
            _id = id;
        }
    }
}
