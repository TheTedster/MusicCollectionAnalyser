using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MusicCollectionAnalyser
{
    class Program
    {
        static void Main(string[] args)
        {
            var musicDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
            ResultSort resultSort = ResultSort.ByAlbumCount;

            foreach (var arg in args) //Environment.GetCommandLineArgs())
            {
                if (arg.Trim().StartsWith("alpha", StringComparison.CurrentCultureIgnoreCase))
                {
                    resultSort = ResultSort.ByArtistName;
                }
                else
                {
                    try
                    {
                        if (Path.GetFullPath(arg) != "")
                        {
                            musicDir = new DirectoryInfo(arg);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid path specified: {0}", arg);
                        return;
                    }
                }
            }
            
            if (musicDir.Exists)
            {
                var artistAlbums = new Dictionary<String, int>();

                var artistDirs = musicDir.GetDirectories();
                var longestArtistName = 0;
                foreach (var artistDir in artistDirs)
                {                    
                    artistAlbums.Add(artistDir.Name, artistDir.EnumerateDirectories().Count());
                    if (artistDir.Name.Length > longestArtistName) longestArtistName = artistDir.Name.Length;
                }

                if(artistAlbums.Count == 0)
                {
                    Console.WriteLine("No artist directories found in {0}", musicDir.FullName);
                }
                else
                {
                    foreach (var aa in resultSort == ResultSort.ByAlbumCount ? artistAlbums.OrderByDescending(aa => aa.Value).ThenBy(aa => aa.Key) : artistAlbums.OrderBy(aa => aa.Key))
                    {
                        Console.WriteLine("{0,-" + (longestArtistName) + "}{1,10}", aa.Key, aa.Value);
                    }
                    Console.WriteLine("{0,-" + (longestArtistName) + "}{1,10}", "TOTAL ALBUMS:", artistAlbums.Count);
                }                
            }
            else
            {
                Console.WriteLine("Folder '{0}' not found / acccessible", musicDir.FullName);
            }
            Console.ReadKey();            
        }

        private enum ResultSort
        {
            ByAlbumCount,
            ByArtistName
        }
    }
}
