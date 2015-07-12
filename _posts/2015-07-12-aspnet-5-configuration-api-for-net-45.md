---
layout: post
title: "ASP.NET 5 Configuration API for .NET 4.5"
description: ""
category: 
tags: [aspnet5, aspnet, configuration]
---
{% include JB/setup %}

As I've stated in previous posts I think the new ASP.NET 5 [Configuration API](https://github.com/aspnet/Configuration) great. This thing is I want to use it now, before I can use 4.6 on all my projects. And even then, we have some projects that will not be upgraded anytime soon. On the other hand, it was in .NET 4.5 I'd be ok.

So I decided to fork the repo and check how much I would have to change it to run on 4.5. As it turns out, it wasn't too bad. I had to create the proper msbuild projects (csproj), modify a few areas of the code where C# 6 was being used and install the proper nuget packages. The code compiles and the tests pass.

I don't mean to redo what Microsoft did in anyway, I'm just interested in maintaining a 4.5 version of the API while 4.5 remains relevant. It's all about making my life easy.

I'm not creating nuget packages for it as I don't want to create any confusion on nuget about what is official or not. If you have a need for this like I do, just feel free to clone my forked repo and rebuild it on your machine.

If you like it or find that you have a use for it, please ping me on twitter. I'd be curious to know if I'm the only mad man out there that wanted something like this.

My changes are on the net45 branch of the repository. Have a look at it here:

[https://github.com/perezgb/Configuration/tree/net45](https://github.com/perezgb/Configuration/tree/net45)

Well, that it for now folks. Have fun and see you next time.