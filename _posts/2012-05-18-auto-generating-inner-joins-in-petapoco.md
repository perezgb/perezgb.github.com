---
layout: post
title: "Auto Generating Inner Joins in PetaPoco"
description: ""
category: 
tags: []
---
{% include JB/setup %}

<div id="post">
<p>One of the neat features of <a href="http://www.toptensoftware.com/petapoco/" title="PetaPoco">PetaPoco</a> is the&nbsp;<a href="http://www.toptensoftware.com/Articles/111/PetaPoco-Experimental-Multi-Poco-Queries" title="Multi-Poco queries">Multi-Poco queries</a>. It allows you to define a query that joins two or more tables and populate a set of related objects.</p>
<p>As an experiment on extending PetaPoco I wanted to be able to auto generate the sql statement for a Multi-Poco query. And I have to say it was pretty simple. Let's say we define our pocos as:</p>
<pre title="code" class="brush: csharp">[PrimaryKey("PersonId")]
public class Person
{
    public int PersonId { get; set; }
    public int AddressId { get; set; }
}

[PrimaryKey("AddressId")]
public class Address
{
    public int AddressId { get; set; }
    public string Street { get; set; }
}
</pre>
<p>Let's say that we would like to retrieve all the Person records with the associated Address object. This is how it would work right now:</p>
<pre title="code" class="brush: csharp">string sql = @"SELECT [Person].[PersonId], [Person].[AddressId],
        [Address].[AddressId], [Address].[Street]
        FROM [Person] INNER JOIN [Address] ON [Person].[AddressId] = [Address].[AddressId]";

var database = new Database(string.Empty);
IEnumerable&lt;person&gt; persons = database.Query&lt;Person,Address&gt;(sql);
</pre>
<p>This is how I'd wanted it to be:</p>
<pre title="code" class="brush: csharp">var database = new Database(string.Empty);
IEnumerable<person> persons = database.AutoQuery&lt;Person,Address&gt;(); </person></pre>
<p>To make it possible I created a few methods using PetaPoco's metadata classes. The main method is BuildSql which takes the type of the main poco (the root table) and an array of types of the pocos that shoud be joined to the main table. Here's the code:</p>
<pre title="code" class="brush: csharp">public partial class Database
{
    public IList&lt;T1&gt; AutoQuery&lt;T1, T2&gt;()
    {
        string sql = BuildSql(typeof(T1), new[] { typeof(T2) });
        return Query&lt;T1&gt;(new[] { typeof(T1), typeof(T2) }, null, sql, null).ToList();
    }

    public string BuildSql(Type rootType, Type[] joinTypes)
    {
        PocoData rootData = PocoData.ForType(rootType);
        string rootTable = EscapeSqlIdentifier(rootData.TableInfo.TableName);

        IEnumerable&lt;string&gt; columns = GetColumns(rootData);
        string join = string.Empty;

        foreach (var joinType in joinTypes)
        {
            PocoData joinData = PocoData.ForType(joinType);
            columns = columns.Union(GetColumns(joinData));
            join += BuildJoin(rootTable, joinData);
        }

        string columnList = string.Join(", ", columns);

        return string.Format("SELECT {0} FROM {1}{2}",
                             columnList,
                             rootTable,
                             join);
    }

    private string BuildJoin(string rootTable, PocoData join)
    {
        string joinedTable = EscapeSqlIdentifier(join.TableInfo.TableName);
        string joinPk = EscapeSqlIdentifier(join.TableInfo.PrimaryKey);
        return string.Format(" INNER JOIN {0} ON {1}.{2} = {3}.{4}",
                             joinedTable,
                             rootTable,
                             joinPk,
                             joinedTable,
                             joinPk);
    }

    private IEnumerable&lt;string&gt; GetColumns(PocoData rootData)
    {
        var tableName = EscapeSqlIdentifier(rootData.TableInfo.TableName);

        var cols = from c in rootData.QueryColumns
                   select tableName + "." + EscapeSqlIdentifier(c);
        return cols;
    }
}
</pre>
<p>Basically what we are doing is generating the sql statement and using the Query method that is already available in PetaPoco. I think that this shows how easy it is to extend PetaPoco with your conventions. And what is even nicer is that you don't need to convice anyone to change PetaPoco, you can just add your changes locally.</p>
<p>Of course this is just a simple example. It's easy to futher evolve it to take additional paramters to specify a WHERE clause for the statement.</p>
<p>I hope this will give other people more ideas on how to extend PetaPoco to solve their own problems.</p>
</div>