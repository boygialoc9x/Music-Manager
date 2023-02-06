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
        public List<Categories> Catogories {get; set;}
        public List<Artists> Artists {get; set;}

    }
}
