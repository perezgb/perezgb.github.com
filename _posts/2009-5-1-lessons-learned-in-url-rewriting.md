---
layout: post
title: "Lessons Learned in Url Rewriting"
description: ""
date: 2009-05-01 12:00:00 UTC
category: 
tags: [asp.net, speakout, urlrewrite]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>When I decided to write my own blog and leave Mephisto one of the few requirements I felt were really important was to maintain the url pattern I was already using. My articles had url's like this:<br />
<br />
www.gbogea.com/year/month/day/permalink<br />
<br />
When I first looked up url rewriting in google I found a great <a href="http://weblogs.asp.net/scottgu/archive/2007/02/26/tip-trick-url-rewriting-with-asp-net.aspx">article by ScottGu</a> that really suited my needs. I'm using IIS7 on my develpment machine and I thought I was also using it on the hosting service I contracted. ScottGu's post shows a really simple way to do rewriting when using IIS7. All you need is the <a href="http://urlrewriter.net/">UrlRewriter.Net module</a> and a few configurations to your web.config.<br />
Checkout SocottGu's post for the whole config, here I'll only show you the pattern I needed to be compatible with the articles url in Mephisto. Here it is: </p>
<pre class="brush: xhtml" title="code">
&lt;rewrite url=&quot;~/((19|20)\d\d)/(0?[1-9]|1[012])/(0?[1-9]|[12][0-9]|3[01])/(.+)&quot; to=&quot;~/PostDetail.aspx?year=$1&amp;amp;month=$3&amp;amp;day=$4&amp;amp;permalink=$5&quot;/&gt;</pre>
<p>Once I did this everything worked fine. I had achieved my objective, or so I thought. The problem is that I found out later on that my hosting service was actually using IIS6 and to my sadness IIS6 doesn't do well with url's that do not have an extension. <br />
<br />
When I contacted my hosting they informed me that they had support for ISAPI Rewrite, which is alson discussed in Scott's post but sadly not in engough depth for my case. The configuration is also a lot more anoying then with UrlRewrite.Net. If you have to use ISAPI Rewrite my first recomendation is to get in touch with your hosting and find out if they have version 2 or 3 installed. It makes a great difference. In my case I had version 3 installed. Here are the steps I took to configure it:<br />
<br />
1 - Create a &quot;.htaccess&quot; file in the root or your web application.<br />
<br />
2 - In the file I've written the following code: </p>
<pre class="brush: csharp" title="code">
RewriteEngine on
RewriteBase /
RewriteRule ((19|20)\d\d)/(0?[1-9]|1[012])/(0?[1-9]|[12][0-9]|3[01])/(.+)$ /PostDetail.aspx?year=$1&amp;month=$3&amp;day=$4&amp;permalink=$5 [NC,L]</pre>
<p><br />
The rewrite rule is basically translating the pattern I'm using (www.gbogea.com/year/month/day/permalink) to the real pattern that the ASP.NET applcation understands (PostDetail.aspx?year=YYYY&amp;month=MM&amp;day=DD&amp;permalink=WHATERVER).<br />
<br />
The options and the end are also very important. NC means Case Insensitive. L means that no other subsquent rule should be processed, it stops here. You will find a lot of examples that use an R (which means Redirect) however this was not suited to my case. The redirect would be done to the new URL and then the url shown in the browser would be the ASP.NET url and not mine.<br />
<br />
This usage was not easy to figure out. If you need any assistance on using ISAPIRewrite I can recomend this two links:<br />
<br />
Documentation: <a href="http://www.helicontech.com/isapi_rewrite/doc/">http://www.helicontech.com/isapi_rewrite/doc/</a><br />
Blog: <a href="http://helicontech.blogspot.com/search/label/isapi_rewrite">http://helicontech.blogspot.com/search/label/isapi_rewrite</a><br />
<br />
Helicon's blog was the one that helped me the most. The have a post about common issues people have which is really to the point.<br />
<br />
Looking back at all the work I had it would probably be cheaper just to have my hosting plan upgraded to IIS7 but where would be the fun in that right? Joking aside, it's this kind of thing that make you learn, perhaps your client doesn't have the option to migrate to IIS7, then what would you do? Having said that, if you have the choice between using IIS 6 and 7 I would definitely go with 7 and avoid using ISAPI Rewrite altogether.<br />
<br />
If you want to have a closer look at my web.config or the .htaccess file you can look at the source code for this blog at <a href="http://speakoutblog.codeplex.com/">speakoutblog.codeplex.com</a>. At this time there is no realease version yet but you can go to the source code and download any commit you want.</p>
</div>