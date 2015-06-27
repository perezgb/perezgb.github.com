---
layout: post
title: "Concurrency in ASP.NET Sessions "
description: ""
date: 2010-03-18 12:00:00 UTC
category: 
tags: [asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Last week some of the guys on my team were running some tests in our application and they noticed a contention problem. When a large report was running any other requests hung until the report was done being generated.</p>
<p>There were no transactions locking the database and no locks on the code either. After testing for a while I noticed that when I used 2 different browsers the problem would happen. Ha! So it must have something to do with the Session. The answer was on the bottom of the <a href="http://msdn.microsoft.com/en-us/library/ms178581.aspx">Session State</a> documentation page on MSDN. Here is the section that matters:</p>
<p>&quot;<em>Access to ASP.NET session state is exclusive per session, which means that if two different users make concurrent requests, access to each separate session is granted concurrently. However, if two concurrent requests are made for the same session (by using the same&nbsp;<span>SessionID</span>&nbsp;value), the first request gets exclusive access to the session information. The second request executes only after the first request is finished.</em>&quot;</p>
<p>&nbsp;So the problem only happens if you have multiple request from the same session. Still this is very annoying! This is done so that the Session maintains the consistency. If you don't need to write to the session (only read) the session state may be defined as ReadOnly in which case there will be no locking.</p>
<p>Depending on your application you might want to set the sessionState as ReadOnly in the web.config (affects all pages) and then mark each page that needs write access to use sessionState=true. Or you may want to do it the other way around. Set the pages that take a long time to process as sessionState=ReadOnly (so they don't block) and leave all the other pages that process quickly as they are.</p>
<p>&nbsp;To set up the web.config:</p>
<pre title="code" class="brush: xhtml">
&lt;system.web&gt;
    &lt;pages enableSessionState=&quot;ReadOnly&quot;/&gt;
&lt;/system.web&gt;</pre>
<p>To set up a single page use the Page directive:</p>
<pre title="code" class="brush: csharp">
&lt;%@ Page ... EnableSessionState=&quot;True&quot; %&gt;</pre>
<p>Setting the Page directive will override the web.config setting.</p>
<p>Hope this helps.</p>
</div>