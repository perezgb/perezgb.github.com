---
layout: post
title: "Dynamic Queries using LINQ"
description: ""
date: 2008-04-16 12:00:00 UTC
category: 
tags: [.net, linq]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><span class="caps">LINQ</span> is great; I&rsquo;m having all kinds of use to it  lately. The other day I needed to do something different, I needed to write a  <span class="caps">LINQ</span> query dynamically, constructing the <span class="caps">LINQ</span> query as a String and then executing it (which is pretty  common thing when you&rsquo;re using <span class="caps">SQL</span>).</p>
<p>I was already convinced that I would need to spend a couple of weeks writing  such functionality. Turns out that Microsoft has already written a <span class="caps">LINQ</span> Dynamic Query Library. It doesn&rsquo;t come with the framework  but you can download it and include in your project. Take a look at <a href="http://weblogs.asp.net/scottgu/archive/2008/01/07/dynamic-linq-part-1-using-the-linq-dynamic-query-library.aspx">ScottGu&rsquo;s  Blog</a> to see his post about it.</p>
<p>Here is an example:</p>
<p>Suppose I have a LocalDBDataSet with a mapping to the Table Person (Name,  Sex, Phone).</p>
<p>If I wanted to get all the people in the table and perform a <span class="caps">LINQ</span> query to filter only the Males (M) I could do it like  this:</p>
<pre title="code" class="brush: csharp">
PersonTableAdapter ta = new PersonTableAdapter();
DataTable dt = ta.GetData();
IEnumerable&lt;LocalDBDataSet.PersonRow&gt; drCollection = 
    ((IEnumerable&lt;LocalDBDataSet.PersonRow&gt;)dt).AsQueryable().Where(&quot;SEX == @0&quot;, &quot;M&quot;);

</pre>
<p>I know this isn&rsquo;t the most exciting example ever and I would need dynamic  <span class="caps">LINQ</span> to do it but I just wanted to show you what you  could accomplish. You get the idea, right?</p>
<p>With this kind of resource you could accomplish a lot of cool stuff. In the  next post I&rsquo;ll show how you can write a Cache class that can be queried with  <span class="caps">LINQ</span> to avoid unnecessary trips to the database.</p>
<p>You can download the <span class="caps">LINQ</span> Dynamic Query Library <a href="http://msdn2.microsoft.com/en-us/vcsharp/bb894665.aspx" title="C# version">here</a> or <a href="http://msdn2.microsoft.com/en-us/vbasic/bb964686.aspx" title="VB version">here</a></p>
</div>