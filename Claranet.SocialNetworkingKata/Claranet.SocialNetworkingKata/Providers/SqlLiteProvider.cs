using Claranet.SocialNetworkingKata.Entities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Claranet.SocialNetworkingKata.Providers
{
    class SqlLiteProvider : IStorageProvider
    {
        public static readonly string SqlLiteDatabasePath = $"{Environment.CurrentDirectory}\\social.db";

        private FileInfo File
        {
            get
            {
                return new FileInfo(SqlLiteDatabasePath);
            }
        }

        private string ConnectionString
        {
            get { return $"Data Source={this.File.FullName}"; }
        }

        public async Task InitializeIfRequired()
        {
            if (!this.File.Exists)
            {
                SQLiteConnection.CreateFile(this.File.FullName);

                using (var connection = new SQLiteConnection(this.ConnectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"CREATE TABLE Posts(Id INTEGER PRIMARY KEY AUTOINCREMENT, Author TEXT, Message TEXT, Time DATE)";
                        await command.ExecuteNonQueryAsync();

                        command.CommandText = @"CREATE TABLE FollowedUsers(User TEXT, Followed TEXT)";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task AddFollowerToUser(string user, string userToFollow)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = 
                        $"SELECT COUNT(*) " +
                        $"FROM FollowedUsers " +
                        $"WHERE lower(User) = lower(@user) AND lower(Followed) = lower(@followed)";
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@followed", userToFollow);
                    var result = (long)(await command.ExecuteScalarAsync());

                    if (result == 0)
                    {
                        command.CommandText = @"INSERT INTO FollowedUsers (User, Followed) VALUES (@user, @followed)";
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public async Task AddMessageForUser(string author, string message, DateTime time)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"INSERT INTO Posts (Author, Message, Time) VALUES (@author, @message, @time)";
                    command.Parameters.AddWithValue("@author", author);
                    command.Parameters.AddWithValue("@message", message);
                    command.Parameters.AddWithValue("@time", time);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<IEnumerable<Post>> GetMessagesByUser(string user)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = @"SELECT * FROM Posts WHERE lower(Author) = lower(@user) ORDER BY TIME DESC";
                    command.Parameters.AddWithValue("@user", user);
                    var reader = (await command.ExecuteReaderAsync());

                    IList<Post> posts = new List<Post>();

                    while (await reader.ReadAsync())
                    {
                        posts.Add(new Post
                        {
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            Message = reader.GetString(reader.GetOrdinal("Message")),
                            Time = reader.GetDateTime(reader.GetOrdinal("Time"))
                        });
                    }

                    return posts.ToArray();
                }
            }
        }

        public async Task<IEnumerable<Post>> GetWallByUser(string user)
        {
            using (var connection = new SQLiteConnection(this.ConnectionString))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = 
                        $"SELECT P.* FROM Posts P " +
                        $"INNER JOIN FollowedUsers F ON P.Author = F.Followed " +
                        $"WHERE lower(F.User) = lower(@user) " +
                        $"UNION " +
                        $"SELECT Posts.* FROM Posts WHERE lower(Author) = lower(@user) " +
                        $"ORDER BY TIME DESC";
                    command.Parameters.AddWithValue("@user", user);
                    var reader = (await command.ExecuteReaderAsync());

                    IList<Post> posts = new List<Post>();

                    while (await reader.ReadAsync())
                    {
                        posts.Add(new Post
                        {
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            Message = reader.GetString(reader.GetOrdinal("Message")),
                            Time = reader.GetDateTime(reader.GetOrdinal("Time"))
                        });
                    }

                    return posts.ToArray();
                }
            }
        }
    }
}
