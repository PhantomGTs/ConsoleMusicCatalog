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
                    Console.WriteLine("Нажми ENTER чтобы показать все песни.");
                    Console.WriteLine("Введите имя автора для поиска:");
                    DataSearch = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(DataSearch))
                    {
                        Console.WriteLine("Вы ввели пустую строку. Повторите ввод.");
                    }
                    else
                    {
                        DataSearch = char.ToUpper(DataSearch[0]) + DataSearch.Substring(1).ToLower();
                        List<Artist> artistResults = catalog.SearchArtists(DataSearch);
                        Console.WriteLine("Результат поиска:");
                        if (artistResults == null)
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
                    }
                    Console.ReadKey();
                    Console.Clear();
                    DataSearch = null;
                    break;
                case "3":
                    Console.WriteLine("Нажми ENTER чтобы показать все песни.");
                    Console.WriteLine("Введите название песни для поиска:");
                    DataSearch = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(DataSearch))
                    {
                        Console.WriteLine("Вы ввели пустую строку. Повторите ввод.");
                    }
                    else
                    {
                        DataSearch = char.ToUpper(DataSearch[0]) + DataSearch.Substring(1).ToLower();
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
                    }
                    Console.ReadKey();
                    Console.Clear();
                    DataSearch = null;
                    break;
                case "4":
                    Console.WriteLine("Нажми ENTER чтобы показать все песни.");
                    Console.WriteLine("Введите жанр для поиска:");
                    DataSearch = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(DataSearch))
                    {
                        Console.WriteLine("Вы ввели пустую строку. Повторите ввод.");
                    }
                    else
                    {
                        DataSearch = char.ToUpper(DataSearch[0]) + DataSearch.Substring(1).ToLower();
                        List<Song> SongResults = catalog.GetSongsByGenre(DataSearch);
                        Console.WriteLine("Результат поиска:");

                        if (SongResults == null)
                        {
                            Console.Write("Ничего не найдено");
                        }
                        else
                        {
                            foreach (var songs in SongResults)
                            {
                                Console.WriteLine($"ID: {songs.ID}, Название: {songs.Title}, Исполнитель: {songs.ArtistName}, Альбом: {songs.AlbumName}, Жанр: {songs.Genre}");
                            }
                        }
                    }

                    
                    Console.ReadKey();
                    Console.Clear();
                    DataSearch = null;
                    break;
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
