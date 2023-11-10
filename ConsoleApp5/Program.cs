using ConsoleApp5;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

class Program
{
    static void Main(string[] args)
    {
        string dbFilePath = "music_catalog.db";

        MusicDatabase catalog = new MusicDatabase(dbFilePath);

        catalog.CreateTables();

        bool online = true;
        
        while (online) 
        {
            catalog.Footer();
            string key = (Console.ReadLine());
            string DataSearch;
            Console.Clear();
            switch (key)
            {
                case "1":
                    catalog.PrintAllArtists();
                    Console.ReadKey();
                    Console.Clear();
                    break;
                case "2":
                    Console.WriteLine("Введите имя автора для поиска");
                    DataSearch = Console.ReadLine();
                    List<Artist> artistResults = catalog.SearchArtists(DataSearch);
                    Console.WriteLine("Результат поиска:");
                    if(artistResults == null) 
                    {
                        Console.Write("Ничего не найдено");
                    }
                    else
                    {
                        foreach (var artist in artistResults)
                        {
                            Console.WriteLine($"ID: {artist.ID}, Имя: {artist.Name}");
                        }
                    }
                    Console.ReadKey();
                    Console.Clear();
                    DataSearch = null;
                    break;
                case "3":
                    Console.WriteLine("Введите название для поиска");
                    DataSearch = Console.ReadLine();
                    List<Album> albumResults = catalog.SearchAlbums(DataSearch);
                    Console.WriteLine("Результат поиска:");
                    if (albumResults == null)
                    {
                        Console.Write("Ничего не найдено");
                    }
                    else
                    {
                        foreach (var albums in albumResults)
                        {
                            Console.WriteLine($"ID: {albums.ID}, Название: {albums.Title}, Исполнитель: {albums.ArtistName}");
                        }
                    }
                    Console.ReadKey();
                    Console.Clear();
                    DataSearch = null;
                    break;
                case "4":

                    break;
                case null:
                    catalog.Error();
                    break;
                case "":
                    catalog.Error();
                    break;

                case "0":
                    online = false;
                    break;
                default:
                    catalog.Error();
                    break;
            }
        }
    }
}
