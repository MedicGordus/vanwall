To publish for windows x64 use:

dotnet publish -c Release -r win-x64

This is windows specific because of the Lamport entropy sources. However, these can be removed in order to make it cross platform capable.

If this is running on a headless OS, you should remove the input devices as entropy sources and instead add a few others.