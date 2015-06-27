---
layout: post
title: "Where is the RenderPartial method?"
description: ""
date: 2008-10-27 12:00:00 UTC
category: 
tags: [mvc, asp.net, .net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>I was coding an demo application using the <span class="caps">ASP</span>.NET  <span class="caps">MVC</span> Beta 1 and all the sudden I couldn&rsquo;t access the  RenderPartial method of the HtmlHelper from a Helper method I was writing. So  where did it go?</p>
<p>I noticed that in the previous release the RenderPartial was a static method  but in the Beta 1 they changed it to an Extension Method for the and it now  lives in the namespace &rsquo;&rsquo;&lsquo;System.Web.Mvc.Html&rsquo;&rsquo;&rsquo;. So if you want to use this  method all you need is to import this namespace in the classes where you are  using the System.Web.Mvc.HtmlHelper class. In the Views you will notice that it  keeps working as if nothing changed, that&rsquo;s why the <span class="caps">MVC</span>&rsquo;s development team has already added the  System.Web.Mvc.Html namespace in the web.config. Here is the namespace node of  the web.config the in <span class="caps">MVC</span> Beta 1.</p>
<pre title="code" class="brush: xhtml">
&lt;namespaces&gt;
   &lt;add namespace=&quot;System.Web.Mvc&quot;/&gt;
   &lt;add namespace=&quot;System.Web.Mvc.Ajax&quot;/&gt;
   &lt;add namespace=&quot;System.Web.Mvc.Html&quot;/&gt;
   &lt;add namespace=&quot;System.Web.Routing&quot;/&gt;
   &lt;add namespace=&quot;System.Linq&quot;/&gt;
   &lt;add namespace=&quot;System.Collections.Generic&quot;/&gt;
 &lt;/namespaces&gt;
</pre>
<p>&nbsp;</p>
</div>