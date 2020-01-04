# GpxViewer
A .gpx file viewer plugin for Total Commander (TC)

## Overview
This is a TC lister plugin to preview .gpx format file of cycling tracks on Baidu map.
gpx is the defalut format of [Xingzhe行者](http://imxingzhe.com/) Android App. C# webbrowser control is used for map display.  Javascript in map.html draw the polylines and Start/End point. .Net Plugin Interface play a very important role to communicate between unmanaged code(c++/delphi) and managed code (C#). It is a fantastic software with very detail document. Thank the author Oleg Yuvashev.

## Tool Chain
1. [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
2. [.Net framework 4.8](https://dotnet.microsoft.com/download/dotnet-framework/net48)
3. [.Net Plugin Interface 1.4 by Oleg Yuvashev](http://totalcmd.net/plugring/TCdotNetInterface.html)   
4. [Total Commander 9.22a](https://www.ghisler.com/index.htm)
 
## Usage
1. Install the plugin interface in /Deployment
2. [Apply Baidu map key](http://lbsyun.baidu.com/index.php?title=jspopular/guide/getkey)
3. Replace "YourKey" in /map.html to your Baidu map key.
4. Build the project in VS
5. Install GpxViewer.wlx to TC 

## Snapshot
Satellite view
![](https://github.com/lixshnew/GpxViewer/blob/master/Snapshot/TC.JPG)

Map view (default)
![](https://github.com/lixshnew/GpxViewer/blob/master/Snapshot/2017.JPG)

Ctrl+Q  Embedded lister to TC file list area
![](https://github.com/lixshnew/GpxViewer/blob/master/Snapshot/TC_Q.JPG)

