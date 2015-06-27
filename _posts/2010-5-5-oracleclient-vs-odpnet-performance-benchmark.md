---
layout: post
title: "OracleClient vs ODP.NET Performance Benchmark"
description: ""
date: 2010-05-05 12:00:00 UTC
category: 
tags: [vs2010, performance, ado.net, profiler, oracle]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>In my previous post I mentioned that I was doing some comparisons on the performance of <a href="http://msdn.microsoft.com/en-us/library/system.data.oracleclient.aspx">System.Data.OracleClient</a> (provided by Microsoft) versus the <a href="http://download.oracle.com/docs/html/E15167_01/toc.htm">ODP.NET</a> (provided by Oracle). Since we still use a lot of DataSets on our project I decided that the simpler example that could be used to measure the performance would be to fill a DataTable using a DataAdapter.</p>
<p>For the experiment I took a table that would fill the DataTable with about 100.000 records. I then created two classes, one for each provider. The classes accessed the same database using the same SQL. Here is the code for the class using the OracleClient provider:&nbsp;</p>
<pre title="code" class="brush: csharp">
class MicrosoftProvider
{
    public static void RunTest()
    {
        var conn = new System.Data.OracleClient.OracleConnection(Params.ConnectionString);
        var cmd = new System.Data.OracleClient.OracleCommand(Params.CommandText, conn);
        var adapter = new System.Data.OracleClient.OracleDataAdapter(cmd);
        var dataTable = new DataTable();
        adapter.Fill(dataTable);
    }
}</pre>
<p>And here is the class using the ODP.NET provider:</p>
<pre title="code" class="brush: csharp">
class OracleProvider
{
    public static void RunTest()
    {
        var conn = new Oracle.DataAccess.Client.OracleConnection(Params.ConnectionString);
        var cmd = new Oracle.DataAccess.Client.OracleCommand(Params.CommandText, conn);
        var adapter = new Oracle.DataAccess.Client.OracleDataAdapter(cmd);
        var dataTable = new DataTable();
        adapter.Fill(dataTable);
    }
}</pre>
<p>I ran the test one after the other:</p>
<pre title="code" class="brush: csharp">
class Program
{
    static void Main(string[] args)
    {
        OracleProvider.RunTest();
        MicrosoftProvider.RunTest();
    }
}</pre>
<p>To compare the execution of each provider I ran this code using the <a href="http://www.microsoft.com/visualstudio/en-us/">VS 2010</a> <a href="http://blogs.msdn.com/profiler/">Profiler</a>, which by the way is very cool. The visualization of the tests has improved a lot since VS 2008. Here are the results summary:</p>
<p>&nbsp;<img alt="Profiler Summary" width="400" height="269" src="http://www.gbogea.com/upload/ProfilerOracleProviderSummary.JPG" /></p>
<p>&nbsp;As you can see there is a huge difference! The ODP.NET had less than 10% of the samples taken during processing while more than 90% were taken while running the method with the OracleClient provider. The report will also show you the information in more detail:</p>
<p>&nbsp;<img alt="Main method details" width="400" height="268" src="http://www.gbogea.com/upload/ProfilerOracleProviderDetails.JPG" /></p>
<p>And the coolest view of them all will even show the percentage used overlapping the code:</p>
<p><img width="400" height="146" alt="" src="http://www.gbogea.com/upload/ProfilerOracleProviderCodeView.JPG" /></p>
<p>Cool, right? I think so! Ok, now back to our tests... Before wrapping the post there is still one test I could do. I wanted to measure the execution time in seconds of each routine. To do this I used the Stopwatch class.</p>
<pre title="code" class="brush: csharp">
class Program
{
    static void Main(string[] args)
    {
        Time(() =&gt; OracleProvider.RunTest());
        Time(() =&gt; MicrosoftProvider.RunTest());
    }

    static void Time(Action action)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        action.Invoke();
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
    }
}</pre>
<p>When I ran the code I got the following results:</p>
<p><strong>ODP.NET: 20.6756 seconds</strong></p>
<p><strong>OracleClient: 41.7716 seconds</strong></p>
<p>The conclusion for the article is simple. The ODP.NET provider is way faster than the Oracle provider offered by Microsoft. On top of it all, as I mentioned in my previous post, many of the classes in the System.Data.OracleClient namespace are marked as obsolete. Microsoft no longer is going to keep working on them in the future.</p>
</div>