---
layout: post
title: "Web Api - Testing with HttpClient"
description: ""
date: 2012-02-21 12:00:00 UTC
category: 
tags: [restsharp, .net, httpclient, aspnetwebapi]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The <a href="http://www.asp.net/web-api">Web Api</a> is a project that has been on my radar since I first saw Glenn Block&rsquo;s <a href="http://channel9.msdn.com/Events/MIX/MIX11/FRM14">presentation last year on MIX</a>. I was only recently though, when we decided to really take on REST at work that I really started looking into with in depth.</p>
<p>One aspect that caught my attention was the <a href="http://msdn.microsoft.com/en-us/library/system.net.http.httpclient(v=vs.110).aspx">HttpClient</a>. I&rsquo;ve been using <a href="https://github.com/restsharp/RestSharp">RestSharp</a> for a while and I have to say that I am really happy with it. I still wanted to test the new HttpClient and see how it compared to RestSharp.</p>
<p>So let&rsquo;s say I want to call a super duper service that returns a Guid. Here is a VERY simple example of how I would implement a client using RestSharp:</p>
<pre title="code" class="brush: csharp">
public class GuidClient
{
    private readonly IRestClient _client;

    public GuidClient(IRestClient client)
    {
        _client = client;
    }

    public string Execute()
    {
        var request = new RestRequest();

        RestResponse response = _client.Execute(request);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(&quot;Invalid response&quot;);
        }
        
        return response.Content;
    }
}</pre>
<p>As I said, it's a very simple example. The important part here is that if I get a valid response I want to return the content (which should be the GUID), otherwise I want to throw an Exception. Here are the tests:</p>
<pre title="code" class="brush: csharp">
[TestFixture]
class GreetingClientTest
{
    [Test]
    public void Throws_exception_if_response_not_OK()
    {
        var mock = new Mock&lt;IRestClient&gt;();
        mock.Setup(x =&gt; x.Execute(It.IsAny&lt;IRestRequest&gt;()))
            .Returns(new RestResponse {StatusCode = HttpStatusCode.BadRequest});

        var client = new GuidClient(mock.Object);
        Assert.Throws&lt;Exception&gt;(() =&gt; client.Execute());
    }

    [Test]
    public void Returns_content_if_response_is_OK()
    {
        string content = Guid.NewGuid().ToString();
        var mock = new Mock&lt;IRestClient&gt;();
        mock.Setup(x =&gt; x.Execute(It.IsAny&lt;IRestRequest&gt;()))
            .Returns(new RestResponse
                         {
                             StatusCode = HttpStatusCode.OK,
                             Content = content
                         });

        var client = new GuidClient(mock.Object);
        var result = client.Execute();
        Assert.AreEqual(content, result);
    }
}
</pre>
<p>It&rsquo;s easy to notice how simple the RestSharp abstractions make our job of testing. Mock the IRestClient to return the desired RestResponse and it's good to go.</p>
<p>When creating my first client using the HttpClient I wanted to pass the HttpClient as a constructor parameter in the same manner I did with IRestClient. That&rsquo;s when I noticed that the HttpClient doesn&rsquo;t implement any interfaces other than IDisposable. Hum, no interfaces? So how can I mock this thing? Good thing Glenn Block was ready to help on twitter:</p>
<blockquote>
<p>&ldquo;@gblock: @perezgb @howard_dierking with web api you can pass a fake message handler to the client to test.&rdquo;</p>
</blockquote>
<p>So Glenn also sent me the <a href="http://codepaste.net/d5iy14">code from one of his talks where he creates a fake handler</a> in order to help testing with the HttpClient. So taking his code I created an HttpClient version of my service and tests:</p>
<pre title="code" class="brush: csharp">
public class GuidHttpClient
{
    private readonly HttpClient _client;

    public GuidHttpClient(HttpClient client)
    {
        _client = client;
    }

    public string Execute()
    {
        var request = new HttpRequestMessage { RequestUri = new Uri(&quot;http://localhost/guidservice&quot;) };
        Task&lt;HttpResponseMessage&gt; task = _client.SendAsync(request);
        HttpResponseMessage response = task.Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            throw new Exception(&quot;Invalid response&quot;);
        }
        return response.Content.ReadAsStringAsync().Result;
    }
}</pre>
<p>And here are the tests:</p>
<pre title="code" class="brush: csharp">
[TestFixture]
public class GuidHttpClientTest
{
    [Test]
    public void Throws_exception_if_response_not_OK()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        var httpClient = new HttpClient(new FakeHandler
                                            {
                                                Response = response,
                                                InnerHandler = new HttpClientHandler()
                                            });

        var client = new GuidHttpClient(httpClient);
        Assert.Throws&lt;Exception&gt;(() =&gt; client.Execute());
    }

    [Test]
    public void Returns_content_if_response_is_OK()
    {
        string content = Guid.NewGuid().ToString();
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        response.Content = new StringContent(content);

        var httpClient = new HttpClient(new FakeHandler
        {
            Response = response,
            InnerHandler = new HttpClientHandler()
        });

        var client = new GuidHttpClient(httpClient);
        string result = client.Execute();
        Assert.AreEqual(content, result);
    }
}</pre>
<p>And this is the fake message handler:</p>
<pre title="code" class="brush: csharp">
public class FakeHandler : DelegatingHandler
{
    public HttpResponseMessage Response { get; set; }

    protected override Task&lt;HttpResponseMessage&gt; SendAsync(HttpRequestMessage request,
                                                           CancellationToken cancellationToken)
    {
        if (Response == null)
        {
            return base.SendAsync(request, cancellationToken);
        }

        return Task.Factory.StartNew(() =&gt; Response);
    }
}</pre>
<p>Ok, so mission accomplished! I was able to write my service and tests using the HttpClient. One thing I really like is the russian doll model, kinda like the one you can find on FubuMVC, that the DelegatingHandler makes possible. On the other hand, I still like my RestSharp tests better, they seem cleaner but maybe that&rsquo;s just me. It's the way I've been writing code for a while and I feel comfortable with it. Some times though, we have to push ourselves out of our comfort zone and try different things, right?</p>
<p>Please take all this with a grain of salt as I don&rsquo;t consider myself to be any kind of expert in the ASP.NET Web Api. Also this is only my first stab at the HttpClient, I plan to keep going with my tests. I'll let you know if I come across anything interesting ;-)</p>
</div>