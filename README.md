# NiceIO
For when you've had to use System.IO one time too many.

I need to make c# juggle files & directories around a lot. It has to work on osx, linux and windows. It always hurts, and I've never enjoyed it. NiceIO is an attempt to fix that. It's a single file library, no binaries, no .csproj's, no nuget specs, or any of that. .NET Framework 3.5 & Unity compatible. Whenever dealing with files makes you cringe, just grab NiceIO.cs, throw it in your project and get on with your business.

This project is in a very early state and the API is very far from stable.

Basic usage:
```c#
//paths are immutable
Path path1 = new Path(@"/var/folders/something");
// /var/folders/something

//use back,forward,or trailing slashes,  doesnt matter
Path path2 = new Path(@"/var\folders/something///");
// /var/folders/something

//semantically the same
path1 == path2;
// true

//build paths
path1.Combine("dir1/dir2");
// /var/folders/something/dir1/dir2

//handy accessors
Path.HomeDirectory;
// /Users/lucas

//all operations return their destination, so they fluently daisychain
Path myfile = Path.HomeDirectory.CreateDirectory("mysubdir").CreateFile("myfile.txt");
// /Users/lucas/mysubdir/myfile.txt

//common operations you know and expect
myfile.Exists();
// true

//you will never again have to look up if .Extension includes the dot or not
myfile.ExtensionWithDot;
// ".txt"

//getting parent directory
Path dir = myfile.Parent();
// /User/lucas/mysubdir

//copying files,
myfile.Copy("myfile2");
// /Users/lucas/mysubdir/myfile2

//into not-yet-existing directories
myfile.Copy("hello/myfile3");
// /Users/lucas/mysubdir/hello/myfile3

//listing files
dir.Files(SearchOption.AllDirectories);
// { /Users/lucas/mysubdir/myfile.txt, 
//   /Users/lucas/mysubdir/myfile2, 
//   /Users/lucas/mysubdir/hello/myfile3 }

//or directories
dir.Directories();
// { /Users/lucas/mysubdir/hello }

//or both
dir.Contents(SearchOption.AllDirectories);
// { /Users/lucas/mysubdir/myfile.txt, 
//   /Users/lucas/mysubdir/myfile2, 
//   /Users/lucas/mysubdir/hello/myfile3, 
//   /Users/lucas/mysubdir/hello }

//copy entire directory, and listing everything in the copy
myfile.Parent().Copy("anotherdir").Files(SearchOption.AllDirectories);
// { /Users/lucas/anotherdir/myfile, 
//   /Users/lucas/anotherdir/myfile.txt, 
//   /Users/lucas/anotherdir/myfile2, 
//   /Users/lucas/anotherdir/hello/myfile3 }
```

NiceIO is MIT Licensed.
