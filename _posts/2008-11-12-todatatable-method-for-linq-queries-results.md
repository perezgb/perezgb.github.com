---
layout: post
title: "ToDataTable Method for Linq Queries Results"
description: ""
date: 2008-11-12 12:00:00 UTC
category: 
tags: [c#, linq]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>When you query a DataTable using Linq the normal result will be an  IEnumerable<datarow>, which is not a DataTable (obviously). If you need a  DataTable as a result you already have an extension method called  CopyToDataTable which will turn your IEnumerable<datarow> into a new DataTable.  Although if you want to join two DataTables and the merge the DataRows into a  new Anonymous Type the CopyToDataTable will not help you. Let&rsquo;s see how to  create a helper method that will transform an IEnumerable of anonymous types  into a new DataTable.</datarow></datarow></p>
<p>First let&rsquo;s create two DataTables: Person and Job. Here&rsquo;s the code:</p>
<pre class="brush: csharp" title="code">
DataTable dtPerson;
DataTable dtJob;

dtPerson= new DataTable(&quot;Person&quot;);
DataColumn dc;

dc = new DataColumn(&quot;Id&quot;, typeof(int));
dtPerson.Columns.Add(dc);

dc = new DataColumn(&quot;Name&quot;, typeof(string));
dtPerson.Columns.Add(dc);

dc = new DataColumn(&quot;Age&quot;, typeof(int));
dtPerson.Columns.Add(dc);

//JOB DataTable
dtJob = new DataTable(&quot;Job&quot;);

dc = new DataColumn(&quot;PersonId&quot;, typeof(int));
dtJob.Columns.Add(dc);

dc = new DataColumn(&quot;Position&quot;, typeof(string));
dtJob.Columns.Add(dc);
</pre>
<p>Here is the code to populate the DataTables:</p>
<pre class="brush: csharp" title="code">
DataRow dr = dtPerson.NewRow();
dr[0] = 1;
dr[1] = &quot;Gabriel&quot;;
dr[2] = 31;
dtPerson.Rows.Add(dr);

dr = dtPerson.NewRow();
dr[0] = 2;
dr[1] = &quot;Elisa&quot;;
dr[2] = 27;
dtPerson.Rows.Add(dr);

dr = dtJob.NewRow();
dr[0] = 1;
dr[1] = &quot;Programmer&quot;;
dtJob.Rows.Add(dr);

dr = dtJob.NewRow();
dr[0] = 2;
dr[1] = &quot;Manager&quot;;
dtJob.Rows.Add(dr);
</pre>
<p>Now let&rsquo;s write a Linq query to join these two DataTables:</p>
<pre class="brush: csharp" title="code">
var query = from p in dtPerson.AsEnumerable()
             join j in dtJob.AsEnumerable() on p.Field&lt;int&gt;(&quot;Id&quot;) equals j.Field&lt;int&gt;(&quot;PersonId&quot;)
             select new
             {
                 Id = p.Field&lt;int&gt;(&quot;Id&quot;),
                 Name = p.Field&lt;string&gt;(&quot;Name&quot;),
                 Job = j.Field&lt;string&gt;(&quot;Position&quot;)
             };
</pre>
<p>Notice that the result of the query is an IEnumerable of an anonymous type with  three properties (Id, Name and Position) which is a merge of our two DataTables  data. Now for the helper method that will convert this result to a new  DataTable. The base of this idea is in my previous article on using <a href="http://www.gbogea.com/2008/11/8/reflection-on-anonymous-types">reflection  on anonymous types</a>. I&rsquo;ll use this principle to inspect the properties of the  anonymous type and create new DataColumns for the new DataTable. After that all  it&rsquo;s needed is iterating over the results creating new DataRows. Here&rsquo;s the  method:</p>
<pre class="brush: csharp" title="code">
public static DataTable ToDataTable(this IEnumerable objectList)
{
    //if the result is null or if the number of
    //objects is less then 1, return null
    if (objectList == null || 
        objectList.OfType&lt;object&gt;().Count() &lt; 1)
    {
        return null;
    }
    //create a new list based on the IEnumerable
    List&lt;object&gt; list = new List&lt;object&gt;(objectList.OfType&lt;object&gt;());

    //take the first object in the list
    object o = list[0];
    //get the type of the object
    Type t = o.GetType();
    //read all the info of the properties
    PropertyInfo[] properties = t.GetProperties();

    DataTable dt = new DataTable();
    //for each property create a new column
    //in the DataTable
    foreach (var pi in properties)
    {
        //the new column has the name of the property
        //and it's type
        DataColumn dc = new DataColumn(pi.Name, pi.PropertyType);
        //add the column to the DataTable
        dt.Columns.Add(dc);
    }

    //add the rows to the DataTable
    foreach (var item in list)
    {
        DataRow dr = dt.NewRow();
        //each property represents a column 
        //that has to be set
        foreach (var pi in properties)
        {
            dr[pi.Name] = pi.GetValue(item, null);
        }
        dt.Rows.Add(dr);
    }
    return dt;
}
</pre>
<p>That&rsquo;s it. The returning DataTable will have three columns (int Id, string Name,  string Position). If you want you can download the code for the class <a href="http://www.gbogea.com/upload/LinqHelper.cs">here</a>. In this  code for download I transformed the ToDataTable method into an extension method.</p>
</div>