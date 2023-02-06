using System;
using System.Collections.Generic;

namespace Persistence
{
    public class Song
    {
        public int songId {set;get;}
        public string songName {set;get;}
        //public string aritst {set;get;}
        public int length {set;get;}
        public string lyric {set;get;}
        public string downloadLink {set;get;}
        public bool songStatus{set;get;}
        public List<Genres> genres {get; set;}
        public DateTime releaseDate {get;set;}
        public string album {get;set;}
        public string copyright {get;set;}
        public List<Artists> singer {get; set;}
        public List<Artists> band {set;get;}
        public List<Artists> writer {get;set;}
        public List<Artists> produced {get;set;}

    }
}
