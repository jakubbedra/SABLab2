using System.Text;
using Blog.DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TDD.DbTestHelpers.EF;
using YamlDotNet.Serialization;

namespace TDD.DbTestHelpers.Helpers;

public class FileHelper
{
    private Dictionary<long, long> _postIdsInFileToIdsInDb = new Dictionary<long, long>();

    public void ClearTables<TFixtureType>(DbContext context)
    {
        ClearTables(typeof(TFixtureType), context);
    }

    public void ClearTables(Type fixtureType, DbContext context)
    {
        foreach (var fixtureTable in fixtureType.GetProperties())
        {
            var table = context.GetType().GetProperty(fixtureTable.Name);
            var tableType = table.PropertyType;
            var clearTableMethod = typeof(EfExtensions).GetMethod("ClearTable")
                .MakeGenericMethod(tableType.GetGenericArguments());
            clearTableMethod.Invoke(null, new[] { table.GetValue(context, null) });
        }

        context.SaveChanges();
    }

    public void FillFixturesFileFiles<TFixtureType>(DbContext context, string yamlFolderName,
        IEnumerable<string> yamlFilesNames)
    {
        FillFixturesFileFiles(typeof(TFixtureType), context, yamlFolderName, yamlFilesNames);
    }

    public void FillFixturesFileFiles(Type fixtureType, DbContext context, string yamlFolderName,
        IEnumerable<string> yamlFullFilesNames)
    {
        Dictionary<object, object> fixtures = (Dictionary<object, object>)GetFixutresFromYaml(fixtureType, yamlFolderName, yamlFullFilesNames);
        foreach (var fixtureTable in fixtures)
        {
            Dictionary<object, object> table = (Dictionary<object, object>)fixtureTable.Value;
            if (table == null) throw new Exception("Cannot read entities from table " + (string)fixtureTable.Key);
            foreach (var entity in table.Values)
            {
                var dbSetType = context.GetType().GetProperty((string)fixtureTable.Key);
                if (dbSetType == null)
                    throw new Exception(string.Format("Cannot find table {0} in database", (string)fixtureTable.Key));
                var dbSet = dbSetType.GetValue(context, null);
                if (fixtureTable.Key.Equals("Posts"))
                {
                    Post post = new Post()
                    {
                        // Id = long.Parse((string)((Dictionary<object, object>)entity)["Id"]),
                        Author = (string)((Dictionary<object, object>)entity)["Author"],
                        Content = (string)((Dictionary<object, object>)entity)["Content"],
                    };
                    var makeGenericType = typeof(DbSet<>).MakeGenericType(post.GetType());
                    var methodInfo = makeGenericType.GetMethod("Add");
                    Post addedPost = ((EntityEntry<Post>)methodInfo.Invoke(dbSet, new[] { post })).Entity;
                    context.SaveChanges();
                    _postIdsInFileToIdsInDb.Add(long.Parse((string)((Dictionary<object, object>)entity)["Id"]), addedPost.Id);
                }
                else if (fixtureTable.Key.Equals("Comments"))
                {
                    Comment comment = new Comment()
                    {
                        Content = (string)((Dictionary<object, object>)entity)["Content"],
                        PostId = _postIdsInFileToIdsInDb[long.Parse((string)((Dictionary<object, object>)entity)["PostId"])],
                    };
                    var makeGenericType = typeof(DbSet<>).MakeGenericType(comment.GetType());
                    var methodInfo = makeGenericType.GetMethod("Add");
                    methodInfo.Invoke(dbSet, new[] { comment });
                    context.SaveChanges();
                }
            }
        }

    }

    private void AddEntityToDb(DbSet<object> dbSet, Dictionary<string, string> entity)
    {
        Type type = dbSet.GetType().GetGenericArguments()[0];
    }

    private object GetFixutresFromYaml(Type fixtureType, string yamlFolderName, IEnumerable<string> yamlFullFilesNames)
    {
        IDeserializer yamlDeserializer = new DeserializerBuilder().Build();
        try
        {
            return yamlDeserializer.Deserialize(GetAllYamlConfiguration(yamlFullFilesNames, yamlFolderName));
        }
        catch (Exception ex)
        {
            throw new Exception("Cannot deserialize YAML file. See inner exception.", ex);
        }
    }

    private static TextReader GetAllYamlConfiguration(IEnumerable<string> yamlFilesNames, string yamlFolderName)
    {
        var sb = new StringBuilder();
        foreach (var yamlFileName in yamlFilesNames)
        {
            var yamlPath = Path.Combine(yamlFolderName, yamlFileName);
            if (!File.Exists(yamlPath))
                throw new Exception(String.Format("Specified file {0} does not exist in specifiled folder {1}",
                    yamlFileName, yamlFolderName));
            sb.AppendLine(File.ReadAllText(yamlPath));
        }

        return new StringReader(sb.ToString());
    }
}