using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using System.Reflection.Emit;

namespace ConsoleApp5
{
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
        public List<Album> GetAlbums()
        {
            List<Album> albums = new List<Album>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectAlbums = "SELECT * FROM Albums";

                using (var command = new SQLiteCommand(selectAlbums, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            albums.Add(new Album
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = reader["Title"].ToString(),
                                ArtistID = Convert.ToInt32(reader["ArtistID"])
                            });
                        }
                    }
                }
            }

            return albums;
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
        public List<Album> SearchAlbums(string query)
        {
            List<Album> albums = new List<Album>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string searchAlbums = @"
                            SELECT Albums.ID AS AlbumID, Albums.Title AS AlbumTitle, Albums.ArtistID, Artists.ID AS ArtistID, Artists.Name AS ArtistName
                            FROM Albums
                            JOIN Artists ON Albums.ArtistID = Artists.ID
                            WHERE Albums.Title LIKE @Query";

                using (var command = new SQLiteCommand(searchAlbums, connection))
                {
                    command.Parameters.AddWithValue("@Query", $"%{query}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            albums.Add(new Album
                            {
                                ID = Convert.ToInt32(reader["AlbumID"]),
                                Title = reader["AlbumTitle"].ToString(),
                                ArtistID = Convert.ToInt32(reader["ArtistID"]),
                                ArtistName = reader["ArtistName"].ToString()
                            });
                        }
                    }
                }
            }
            if (albums.Count == 0)
            {
                return null;
            }
            else return albums;
        }

        public List<Song> GetSongsByGenre(string genre)
        {
            List<Song> songs = new List<Song>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string selectSongsByGenre = @"
                SELECT
                    Albums.ID AS AlbumID,
                    Albums.Title AS AlbumTitle,
                    Artists.ID AS ArtistID,
                    Artists.Name AS ArtistName,
                    Songs.ID AS SongID,
                    Songs.Title AS SongTitle,
                    Songs.Genre
                FROM
                    Albums
                JOIN
                    Artists ON Albums.ArtistID = Artists.ID
                JOIN
                    Songs ON Albums.ID = Songs.AlbumID
                WHERE
                    Songs.Genre COLLATE NOCASE LIKE @Genre";

                using (var command = new SQLiteCommand(selectSongsByGenre, connection))
                {
                    command.Parameters.AddWithValue("@Genre", $"%{genre}%");

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            songs.Add(new Song
                            {
                                ID = Convert.ToInt32(reader["SongID"]),
                                Title = reader["SongTitle"].ToString(),
                                AlbumID = Convert.ToInt32(reader["AlbumID"]),
                                Genre = reader["Genre"].ToString(),
                                AlbumName = reader["AlbumTitle"].ToString(),
                                ArtistName = reader["ArtistName"].ToString()
                            });
                        }
                    }
                }
            }

            if (songs.Count == 0)
            {
                return null;
            }
            else
            {
                return songs;
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
}
