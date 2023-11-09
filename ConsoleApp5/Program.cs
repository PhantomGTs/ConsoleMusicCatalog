using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

class Artist
{
    public int ID {get; set;}
    public string Name {get; set;}
}


class Album
{
    public int ID {get; set;}
    public string Title {get; set;}
    public int ArtistID {get; set;}
}

class Song
{
    public int ID {get; set;}
    public string Title {get; set;}
    public int AlbumID {get; set;}
    public string Genre {get; set;}
}

class MusicDatabase
{
    private string connectionString;

    public MusicDatabase(string dbFilePath)
    {
        connectionString = $"Data Source={dbFilePath};Version=3;";
    }

    public void CreateTables()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string createArtistsTable = "CREATE TABLE IF NOT EXISTS Artists (ID INTEGER PRIMARY KEY, Name TEXT)";
            string createAlbumsTable = "CREATE TABLE IF NOT EXISTS Albums (ID INTEGER PRIMARY KEY, Title TEXT, ArtistID INTEGER)";
            string createSongsTable = "CREATE TABLE IF NOT EXISTS Songs (ID INTEGER PRIMARY KEY, Title TEXT, AlbumID INTEGER, Genre TEXT)";

            using (var command = new SQLiteCommand(createArtistsTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(createAlbumsTable, connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(createSongsTable, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }



    public List<Artist> SearchArtists(string query)
    {
        List<Artist> artists = new List<Artist>();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string searchArtists = "SELECT * FROM Artists WHERE Name LIKE @Query";

            using (var command = new SQLiteCommand(searchArtists, connection))
            {
                command.Parameters.AddWithValue("@Query", $"%{query}%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        artists.Add(new Artist
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString()
                        });
                    }
                }
            }
        }
        if (artists.Count == 0)
        {
            return null; 
        }
        else return artists;
    }


    public List<Artist> GetAllArtists()
    {
        List<Artist> artists = new List<Artist>();

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string selectArtists = "SELECT * FROM Artists";

            using (var command = new SQLiteCommand(selectArtists, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        artists.Add(new Artist
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString()
                        });
                    }
                }
            }
        }

        return artists;
    }

    public void PrintAllArtists()
    {
        List<Artist> allArtists = GetAllArtists();

        Console.WriteLine("Все артисты:");
        foreach (var artist in allArtists)
        {
            Console.WriteLine($"ID: {artist.ID}, Артист: {artist.Name}");
        }
    }

    public void Footer()
    {
        Console.WriteLine("МУЗЫКАЛЬНЫЙ КАТАЛОГ");
        Console.WriteLine("1 - Вывести всех артистов");
        Console.WriteLine("2 - Поиск музыкантов");
        Console.WriteLine("3 - Поиск альбомов");
        Console.WriteLine("4 - Поиск по жанрам");
        Console.WriteLine("0 - Завершить работу");
        Console.Write("Введите ключ команду:");
    }

    public void Error() 
    {
        Console.Clear();
        Console.WriteLine("Ключ команда введена неправильно!");
        Console.WriteLine("Нажмите ENTER чтобы продолжить");
        Console.ReadKey();
        Console.Clear();
    }

}

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
