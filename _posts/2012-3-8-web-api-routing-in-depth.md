---
layout: post
title: "Web Api - Routing in Depth"
description: ""
date: 2012-03-08 12:00:00 UTC
category: 
tags: [routing, aspnetwebapi]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>This week I had to use <a href="http://www.jetbrains.com/decompiler/">dotPeek </a>on the Web Api to understand how the framework selects the controller method (action) that gets executed. The process is not complicated but I thought it might help other people if I documented my findings.</p>
<p>There's also a post on the asp.net website that talks about the web api rounting, check it out&nbsp;<a href="http://www.asp.net/web-api/overview/web-api-routing-and-actions/routing-in-aspnet-web-api">here</a>.</p>
<p>The diagram below tries do describe the decision process behind selecting the method of the controller that will get executed. I've added a little extra documentation through the annotations on the diagram.</p>
<p><img alt="" src="http://www.perezgb.com/upload/webapiroutemain.jpg" /></p>
<h2>1 Action Based</h2>
<p>If the route defines the action parameter then the method selection will be based on the value of that parameter. The matching will be based on the method name or the method alias (A method in the controller may be aliased using the <em>ActionName </em>attribute).<br />
All the matching methods are then filtered by verb. The filtering is done by attributes applied to the method (such as <em>HttpGet</em>). Only the methods that match the verb of the incoming request will be returned. Methods with no attributes will also be considered valid as they don&rsquo;t have any attributes to allow filtration.</p>
<h2>2 Verb Based</h2>
<p>When the method selection is verb based we need to get the Http verb used in the request.&nbsp;The controller methods are then filtered in two ways:</p>
<ul>
    <li>Get all the methods annotated with http verb attributes matching the http method in the request (attributes such as HttpGet or HttpPost);</li>
    <li>Get all the methods that have the prefix matching the http method in the request (PostBooks matches the Post verb);</li>
</ul>
<h2>3 Check Results Found</h2>
<p>All the candidate methods returned by either the verb based or action based approach are then analyzed. If no candidates where found an exception is thrown. If multiple methods were found it&rsquo;s necessary to analyze it&rsquo;s parameters to find out the best possible fit.</p>
<h2>4 Find Route by Parameters</h2>
<p>The parameters from the route and the parameters from the query string are compared to the method's parameters. If no parameters are found in the query string or in the route, all methods without parameters will be selected. If multiple methods match the parameters found, the ones that take the most parameters will win. Here is a diagram that details this specific part of the process:</p>
<p><img alt="" src="http://www.perezgb.com/upload/webapiroutedetail.jpg" />&nbsp;</p>
<h2>5 Selection Filters</h2>
<p>The last filters executed on the candidate methods will make sure that all methods marked with the <em>NonAction </em>attribute will be excluded.<br />
&nbsp;</p>
<p>I hope these diagrams were clear enough to help understand better the process of selecting the controller method that gets executed for a request.</p>
</div>