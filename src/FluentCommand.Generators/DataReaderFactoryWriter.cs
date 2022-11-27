
using System.Collections.Immutable;

namespace FluentCommand.Generators;

public static class DataReaderFactoryWriter
{
    public static string Generate(EntityClass entityClass)
    {
        var codeBuilder = new IndentedStringBuilder();
        codeBuilder
            .AppendLine("// <auto-generated>")
            .AppendLine("//     Generated by the FluentCommand source generator.")
            .AppendLine("// </auto-generated>")
            .AppendLine()
            .AppendLine("using global::FluentCommand.Extensions;")
            .AppendLine();

        codeBuilder
            .Append("namespace ")
            .AppendLine(entityClass.EntityNamespace)
            .AppendLine("{")
            .IncrementIndent();

        codeBuilder
            .AppendLine("/// <summary>")
            .AppendLine("/// Extension methods for FluentCommand")
            .AppendLine("/// </summary>");

        codeBuilder
            .Append("[global::System.CodeDom.Compiler.GeneratedCode(\"")
            .Append(ThisAssembly.Project.AssemblyName)
            .Append("\", \"")
            .Append(ThisAssembly.Info.Version)
            .AppendLine("\")]");

        codeBuilder
            .AppendLine("[global::System.Diagnostics.DebuggerNonUserCodeAttribute]")
            .AppendLine("[global::System.Diagnostics.DebuggerStepThroughAttribute]")
            .Append("public static partial class ")
            .Append(entityClass.EntityName)
            .AppendLine("DataReaderFactoryExtensions")
            .AppendLine("{")
            .IncrementIndent();

        WriteQueryEntity(codeBuilder, entityClass);

        WriteQuerySingleEntity(codeBuilder, entityClass);

        WriteQueryEntityTask(codeBuilder, entityClass);

        WriteQuerySingleEntityTask(codeBuilder, entityClass);

        WriteEntityFactory(codeBuilder, entityClass);

        codeBuilder
            .DecrementIndent()
            .AppendLine("}") // class
            .DecrementIndent()
            .AppendLine("}"); // namespace

        return codeBuilder.ToString();
    }

