# StazorPages

StazorPages Middleware allows you to create a static website with  [ASP.NET Core 5.0](https://docs.microsoft.com/en-us/aspnet/core/release-notes/aspnetcore-5.0).
Dynamic content is pulled in from a headless CMS, a CaaS (Content-as-a-Service) platform or another third-party content repository on the first request and static HTML pages are created for future requests. A webhook enables the deletion of static pages on content updates. 

## How it works

![Stazor Pages Lifecycle](https://raw.githubusercontent.com/emmanueltissera/stazorpages/master/Assets/Images/stazor-pages-lifecycle.gif "Stazor Pages Lifecycle")

## Prerequisites

* ASP.NET Core 5.0

## NuGet Package

This middleware is available as a NuGet Package at [https://www.nuget.org/packages/StazorPages](https://www.nuget.org/packages/StazorPages).

## Works with a plugin

This middleware best works with a plugin built for each content source such as a headless CMS, a CaaS platform or other third-party content repository

## Develop your own plugin

This Middleware could be used to build your own plugin. [StazorPages.Heartcore](https://github.com/emmanueltissera/stazorpages.heartcore) plugin is a good example. 

Watch this space for more comprehensive documentation!

## Available plugins

* [StazorPages.Heartcore](https://github.com/emmanueltissera/stazorpages.heartcore) plugin for [Umbraco Heartcore](https://umbraco.com/products/umbraco-heartcore/) - Available as a [NuGet package](https://www.nuget.org/packages/StazorPages.Heartcore)


## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)