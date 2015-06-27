---
layout: post
title: "How to Match Date without Time in NHibernate"
description: ""
date: 2009-11-26 12:00:00 UTC
category: 
tags: [speakout, nhibernate]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>I'm changing my <a href="http://speakoutblog.codeplex.com/">blog </a>to use an NHibernate data layer. I'll post about this experience soon.</p>
<p>One thing I think is worth mentioning is how to write a query to filter by date only. Yes, I want to ignore the time part of the date class. I new to NHibernate and I thought that there would be a function to do this. Well there isn't. There is nothing bad about this I just wanted to write about it to let others find the recommended solution to solve this problem.</p>
<p>The way to do it is by using BETWEEN and comparing the hour zero (00:00:00) of the day and the last second of the day (23:59:59).</p>
<p>Here is an example where I need to find all posts published in a given date:</p>
<pre title="code" class="brush: csharp">
public IList&lt;Post&gt; FindByDate(DateTime datePublished)
{
    DateTime initDate = datePublished.Date;
    DateTime endDate = datePublished.Date.AddDays(1).AddSeconds(-1);
    return Session.CreateCriteria&lt;Post&gt;()
        .Add(Expression.Between(&quot;PublishDate&quot;, initDate, endDate))
        .List&lt;Post&gt;();
}</pre>
<p>&nbsp;And here is the same example using HQL:</p>
<br />
<pre title="code" class="brush: csharp">
public IList&lt;Post&gt; FindByDate(DateTime datePublished)
{
    DateTime initDate = datePublished.Date;
    DateTime endDate = datePublished.Date.AddDays(1).AddSeconds(-1);
    string hql = &quot;from Post where PublishDate between :initDate and :endDate&quot;;
    return Session.CreateQuery(hql)
        .SetDateTime(&quot;initDate&quot;, initDate)
        .SetDateTime(&quot;endDate&quot;, endDate)
        .List&lt;Post&gt;();
}</pre>
<p>Very simple. My only doubt was if there were any other way to do this. I found a <a href="http://groups.google.com/group/nhusers/browse_thread/thread/59c8899d6709678d/b8e7cbed469dff06?lnk=gst&amp;q=date#b8e7cbed469dff06">discussion</a> on the NHibernate group where Ayende says that this is the way to do it. Case closed :-)</p>
<p>&nbsp;</p>
</div>