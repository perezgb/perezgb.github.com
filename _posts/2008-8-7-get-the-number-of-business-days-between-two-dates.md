---
layout: post
title: "Get the number of business days between two dates"
description: ""
date: 2008-08-07 12:00:00 UTC
category: 
tags: [.net, c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Here&rsquo;s a simple way of getting the number of business days between two dates  using C#.</p>
<pre title="code" class="brush: csharp">
DateTime dtBegin = new DateTime(2008, 8, 6);
            DateTime dtEnd = new DateTime(2008, 8, 13);

            int dayCount = 0;

            //while the End date is not reached
            while (dtEnd.CompareTo(dtBegin) &gt; 0)
            {
                //check if the day is not a weekend day
                if ((dtBegin.DayOfWeek != DayOfWeek.Saturday) &amp;&amp; (dtBegin.DayOfWeek != DayOfWeek.Sunday))
                {
                    dayCount++;
                }
                //go to next day
                dtBegin = dtBegin.AddDays(1);
            }

</pre>
<p>You can enhance this method consulting a list of holidays to check if the  current date is indeed a business day. For this you have to have the list of  holidays somewhere. In our example I&rsquo;ll create a fake method that will provide  the holidays list.</p>
<pre title="code" class="brush: csharp">
public static void CalculateNumberOfWeekdays()
        {
            DateTime dtBegin = new DateTime(2008, 8, 6);
            DateTime dtEnd = new DateTime(2008, 8, 13);
            List&lt;DateTime&gt; holidays = GetHolidays();

            int dayCount = 0;

            //while the End date is not reached
            while (dtEnd.CompareTo(dtBegin) &gt; 0)
            {
                //check if the day is not a weekend day
                if ((dtBegin.DayOfWeek != DayOfWeek.Saturday) 
                    &amp;&amp; (dtBegin.DayOfWeek != DayOfWeek.Sunday)
                    &amp;&amp; (!holidays.Contains(dtBegin)))
                {
                    dayCount++;
                }
                //go to next day
                dtBegin = dtBegin.AddDays(1);
            }
            Console.WriteLine(dayCount);
        }

        public static List&lt;DateTime&gt; GetHolidays()
        {
            List&lt;DateTime&gt; holidays = new List&lt;DateTime&gt;();
            holidays.Add(new DateTime(2008, 8, 7));
            return holidays;
        }            

</pre>
<p>See you next time.</p>
</div>