---
layout: post
title: "Linq: Using Group By with Multiple Columns"
description: ""
date: 2009-03-17 12:00:00 UTC
category: 
tags: [linq, c#, .net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>I have a group of classes that I build to get metadata from the database to use  with my code generators. The class has the following structure:</p>
<pre title="code" class="brush: csharp">
public class ConstraintMetadata
{
    public string ConstraintName { get; set; }

    public string ColumnName { get; set; }

    public string ConstraintType { get; set; }

    public string TableName { get; set; }
}</pre>
<p>I wanted to write a group by clause using Linq to group the objects I retrieved  from the db by TableName, ConstraintName and ConstraintType. This would allow me  to have one object for each constraint with a list of all the columns. Since  Linq only allows one object in the group by clause the way out is to create an  anonymous object with all the properties I want.</p>
<pre title="code" class="brush: csharp">
var constraints = from c in constraintsMetadata
                  group c by new
                             {
                                 c.TableName, 
                                 c.ConstraintType, 
                                 c.ConstraintName
                             } into g
                      select new
                      {
                          g.Key.ConstraintName,
                          g.Key.ConstraintType,
                          g.Key.TableName,
                          Columns = g.Select(x=&gt;x.ColumnName).ToList()
                      };
</pre>
<p>Now, if I want to get all the constraints for a table and print it to the  console window I can do this:</p>
<pre title="code" class="brush: csharp">
var tableConstraints = constraints.Where(x =&gt; x.TableName == table.Name);

foreach (var tableConstraint in tableConstraints)
{
    Console.Out.WriteLine(&quot;Constraint Name:{0}, Table:{1}, Type:{2} Columns:&quot;, 
        tableConstraint.ConstraintName , 
        tableConstraint.TableName, 
        tableConstraint.ConstraintType);
    foreach (var column in tableConstraint.Columns)
    {
        Console.Out.WriteLine(column);
    }
}
</pre>
<p>&nbsp;</p>
</div>