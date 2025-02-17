using System;
using System.Linq;

using FluentAssertions;

using FluentCommand.Entities;
using FluentCommand.Extensions;
using FluentCommand.Query;

using Xunit;
using Xunit.Abstractions;

namespace FluentCommand.SqlServer.Tests;

public class DataQueryTests : DatabaseTestBase
{
    public DataQueryTests(ITestOutputHelper output, DatabaseFixture databaseFixture) : base(output, databaseFixture)
    {
    }

    [Fact]
    public async System.Threading.Tasks.Task SqlQuerySingleEntityAsync()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        string email = "kara.thrace@battlestar.com";

        var user = await session
            .Sql(builder => builder
                .Select<User>()
                .Where(p => p.EmailAddress, email)
            )
            .QuerySingleAsync(r => new User
            {
                Id = r.GetGuid("Id"),
                EmailAddress = r.GetString("EmailAddress"),
                IsEmailAddressConfirmed = r.GetBoolean("IsEmailAddressConfirmed"),
                DisplayName = r.GetString("DisplayName"),
                PasswordHash = r.GetString("PasswordHash"),
                ResetHash = r.GetString("ResetHash"),
                InviteHash = r.GetString("InviteHash"),
                AccessFailedCount = r.GetInt32("AccessFailedCount"),
                LockoutEnabled = r.GetBoolean("LockoutEnabled"),
                LockoutEnd = r.GetDateTimeOffsetNull("LockoutEnd"),
                LastLogin = r.GetDateTimeOffsetNull("LastLogin"),
                IsDeleted = r.GetBoolean("IsDeleted"),
                Created = r.GetDateTimeOffset("Created"),
                CreatedBy = r.GetString("CreatedBy"),
                Updated = r.GetDateTimeOffset("Updated"),
                UpdatedBy = r.GetString("UpdatedBy"),
                RowVersion = r.GetBytes("RowVersion"),
            });

        user.Should().NotBeNull();
        user.EmailAddress.Should().Be(email);
    }

    [Fact]
    public async System.Threading.Tasks.Task SqlQueryCountAsync()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        string email = "kara.thrace@battlestar.com";

        var count = await session
            .Sql(builder => builder
                .Select<User>()
                .Count()
                .Where(p => p.EmailAddress, email)
            )
            .QueryValueAsync<int>();

        count.Should().Be(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task SqlQuerySumAsync()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        var count = await session
            .Sql(builder => builder
                .Select<Status>()
                .Aggregate(p => p.DisplayOrder, AggregateFunctions.Sum)
            )
            .QueryValueAsync<int>();

        count.Should().BeGreaterThan(1);
    }

    [Fact]
    public async System.Threading.Tasks.Task SqlQueryInEntityAsync()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        var values = new[] { 1, 2, 3 };

        var results = await session
            .Sql(builder => builder
                .Select<Status>()
                .WhereIn(p => p.Id, values)
                .Tag()
            )
            .QueryAsync<Status>();

        results.Should().NotBeNull();

        var list = results.ToList();
        list.Count.Should().Be(3);
    }

    [Fact]
    public async System.Threading.Tasks.Task SqlQueryInComplexEntityAsync()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        var values = new[] { 1, 2, 3 }.ToDelimitedString();

        var results = await session
            .Sql(builder =>
            {
                builder
                    .Statement()
                    .Query("CREATE TABLE #identifiers (Id int);");

                builder
                    .Statement()
                    .Query("INSERT INTO #identifiers (Id) SELECT CONVERT(int, value) FROM STRING_SPLIT(@Identifiers, @Separator);")
                    .Parameter("@Identifiers", values)
                    .Parameter("@Separator", ",");

                builder
                    .Select<Status>()
                    .Tag()
                    .From(tableAlias: "s")
                    .Join(j => j
                        .Left("Id", "s")
                        .Right("Id", "#identifiers", null, "i")
                    );
            })
            .QueryAsync<Status>();

        results.Should().NotBeNull();

        var list = results.ToList();
        list.Count.Should().Be(3);
    }


    [Fact]
    public async System.Threading.Tasks.Task SqlInsertValueQuery()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        var id = Guid.NewGuid();

        var userId = await session
            .Sql(builder => builder
                .Insert<User>()
                .Value(p => p.Id, id)
                .Value(p => p.EmailAddress, $"{id}@email.com")
                .Value(p => p.DisplayName, "Last, First")
                .Value(p => p.FirstName, "First")
                .Value(p => p.LastName, "Last")
                .Output(p => p.Id)
                .Tag()
            )
            .QueryValueAsync<Guid>();

        userId.Should().Be(id);
    }

    [Fact]
    public async System.Threading.Tasks.Task SqlInsertEntityQuery()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            EmailAddress = $"{id}@email.com",
            DisplayName = "Last, First",
            FirstName = "First",
            LastName = "Last",
            Created = DateTimeOffset.Now,
            Updated = DateTimeOffset.Now
        };

        var userId = await session
            .Sql(builder => builder
                .Insert<User>()
                .Values(user)
                .Output(p => p.Id)
                .Tag()
            )
            .QueryValueAsync<Guid>();

        userId.Should().Be(id);
    }


    [Fact]
    public async System.Threading.Tasks.Task SqlInsertUpdateDeleteEntityQuery()
    {
        var session = GetConfiguration().CreateSession();
        session.Should().NotBeNull();

        var id = Guid.NewGuid();
        var user = new User
        {
            Id = id,
            EmailAddress = $"{id}@email.com",
            DisplayName = "Last, First",
            FirstName = "First",
            LastName = "Last",
            Created = DateTimeOffset.Now,
            Updated = DateTimeOffset.Now
        };

        var userId = await session
            .Sql(builder => builder
                .Insert<User>()
                .Values(user)
                .Output(p => p.Id)
                .Tag()
            )
            .QueryValueAsync<Guid>();

        userId.Should().Be(id);

        var selected = await session
            .Sql(builder => builder
                .Select<User>()
                .Where(p => p.Id, id)
                .Tag()
            )
            .QuerySingleAsync<User>();

        selected.Should().NotBeNull();
        selected.Id.Should().Be(id);

        var updateId = await session
            .Sql(builder => builder
                .Update<User>()
                .Value(p => p.DisplayName, "Updated")
                .Output(p => p.Id)
                .Where(p => p.Id, id)
                .Tag()
            )
            .QueryValueAsync<Guid>();

        updateId.Should().Be(id);

        var deleteId = await session
            .Sql(builder => builder
                .Delete<User>()
                .Output(p => p.Id)
                .Where(p => p.Id, id)
                .Tag()
            )
            .QueryValueAsync<Guid>();

        deleteId.Should().Be(id);

    }

}
