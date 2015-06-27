---
layout: post
title: "Access Asp.Net WebService from PHP Client"
description: ""
date: 2008-06-17 12:00:00 UTC
category: 
tags: [asp.net web, services]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Creating and consuming webservices in asp.net is an easy task with the help of Visual Studio. Although most of the scenarios I worked with were always with asp.net applications consuming asp.net web services, which makes things even easier. This week however I learned something new when I had to get a<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>client application to connect to my asp.net webservices.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">One of out clients was called saying that the service was not working. When I tested his<span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>client I noticed the the parameters that he was passing where not getting to my webservice even though the service was being called. The only problem then was passing the parameters. I Googled a little and got my own<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>client running.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">For the sake of this post let&rsquo;s say that our webservice gets an string as a parameters and returns the same value, this will let us test our code. So here is the asp.net webservice:</p>
<pre title="code" class="brush: csharp">
[WebService(Namespace = &quot;http://tempuri.org/&quot;)]
public class Service : System.Web.Services.WebService
{

    [WebMethod]
    public string MyTestMethod(string myParameter)
    {
        return &quot;The value of the parameter is: &quot; + myParameter;
    }

}</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">As you can see there&rsquo;s nothing to it.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">The<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>code is not much harder once you Google a little. First you have to have<a style="border-bottom: 1px dotted rgb(187, 187, 187); text-decoration: none; color: rgb(125, 0, 10);" href="http://sourceforge.net/projects/nusoap/">NuSoap Library</a><span class="Apple-converted-space">&nbsp;</span>which has the webservices infrastructure for<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span>. You may put it in the same directory of your test page. Then you code a simple client to access our service:</p>
<pre title="code" class="brush: xhtml">
&lt;?php

    require_once('./nusoap.inc');

    $wsdl = &quot;http://mysite/myapp/Service.asmx?wsdl&quot;;

    $mynamespace = &quot;http://tempuri.org/&quot;;

    $theVariable = array('myParameter'=&gt; 'gabriel');

    $s = new SoapClient($wsdl,true);

    $result = $s-&gt;call('MyTestMethod',$theVariable);

    echo &quot;&lt;h1&gt;Resultado:&lt;/h1&gt;&quot;;

    echo &quot;&lt;pre&gt;&quot;; print_r($result); print &quot;&lt;/pre&gt;&quot;;
?&gt;</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">After I had the code I could verify for my self that my client was right. The parameters in the above code are not getting to the webservice. The coding is right, I checked in several sites in the Internet, so where could the problem be?</p>
Well,<span class="Apple-converted-space">&nbsp;</span><span class="caps">SOAP</span><span class="Apple-converted-space">&nbsp;</span>specification allows for two formatting options: Style and Use. Style handles the formatting of the Body element of the envelope. Style allows for 2 values:
<ol>
    <li><span class="caps">RPC</span><span class="Apple-converted-space">&nbsp;</span>(Remote Procedure Call) which is the older format;</li>
    <li>Document which is a newer format and is VisualStudio default format.</li>
</ol>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">By now I bet you have figured out the solution, right? If you said that<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>default encoding is<span class="Apple-converted-space">&nbsp;</span><b><span class="caps">RPC</span></b><span class="Apple-converted-space">&nbsp;</span>and asp.net default encoding is<span class="Apple-converted-space">&nbsp;</span><b>Document</b><span class="Apple-converted-space">&nbsp;</span>then you&rsquo;re right on.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">How to solve the problem? Well, you can solve the problem on both ends, it all depends on your choice. First let&rsquo;s fix it on the asp.net side, it&rsquo;s really simple. All you have to do is use the SoapRpcMethod attribute on WebMethod, like this:</p>
<pre title="code" class="brush: csharp">
[WebService(Namespace = &quot;http://tempuri.org/&quot;)]
public class Service : System.Web.Services.WebService
{

    [SoapRpcMethod()]
    [WebMethod]
    public string MyTestMethod(string myParameter)
    {
        return &quot;The value of the parameter is: &quot; + myParameter;
    }

}</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">That&rsquo;s it, the<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>client starts to work instantly.</p>
This is a good solution if you only had<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>clients, since that was not my case I didn&rsquo;t want to mess with my webservice and have to make changes to all other asp.net clients. I wanted to solve the problem on the<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>client, so let&rsquo;s see how we could code the<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>client to fix the problem.</span></p>
<pre title="code" class="brush: xhtml">
&lt;?php

    require_once('./nusoap.inc');

    $wsdl = &quot;http://mysite/myapp/Service.asmx?wsdl&quot;;

    $mynamespace = &quot;http://tempuri.org/&quot;;

    $theVariable = array('myParameter'=&gt; 'gabriel');

    $s = new SoapClient($wsdl,true);

    //here is the only change you need to do
    $result = $s-&gt;call('MyTestMethod',array('parameters' =&gt;  $theVariable));

    echo &quot;&lt;h1&gt;Resultado:&lt;/h1&gt;&quot;;

    echo &quot;&lt;pre&gt;&quot;; print_r($result); print &quot;&lt;/pre&gt;&quot;;
?&gt;</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">What I had to do in the<span class="Apple-converted-space">&nbsp;</span><span class="caps">PHP</span><span class="Apple-converted-space">&nbsp;</span>code was to add an extra array around the array with the original variables. This handles the difference in the Document encoding used by asp.net.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">This problem really got on my nerves last week and I&rsquo;m really glad to have found a solution. As usual I wanted to share this with the world and maybe avoid the same headache to others. Ohh, and I also got to learn a little php on the way&hellip;</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Happy coding friends!</p>
</span></p>
</span></p>
</span></p>
</span></p>
</div>