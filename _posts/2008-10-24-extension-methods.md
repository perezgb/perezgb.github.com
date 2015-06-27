---
layout: post
title: "Extension Methods"
description: ""
date: 2008-10-24 12:00:00 UTC
category: 
tags: [.net, c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Extension methods are one of my favorite features of C# 3.0, they allow you  to create new methods and plug them in classes already defined without having to  derive them or even change it&rsquo;s source code.</p>
<p>Everyone uses the DateTime class and a lot of those people have a class with  handy methods to handle DateTime. Many of the handy methods would look better in  the DateTime class but you can&rsquo;t change it&rsquo;s source code and you don&rsquo;t want to  derive from it and create a new DateTime class (of course). With extension  methods you can plug these new methods in the DateTime class.</p>
<p>Let&rsquo;s create a simple method named &rsquo;&rsquo;&lsquo;DaysSince&rdquo; that operates on two dates  returning the number of days from one date to the other. The logic of the method  is really simple:</p>
<pre title="code" class="brush: csharp">
    public static class DateUtil
    {
        public static int DaysSince(DateTime d1, DateTime d2)
        {
            TimeSpan ts = d1.Subtract(d2);
            return ts.Days;
        }
    }
</pre>
<p>You would use this class like this:</p>
<pre title="code" class="brush: csharp">
DateTime today = DateTime.Now;
DateTime pastDate = new DateTime(2008, 10, 9);
Console.WriteLine(DateUtil.DaysSince(today, pastDate);

</pre>
<p>My goal now is to be able to do this:</p>
<pre title="code" class="brush: csharp">
DateTime today = DateTime.Now;
DateTime pastDate = new DateTime(2008, 10, 9);
Console.WriteLine(today.DaysSince(pastDate));</pre>
<p>To make this change just put a &rsquo;&rsquo;&lsquo;this&rsquo;&rsquo;&rsquo; keyword in front of the first  parameter of the static method. Now it should be like this:</p>
<pre title="code" class="brush: csharp">
public static class DateUtil
{
    public static int DaysSince(this DateTime d1, DateTime d2)
    {
        TimeSpan ts = d1.Subtract(d2);
        return ts.Days;
    }
}

</pre>
<p>That is it, you can now make a call to the DaysSince method from any DateTime  instance variable.</p>
<p>Here a the rules for creating a ExtensionMethod:</p>
<ol>
    <li>Create a public static class (you call call it anything you want)</li>
    <li>Create the method you want as a public static method</li>
    <li>The first parameter of the method must always the a &rsquo;&rsquo;&lsquo;this&rsquo;&rsquo;&rsquo; keyword  followed by the class you want to plug the method to and the name of the  parameter</li>
    <li>Whenever you want to use the extension method the &rsquo;&rsquo;&lsquo;namespace&rsquo;&rsquo;&rsquo; in which  the class is into must be referenced</li>
</ol>
<p>If you want to create a method &rsquo;&rsquo;&lsquo;WeeksAgo&rsquo;&rsquo;&rsquo; that let you get a new DateTime  x weeks before the a certain date all you need to do is this:</p>
<pre title="code" class="brush: csharp">
public static class DateUtil
{
    public static int DaysSince(this DateTime d1, DateTime d2)
    {
        TimeSpan ts = d1.Subtract(d2);
        return ts.Days;
    }
    public static DateTime WeeksAgo(this DateTime d1, int numberOfWeeks)
    {
        DateTime newDate = d1.AddDays(-numberOfWeeks * 7);
        return newDate;
    }
}
</pre>
<p>And then you can use it like this:</p>
<pre title="code" class="brush: csharp">
Console.WriteLine(today.WeeksAgo(2).ToString());</pre>
<p>Really cool addition to your utility belt, isn&rsquo;t it?</p>
</div>