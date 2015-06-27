---
layout: post
title: "Cache using Dynamic LINQ and Generics"
description: ""
date: 2008-05-03 12:00:00 UTC
category: 
tags: [.net, generics, cache, linq]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>You know those tables in your systems that are almost static (never change)  by are consulted thousands of times a day? Well, the other day at work some  asked me if we could avoid all this trips to the database.</p>
<p>The solution I came up with creates a cache at our <span class="caps">BLL</span> (Business Logic Layer) avoiding <span class="caps">DAL</span> layer to be called. First of all our base architecture is  based on the principles on the asp.net tutorial <a href="http://www.asp.net/learn/data-access/tutorial-02-cs.aspx">Creating a  Business Logic Layer</a>. This article basically creates something very similar  to a <a href="http://www.dofactory.com/Patterns/PatternProxy.aspx">proxy  pattern</a> before the <span class="caps">DAL</span> which gives me the perfect  place to intercept any calls to the <span class="caps">DAL</span> and consult my  cache.</p>
<p>To avoid the trip to the database, when the <span class="caps">BLL</span> is  instantiated I do one query to the database which returns all the records in the  table. The resulting DataTable is stored in my cache class which is declared as  static in the class. This will make possible that the cache is populated only  once. All subsequent queries will fetch the results from the Cache instead of  making a trip to the database.</p>
<p>The cache table is very simple. It stores a DataTable and performs filters in  it using <span class="caps">LINQ</span> syntax. I had other options but since I  was starting to work with <span class="caps">LINQ</span> at the time it seemed  like a good choice. For this code I use the Dynamic <span class="caps">LINQ</span>  which I mentioned in my <a href="http://www.gbogea.com/2008/4/16/dynamic-queries-using-linq">previous  post</a>.</p>
<p>In order to work with TypedDataSets (which was a requirement for me) I create  the Cache class to accept the DataTable and DataRow types.</p>
<pre class="brush: csharp" title="code">
public class CacheDataTable&lt;T, S&gt;  where T : DataTable where S : DataRow
    {
        private T cacheDataTable;

        //stores the DataTable that will be used for future consults
        public CacheDataTable(T dataTable)
        {
            this.cacheDataTable = dataTable;
        }

        //returns a DataTable filtered by the expression
        public T GetData(string expression, params object[] values)
        {
            IEnumerable&lt;S&gt; tmp = ((IEnumerable&lt;S&gt;)cacheDataTable).AsQueryable().Where(expression, values);
            return getDataTablePopulated(tmp);
        }

        //returns all the records
        public T GetData()
        {
            return getDataTablePopulated((IEnumerable&lt;S&gt;)cacheDataTable);
        }

        //takes an enumeration and creates a new DataTable
        private T getDataTablePopulated(IEnumerable&lt;S&gt; rows)
        {
            T dt = Activator.CreateInstance&lt;T&gt;();
            foreach (S row in rows)
            {
                dt.ImportRow(row);
            }
            return dt;
        }
    }
</pre>
<p>After having the Cache class defined you can use it in the <span class="caps">BLL</span> like this:</p>
<pre class="brush: csharp" title="code">
public class CountryBLL
{
        CountryDAL dal;

        //the cache is static so that it is shared among all instances from the CountryBLL class
        private static CacheDataTable&lt;DataSet1.CountryDataTable, DataSet1.CountryRow&gt; cache;

        public TBG_CountryBLL()
        {
            dal = new CountryDAL();
            //tests if the cache class has not been created yet. Only happens at the first access
            if (cache == null)
            {
                cache = new CacheDataTable&lt;DataSet1.CountryDataTable, DataSet1.CountryRow&gt;(dal.GetData());
            }
        }

        public DataSet1.CountryDataTable GetData()
        {
            //gets all records from the cache
            return cache.GetData();
        }

        public DataSet1.CountryDataTable GetDataByPK(decimal id)
        {
            return cache.GetData(&quot;id == @0&quot;, id);
        }
}

</pre>
<p>Coded like this the CountryBLL will only cause a database connection at its  first instantiation. This approach should only be used with tables which are  static (or almost). If one record is inserted in this table it would demand the  application to be restarted so that the static cache variable would be unloaded  and created again. There are ways around this but it is not the objective of  this post.</p>
<p>Hope this will help someone :-)</p>
</div>