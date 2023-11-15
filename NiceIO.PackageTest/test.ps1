dotnet pack .. -p:NuspecFile=NiceIO.nuspec
dotnet add package NiceIO --source ..\bin\release --package-directory packages
if ((dotnet run) -ne 'Path is a/b/c/file.txt') { throw 'it broke' }
''
'Everything is shiny'
