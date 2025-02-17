using System.Collections.Generic;

using FluentCommand.Entities;
using FluentCommand.Query;
using FluentCommand.Query.Generators;

using VerifyXunit;

using Xunit;

namespace FluentCommand.Tests.Query;

[UsesVerify]
public class DeleteBuilderTest
{
    [Fact]
    public async System.Threading.Tasks.Task DeleteEntityWithOutput()
    {
        var sqlProvider = new SqlServerGenerator();
        var parameters = new List<QueryParameter>();

        var builder = new DeleteEntityBuilder<Status>(sqlProvider, parameters)
            .Output(p => p.Id)
            .Where(p => p.Id, 1);

        var queryStatement = builder.BuildStatement();

        var sql = queryStatement.Statement;

        await Verifier.Verify(sql).UseDirectory("Snapshots");
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteEntityWithComment()
    {
        var sqlProvider = new SqlServerGenerator();
        var parameters = new List<QueryParameter>();

        var builder = new DeleteEntityBuilder<Status>(sqlProvider, parameters)
            .Tag()
            .Output(p => p.Id)
            .Where(p => p.Id, 1);

        var queryStatement = builder.BuildStatement();

        var sql = queryStatement.Statement;

        await Verifier.Verify(sql).UseDirectory("Snapshots");
    }

    [Fact]
    public async System.Threading.Tasks.Task DeleteEntityJoin()
    {
        var sqlProvider = new SqlServerGenerator();
        var parameters = new List<QueryParameter>();

        var builder = new DeleteEntityBuilder<Task>(sqlProvider, parameters)
            .Tag()
            .Output(p => p.Id)
            .From(tableAlias: "t")
            .Join<Priority>(p => p
                .Left(p => p.PriorityId, "t")
                .Right(p => p.Id, "p")
            )
            .Where<Priority, int>(p => p.Id, 4, "p", FilterOperators.GreaterThanOrEqual);

        var queryStatement = builder.BuildStatement();

        var sql = queryStatement.Statement;

        await Verifier.Verify(sql).UseDirectory("Snapshots");
    }

}
