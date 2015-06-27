---
layout: post
title: "DateTime Testing Strategies"
description: ""
category: 
tags: []
---
{% include JB/setup %}

<div id="post">
<p>&nbsp;Trying to test code that is dependent on DateTime is an interesting challenge. Consider the simple class:</p>
<pre title="code" class="brush: csharp">
public class Token
{
    public Guid Id { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsExpired
    {
        get { return ExpiresAt &lt; DateTime.Now; }
    }

    public Token(Guid id, DateTime expiresAt)
    {
        Id = id;
        ExpiresAt = expiresAt;
    }
}</pre>
<p>&nbsp;In order to test the IsExpired property there are three scenarios to be considered:</p>
<ol>
    <li>ExpiresAt &gt; Now: Not expired</li>
    <li>ExpiresAt &lt; Now: Expired</li>
    <li>ExpiresAt == Now: Expired</li>
</ol>
<p>&nbsp;Scenario 1 and 2 are easy to test:</p>
<pre title="code" class="brush: csharp">
[TestFixture]
public class TokenTests
{
    [Test]
    public void Token_is_expired_when_ExpiresAt_less_than_now()
    {
        var token = new Token(Guid.NewGuid(), DateTime.Now.AddDays(1));
        Assert.IsFalse(token.IsExpired);
    }

    [Test]
    public void Token_is_not_expired_when_ExpiresAt_greater_than_now()
    {
        var token = new Token(Guid.NewGuid(), DateTime.Now.AddDays(-1));
        Assert.IsTrue(token.IsExpired);
    }
}
</pre>
<p>Just by adding and subtracting a day it possible to accomplish these tests.</p>
<p>Scenario 3 is the problem, DateTime.Now is called twice so you can&rsquo;t really be sure the values will be the same. This is in no way a new problem what I want to show in this post are the different strategies I found to solve the problem so you can pick the one you fits the best your needs.</p>
<h2>Approach 1</h2>
<p>The simplest approach is to define an ITimeProvider interface and it&rsquo;s default implementation.</p>
<pre title="code" class="brush: csharp">
public interface ITimeProvider
{
    DateTime Now();
}

class DefaultTimeProvider : ITimeProvider
{
    public DateTime Now()
    {
        return DateTime.Now;
    }
}</pre>
<p>With this approach any mocking framework can be used to test our equality:</p>
<pre title="code" class="brush: csharp">
[TestFixture]
public class TokenTests
{
    [Test]
    public void Token_is_expired_when_ExpiresAt_is_equals_to_now()
    {
        var dateTime = new DateTime(2012, 1, 1);
        var mock = new Mock&lt;ITimeProvider&gt;();
        mock.Setup(x =&gt; x.Now()).Returns(dateTime);
        var token = new Token(Guid.NewGuid(), dateTime);
        Assert.IsTrue(token.IsExpired);
    }
}</pre>
<p>This is actually the most common approach to testing and usually it&rsquo;s the one I use. The DateTime problem is a special case though. When DateTime.Now is used in POCOs, like in the case of our example, I find it awkward to have to use an IoC container to inject the provider. Maybe you like, it's just not for me :-)</p>
</div>