using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    class Artist
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }


    class Album
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int ArtistID { get; set; }
        public string ArtistName { get; set;}
    }

    class Song
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int AlbumID { get; set; }
        public string Genre { get; set; }
        public string AlbumName { get; set; }
        public string ArtistName { get; set;}
}
}
