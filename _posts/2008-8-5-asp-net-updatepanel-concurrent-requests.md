---
layout: post
title: "Asp.Net: UpdatePanel concurrent requests"
description: ""
date: 2008-08-05 12:00:00 UTC
category: 
tags: [ajax, asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Yesterday I was coding the infrastructure to start a thread in the server  that would take too long to finish and would timeout the browser. The idea is  that you could start a thread to run your long operation and let the server  return the response to the browser. In order to give some feedback to the user I  would query the server from time to time to check on the progress of the  operation.</p>
<p>Everything was going great but then someone said the client wanted to be able  to cancel the task running on the server. At first though it would not mess up  what I had done already. The thing is that if the server was already querying  the status of the task and I tried to cancel at the same time I would get an  error.</p>
<p>Long story short, you have problem when the UpdatePanel makes multiple  requests concurrently. When you do this the last request being executed wins. I  looked around and found an interesting solution to this problem.</p>
<p><a href="http://geekswithblogs.net/rashid/archive/2007/08/08/Asp.net-Ajax-UpdatePanel-Simultaneous-Update---A-Remedy.aspx">http://geekswithblogs.net/rashid/archive/2007/08/08/Asp.net-Ajax-UpdatePanel-Simultaneous-Update&mdash;-A-Remedy.aspx</a></p>
<p>What this guy did was to intercept the asyn requests and enqueue them so they  are only done one at a time. Of all the things I found on the Internet this was  the most interesting solution.</p>
<p>There is also an interesting solution in the <a href="http://www.asp.net/">Asp.Net site</a> that allows you to define on among  several UpdatePanel that gets precedence over the others. So, if one request is  being executed and the preferred UpdatePanel trying to execute the other one is  canceled. See it <a href="http://asp.net/ajax/documentation/live/tutorials/ExclusiveAsyncPostback.aspx">here</a></p>
</div>