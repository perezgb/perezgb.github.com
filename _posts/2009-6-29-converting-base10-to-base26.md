---
layout: post
title: "Converting Base10 to Base26"
description: ""
date: 2009-06-29 12:00:00 UTC
category: 
tags: [c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>There is a post I answered on the asp.net forums I thought was really interesting because it reminded me of my college times. Actually I'm pretty sure that the guy who posted the question was trying to get an answer to his homework, but it doesn't matter, I had fun doing it :-).  The problem was to do an ascending sequence using the alphabet (which is a Base26). Something like: A,B,C...Z,AA,AB,AC...AZ,BA,BB,BC...BZ and so on. Well the easiest way to do this is to convert our regular Base10 numbers to Base26 because using Base10 we can do a plain for loop and cout up to the number we want and while doing the loop we can convert the Base10 to Base26. Here is the algorithm:&nbsp;</p>
<pre title="code" class="brush: csharp">
        public static string NumberToString(int value)
        {
            StringBuilder sb = new StringBuilder();
            
            do
            {
                value--;
                int remainder = 0;
                value = Math.DivRem(value, 26, out remainder);
                sb.Insert(0, Convert.ToChar('A' + remainder));
                
            } while (value &gt; 0);

            return sb.ToString();
        }</pre>
<p>I know this isn't very useful for everyday programming but it was fun to remember the time I was still in college solving this kind of problem. It was a really fun exercise.</p>
</div>