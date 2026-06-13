using Microsoft.Data.SqlClient;
using ExternalApiCache.Models;

namespace ExternalApiCache.Repositories;

public class PostRepository
{
    private readonly string _connectionString;

    public PostRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("Connection string not found.");
    }

    public async Task<Post?> GetPostByIdAsync(int id)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = @"SELECT Id, UserId, Title, Body 
                           FROM Posts 
                           WHERE Id = @Id";

            using SqlCommand cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return new Post
            {
                Id = reader.GetInt32(0),
                UserId = reader.GetInt32(1),
                Title = reader.GetString(2),
                Body = reader.GetString(3)
            };
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error while fetching post by id.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Unexpected error while fetching post by id.", ex);
        }
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        try
        {
            List<Post> posts = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = @"SELECT Id, UserId, Title, Body FROM Posts";

            using SqlCommand cmd = new SqlCommand(sql, connection);

            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                posts.Add(new Post
                {
                    Id = reader.GetInt32(0),
                    UserId = reader.GetInt32(1),
                    Title = reader.GetString(2),
                    Body = reader.GetString(3)
                });
            }

            return posts;
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error while fetching all posts.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Unexpected error while fetching all posts.", ex);
        }
    }

    public async Task InsertPostAsync(Post post)
    {
        try
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            string sql = @"INSERT INTO Posts (Id, UserId, Title, Body)
                           VALUES (@Id, @UserId, @Title, @Body)";

            using SqlCommand cmd = new SqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@Id", post.Id);
            cmd.Parameters.AddWithValue("@UserId", post.UserId);
            cmd.Parameters.AddWithValue("@Title", post.Title);
            cmd.Parameters.AddWithValue("@Body", post.Body);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (SqlException ex)
        {
            throw new Exception("Database error while inserting post.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Unexpected error while inserting post.", ex);
        }
    }
}