    private static void WriteQuerySingleEntityTask(IndentedStringBuilder codeBuilder, EntityClass entity)
    {
        // public static Task<Entity> QuerySingleEntityAsync(this IDataCommand dataCommand) => QuerySingleAsync(dataCommand, EntityFactory);
        codeBuilder
            .AppendLine("/// <summary>")
            .Append("/// Executes the query and returns the first row in the result as a <see cref=\"T:")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/> object.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"dataCommand\">The <see cref=\"T:FluentCommand.IDataCommand\"/> for this extension method.</param>")
            .AppendLine("/// <returns>")
            .Append("/// A instance of <see cref=\"")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/>  if row exists; otherwise null.")
            .AppendLine("/// </returns>")
            .Append("public static global::System.Threading.Tasks.Task<")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .Append("> QuerySingle")
            .Append(entity.EntityName)
            .AppendLine("Async(this global::FluentCommand.IDataCommand dataCommand)")
            .IncrementIndent()
            .Append("=> global::FluentCommand.Internal.DataSequentialReader.QuerySingleAsync(dataCommand, ")
            .Append(entity.EntityName)
            .AppendLine("Factory);")
            .DecrementIndent()
            .AppendLine();
    }

    private static void WriteQueryEntityTask(IndentedStringBuilder codeBuilder, EntityClass entity)
    {
        // public static Task<IEnumerable<Entity>> QueryEntityAsync(this IDataCommand dataCommand) => QueryAsync(dataCommand, EntityFactory);
        codeBuilder
            .AppendLine("/// <summary>")
            .Append("/// Executes the command against the connection and converts the results to <see cref=\"T:")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/> objects.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"dataCommand\">The <see cref=\"T:FluentCommand.IDataCommand\"/> for this extension method.</param>")
            .AppendLine("/// <returns>")
            .Append("/// An <see cref=\"T:System.Collections.Generic.IEnumerable`1\" /> of <see cref=\"T:")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/> objects.")
            .AppendLine("/// </returns>")
            .Append("public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .Append(">> Query")
            .Append(entity.EntityName)
            .AppendLine("Async(this global::FluentCommand.IDataCommand dataCommand)")
            .IncrementIndent()
            .Append("=> global::FluentCommand.Internal.DataSequentialReader.QueryAsync(dataCommand, ")
            .Append(entity.EntityName)
            .AppendLine("Factory);")
            .DecrementIndent()
            .AppendLine();

    }

    private static void WriteQuerySingleEntity(IndentedStringBuilder codeBuilder, EntityClass entity)
    {
        // public static Entity QuerySingleEntity(this IDataCommand dataCommand) => QuerySingle(dataCommand, EntityFactory);
        codeBuilder
            .AppendLine("/// <summary>")
            .Append("/// Executes the query and returns the first row in the result as a <see cref=\"T:")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/> object.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"dataCommand\">The <see cref=\"T:FluentCommand.IDataCommand\"/> for this extension method.</param>")
            .AppendLine("/// <returns>")
            .Append("/// A instance of <see cref=\"")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/>  if row exists; otherwise null.")
            .AppendLine("/// </returns>")
            .Append("public static ")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .Append(" QuerySingle")
            .Append(entity.EntityName)
            .AppendLine("(this global::FluentCommand.IDataCommand dataCommand)")
            .IncrementIndent()
            .Append("=> global::FluentCommand.Internal.DataSequentialReader.QuerySingle(dataCommand, ")
            .Append(entity.EntityName)
            .AppendLine("Factory);")
            .DecrementIndent()
            .AppendLine();
    }

    private static void WriteQueryEntity(IndentedStringBuilder codeBuilder, EntityClass entity)
    {
        // public static IEnumerable<Entity> QueryEntity(this IDataCommand dataCommand) => Query(dataCommand, EntityFactory);
        codeBuilder
            .AppendLine("/// <summary>")
            .Append("/// Executes the command against the connection and converts the results to <see cref=\"T:")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/> objects.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"dataCommand\">The <see cref=\"T:FluentCommand.IDataCommand\"/> for this extension method.</param>")
            .AppendLine("/// <returns>")
            .Append("/// An <see cref=\"T:System.Collections.Generic.IEnumerable`1\" /> of <see cref=\"T:")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/> objects.")
            .AppendLine("/// </returns>")
            .Append("public static global::System.Collections.Generic.IEnumerable<")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .Append("> Query")
            .Append(entity.EntityName)
            .AppendLine("(this global::FluentCommand.IDataCommand dataCommand)")
            .IncrementIndent()
            .Append("=> global::FluentCommand.Internal.DataSequentialReader.Query(dataCommand, ")
            .Append(entity.EntityName)
            .AppendLine("Factory);")
            .DecrementIndent()
            .AppendLine();
    }

    private static void WriteEntityFactory(IndentedStringBuilder codeBuilder, EntityClass entity)
    {
        codeBuilder
            .AppendLine("/// <summary>")
            .Append("/// A factory for creating <see cref=\"T:")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/> objects from the current row in the specified <paramref name=\"dataRecord\"/>.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"dataRecord\">The open <see cref=\"T:System.Data.IDataRecord\"/> to get the object from.</param>")
            .AppendLine("/// <returns>")
            .Append("/// A instance of <see cref=\"")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .AppendLine("\"/>  having property names set that match the field names in the <paramref name=\"dataRecord\"/>.")
            .AppendLine("/// </returns>")
            .Append("public static ")
            .Append(entity.EntityNamespace)
            .Append(".")
            .Append(entity.EntityName)
            .Append(" ")
            .Append(entity.EntityName)
            .AppendLine("Factory(this global::System.Data.IDataRecord dataRecord)")
            .AppendLine("{")
            .IncrementIndent();

        foreach (var entityProperty in entity.Properties)
        {
            var aliasType = GetAliasMap(entityProperty.PropertyType);
            var fieldName = CamelCase(entityProperty.PropertyName);

            codeBuilder
                .Append(aliasType)
                .Append(" ")
                .Append(fieldName)
                .AppendLine(" = default;");
        }

        codeBuilder
            .AppendLine()
            .AppendLine("for (var __index = 0; __index < dataRecord.FieldCount; __index++)")
            .AppendLine("{")
            .IncrementIndent()
            .AppendLine("if (dataRecord.IsDBNull(__index))")
            .IncrementIndent()
            .AppendLine(" continue;")
            .DecrementIndent()
            .AppendLine();

        codeBuilder
            .AppendLine("var __name = dataRecord.GetName(__index);")
            .AppendLine("switch (__name)")
            .AppendLine("{")
            .IncrementIndent();

        foreach (var entityProperty in entity.Properties)
        {
            var fieldName = CamelCase(entityProperty.PropertyName);
            var readerName = GetReaderName(entityProperty.PropertyType);

            codeBuilder
                .Append("case nameof(")
                .Append(entity.EntityNamespace)
                .Append(".")
                .Append(entity.EntityName)
                .Append(".")
                .Append(entityProperty.PropertyName)
                .AppendLine("):");

            codeBuilder
                .IncrementIndent()
                .Append(fieldName)
                .Append(" = dataRecord.")
                .Append(readerName)
                .AppendLine("(__index);")
                .AppendLine("break;")
                .DecrementIndent();
        }

        codeBuilder
            .DecrementIndent()
            .AppendLine("}") // switch
            .DecrementIndent()
            .AppendLine("}") // for
            .AppendLine();

        codeBuilder
            .Append("return new ")
            .Append(entity.EntityNamespace)
            .Append(".")
            .AppendLine(entity.EntityName)
            .AppendLine("{")
            .IncrementIndent();

        foreach (var entityProperty in entity.Properties)
        {
            var fieldName = CamelCase(entityProperty.PropertyName);
            codeBuilder
                .Append(entityProperty.PropertyName)
                .Append(" = ")
                .Append(fieldName)
                .AppendLine(",");
        }

        codeBuilder
            .DecrementIndent()
            .AppendLine("};"); // new

        codeBuilder
            .DecrementIndent()
            .AppendLine("}") // method
            .AppendLine();
    }

    private static string GetAliasMap(string type)
    {
        return type switch
        {
            "System.Boolean" => "bool",
            "System.Byte" => "byte",
            "System.Byte[]" => "byte[]",
            "System.Char" => "char",
            "System.Decimal" => "decimal",
            "System.Double" => "double",
            "System.Single" => "float",
            "System.Int16" => "short",
            "System.Int32" => "int",
            "System.Int64" => "long",
            "System.String" => "string",
            _ => type,
        };
    }

    private static string GetReaderName(string propertyType)
    {
        // remove nullable
        var type = propertyType.EndsWith("?") ? propertyType.Substring(0, propertyType.Length - 1) : propertyType;

        return type switch
        {
            "System.Boolean" => "GetBoolean",
            "System.Byte" => "GetByte",
            "System.Byte[]" => "GetBytes",
            "System.Char" => "GetChar",
            "System.DateTime" => "GetDateTime",
            "System.DateTimeOffset" => "GetDateTimeOffset",
            "System.Decimal" => "GetDecimal",
            "System.Double" => "GetDouble",
            "System.Guid" => "GetGuid",
            "System.Single" => "GetFloat",
            "System.Int16" => "GetInt16",
            "System.Int32" => "GetInt32",
            "System.Int64" => "GetInt64",
            "System.String" => "GetString",
            "bool" => "GetBoolean",
            "byte" => "GetByte",
            "byte[]" => "GetBytes",
            "char" => "GetChar",
            "decimal" => "GetDecimal",
            "double" => "GetDouble",
            "float" => "GetFloat",
            "short" => "GetInt16",
            "int" => "GetInt32",
            "long" => "GetInt64",
            "string" => "GetString",
            "FluentCommand.ConcurrencyToken" => "GetBytes",
            _ => $"GetValue<{propertyType}>"
        };
    }

    private static string CamelCase(string name)
    {
        if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
            return name;

        char[] chars = name.ToCharArray();
        FixCasing(chars);

        return new string(chars);
    }

    private static void FixCasing(Span<char> chars)
    {
        for (int i = 0; i < chars.Length; i++)
        {
            if (i == 1 && !char.IsUpper(chars[i]))
                break;

            bool hasNext = (i + 1 < chars.Length);

            // Stop when next char is already lowercase.
            if (i > 0 && hasNext && !char.IsUpper(chars[i + 1]))
            {
                // If the next char is a space, lowercase current char before exiting.
                if (chars[i + 1] == ' ')
                    chars[i] = char.ToLowerInvariant(chars[i]);

                break;
            }

            chars[i] = char.ToLowerInvariant(chars[i]);
        }
    }
}

public record EntityClass(
    InitializationMode InitializationMode,
    string EntityNamespace,
    string EntityName,
    ImmutableArray<EntityProperty> Properties
);

public record EntityProperty(
    string PropertyName,
    string PropertyType
);

public enum InitializationMode
{
    ObjectInitializer,
    Constructor
}