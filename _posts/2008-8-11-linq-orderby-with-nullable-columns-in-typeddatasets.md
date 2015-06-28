---
layout: post
title: "LINQ: OrderBy with nullable columns in TypedDataSets"
description: ""
date: 2008-08-11 12:00:00 UTC
category: 
tags: [DataSet, linq, .net, dataset]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>This article shows a tip on how you can do sorting using the LINQ OrderBy with Typed DataSets.  For our example I&rsquo;ll use the the Northwind Database and it&rsquo;s Customers table.  I&rsquo;ll use two columns in this table. One is the ContactName which doesn&rsquo;t have any null values and the Region column which has null values.</p>
<p><img alt="" src="http://www.perezgb.com/upload/CustomersDataTable.jpg" /></p>
<p>First lets populate the DataTable we want to query using <a href="http://msdn.microsoft.com/en-us/library/bz9tthwx(VS.80).aspx">TableAdapter</a>  I had previously generated in the DataSet.</p>
<pre class="brush: csharp" title="code">
CustomersTableAdapter ta = new CustomersTableAdapter();
DataSetNorthwind.CustomersDataTable dt = ta.GetData();
</pre>
<p>I want to use <span class="caps">LINQ</span> select all values ordering them by  the Region column (which may be null). So here is the initial idea:</p>
<pre class="brush: csharp" title="code">
 var query = from r in dt.AsEnumerable()
                        orderby r.Region
                        select r;
</pre>
<p>Ok, no compilation errors but when you run the query you will get an  error:</p>
<p><b>The value for column &lsquo;Region&rsquo; in table &lsquo;Customers&rsquo; is DBNull.  <del>-</del>&gt; System.InvalidCastException: ...</b></p>
<p>If you think about it this is expected. The nullable columns in the DataSet all  have an IsNull method to check if the column is null or not and avoid this kind  of error. We are going to use the IsRegionNull() method in our favor to help us  solve the problem:</p>
<pre class="brush: csharp" title="code">
var query = from r in dt.AsEnumerable()
        orderby (r.IsRegionNull() ? &quot;&quot; : r.Region)
        select r;
</pre>
<p>I used the ternary operator to check if the column region is null or not. If  it is then I&rsquo;ll use an empty string as the value, if it&rsquo;s not null I can call  the Region property to get the value.</p>
<p>To see the ending result you can print all the values:</p>
<pre class="brush: csharp" title="code">
foreach (DataSetNorthwind.CustomersRow row in query)
{
     Console.WriteLine(&quot;{0} - {1}&quot;, row.ContactName, 
          row.IsRegionNull() ? &quot;&quot; : row.Region);
}
</pre>
<p>Of course you could have filtered out the rows with null regions, but the  goal here is to show what you can do to order the rows when you have a column  with values that may be null.</p>
</div>