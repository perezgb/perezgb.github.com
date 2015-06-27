---
layout: post
title: "Design Pattern: Singleton"
description: ""
date: 2008-07-16 12:00:00 UTC
category: 
tags: [c#, design patterns]
comments: true
---
{% include JB/setup %}

<div id="post">
<p><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">This is the first post on a series about Design Patterns using C#. I will not publish all the articles in a sequence but you can expect from time to time to have a new post about some pattern.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">The first pattern I want to talk about is the Singleton.</p>
<h2 style="margin: 0px 0px 1em; padding: 0px; font-weight: normal; font-family: 'Trebuchet MS',Verdana,sans-serif; font-size: 150%; color: rgb(125, 0, 10);">Objective</h2>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">You need a singleton when your application require one and only one instance of a single class and also that this instance can be accessed whenever necessary.</p>
<h2 style="margin: 0px 0px 1em; padding: 0px; font-weight: normal; font-family: 'Trebuchet MS',Verdana,sans-serif; font-size: 150%; color: rgb(125, 0, 10);">Case Scenario</h2>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Imagine that you want a Logger class that will store all messages in a internal list. You need the hold the all the messages in a single place. If you had more than one instance of a Logger class you would have the messages scattered over the various instances. To avoid this will use a singleton.</p>
<h2 style="margin: 0px 0px 1em; padding: 0px; font-weight: normal; font-family: 'Trebuchet MS',Verdana,sans-serif; font-size: 150%; color: rgb(125, 0, 10);">The Solution</h2>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">We are going to create a Logger class in our example.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">The first issue the singleton should handle is how to prevent users from creating multiple instances of a class. To achieve this you have to block (hide) the constructor. This can be done making it private.</p>
<pre title="code" class="brush: csharp">
private Logger() { }
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">Now the that the constructor is private how can you instantiate the class? You are right, you can&rsquo;t do it from outside the class so we are going to do it from the inside. Since we need to keep the one instance we create we need a variable to hold it. We are going to declare this variable as static so that we don&rsquo;t need the class to be instantiated in order to keep the value.</span></p>
<pre title="code" class="brush: csharp">
//this will hold our unique instance
private static Logger instance = null;
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">The next step is to come up with a way to create the only instance and at the same time gain Access to that instance. This can be done using a static method that will check the instance variable and if it is null it will create a new instance and return it, if it&rsquo;s not null it will return the previously create instance.</span></p>
<pre title="code" class="brush: csharp">
public static Logger Instance()
        {
            //if the instance is null you have to instantiate it
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">This is all we need for our singleton but we still need to add the functionality for the Logger class. We need a list to store the messages, a method to add new messages and a method to print all the messages already stored. Here is the code for it:</span></p>
<pre title="code" class="brush: csharp">
//holds all the messages
        private List&lt;string&gt; messages = new List&lt;string&gt;();

        //add a new message to the Logger
        public void Log(string message)
        {
            messages.Add(message);
        }

        public void PrintAllMessages()
        {
            foreach (string message in messages)
            {
                Console.WriteLine(message);
            }
        }</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">Here is the code for the whole class:</span></p>
<pre title="code" class="brush: csharp">
public class Logger
    {
        private static Logger instance = null;

        private Logger() { }

        public static Logger Instance()
        {
            //if the instance is null you have to instantiate it
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        //holds all the messages
        private List&lt;string&gt; messages = new List&lt;string&gt;();

        //add a new message to the Logger
        public void Log(string message)
        {
            messages.Add(message);
        }

        public void PrintAllMessages()
        {
            foreach (string message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">
<h2 style="margin: 0px 0px 1em; padding: 0px; font-weight: normal; font-family: 'Trebuchet MS',Verdana,sans-serif; font-size: 150%; color: rgb(125, 0, 10);">How to use it</h2>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Using the singleton is easy, the only difference from normal class use is that when you need an instance of the object you will not be able to call the constructor (it&rsquo;s private now) so you need to request an instance through the Instance() method that we created.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">In our usage examples I&rsquo;ll declare two logger variables and pass different messages to each one. Since there&rsquo;s only one instance both these variables will have references to the same object. This can be verified when you call the PrintAllMessages method of any of the loggers. You&rsquo;ll notice that they will print all the messages that were passed to any of the variables.</p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">Here is the code so you can test it at home:</p>
<pre title="code" class="brush: csharp">
 class Program
    {
        static void Main(string[] args)
        {
            Logger logger1 = Logger.Instance();
            Logger logger2 = Logger.Instance();

            logger1.Log(&quot;loading one&quot;);
            logger2.Log(&quot;loading two&quot;);

            logger1.Log(&quot;Success&quot;);
            logger2.Log(&quot;Failure&quot;);

            logger1.PrintAllMessages();
        }
    }
</pre>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;"><span style="border-collapse: separate; color: rgb(102, 102, 102); font-family: Verdana; font-size: 11px; font-style: normal; font-variant: normal; font-weight: normal; letter-spacing: normal; line-height: 18px; orphans: 2; text-align: left; text-indent: 0px; text-transform: none; white-space: normal; widows: 2; word-spacing: 0px;" class="Apple-style-span">That&rsquo;s it for the Singleton, I hope this is useful to someone somewhere :-)</span></p>
</span></p>
<p style="margin: 0px 0px 1.5em; padding: 0px; font-weight: normal;">&nbsp;</p>
</span></p>
</div>