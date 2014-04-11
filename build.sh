#!/bin/sh
xbuild NLog.sln /verbosity:minimal
mono Tests/Libraries/NSpec/NSpecRunner.exe Tests/bin/Debug/Tests.dll
